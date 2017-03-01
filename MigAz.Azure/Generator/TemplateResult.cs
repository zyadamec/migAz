using MigAz.Azure.Arm;
using MigAz.Azure.Interface;
using MigAz.Azure.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Text;
using MigAz.Core.Interface;

namespace MigAz.Azure.Generator
{
    public class TemplateResult
    {
        private Guid _ExecutionGuid = Guid.NewGuid();
        private String _OutputPath = AppDomain.CurrentDomain.BaseDirectory;
        private List<ArmResource> _Resources = new List<ArmResource>();
        private List<CopyBlobDetail> _CopyBlobDetails = new List<CopyBlobDetail>();
        private Dictionary<string, Parameter> _Parameters = new Dictionary<string, Parameter>();
        private List<String> _Messages = new List<string>();
        private ISubscription _SourceSubscription;
        private ISubscription _TargetSubscription;
        private ArmResourceGroup _TargetResourceGroup;
        private ILogProvider _logProvider;

        private TemplateResult() { }
        public TemplateResult(ISubscription sourceSubscription, ISubscription targetSubscription, ArmResourceGroup targetResourceGroup, ILogProvider logProvider, string outputPath)
        {
            _SourceSubscription = sourceSubscription;
            _TargetSubscription = targetSubscription;
            _TargetResourceGroup = targetResourceGroup;
            _OutputPath = outputPath;
            _logProvider = logProvider;
        }

        public Guid ExecutionGuid
        {
            get { return _ExecutionGuid; }
        }

        public string OutputPath
        {
            get { return _OutputPath; }
            set { _OutputPath = value; }
        }

        public List<String> Messages
        {
            get { return _Messages; }
            set { _Messages = value; } 
        }

        public List<ArmResource> Resources { get { return _Resources; } }
        public List<CopyBlobDetail> CopyBlobDetails { get { return _CopyBlobDetails; } }
        public Dictionary<string, Parameter> Parameters { get { return _Parameters; } }
        public ISubscription SourceSubscription { get { return _SourceSubscription; } }
        public ArmResourceGroup TargetResourceGroup { get { return _TargetResourceGroup; } }

        public static string GetCopyBlobDetailPath(string outputPath)
        {
            return Path.Combine(outputPath, "copyblobdetails.json");
        }

        public string GetCopyBlobDetailPath()
        {
            return GetCopyBlobDetailPath(_OutputPath);
        }

        public static string GetTemplatePath(string outputPath)
        {
            return Path.Combine(outputPath, "export.json");
        }

        public string GetTemplatePath()
        {
            return GetTemplatePath(_OutputPath);
        }

        public static string GetInstructionPath(string outputPath)
        {
            return Path.Combine(outputPath, "DeployInstructions.html");
        }

        public string GetInstructionPath()
        {
            return GetInstructionPath(_OutputPath);
        }

        public static bool OutputFilesExist(string outputPath)
        {
            return File.Exists(GetInstructionPath(outputPath)) || File.Exists(GetTemplatePath(outputPath)) || File.Exists(GetInstructionPath(outputPath));
        }

        public string GetTemplateString()
        {
            Template template = new Template();
            template.resources = this.Resources;
            template.parameters = this.Parameters;

            // save JSON template
            string jsontext = JsonConvert.SerializeObject(template, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });
            jsontext = jsontext.Replace("schemalink", "$schema");

            return jsontext;
        }

        public JObject GenerateTemplate()
        {
            return JObject.Parse(GetTemplateString());
        }
        
        public void Write()
        {
            if (!Directory.Exists(_OutputPath))
            {
                throw new ArgumentException("Output path '" + _OutputPath + "' does not exist.");
            }

            StreamWriter templateWriter = null;
            try
            {
                templateWriter = new StreamWriter(GetTemplatePath());
                templateWriter.Write(GetTemplateString());
            }
            finally
            {
                if (templateWriter != null)
                {
                    templateWriter.Close();
                    templateWriter.Dispose();
                }
            }

            // save blob copy details file
            StreamWriter copyBlobDetailWriter = null;
            try
            {
                string jsontext = JsonConvert.SerializeObject(this.CopyBlobDetails, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });
                copyBlobDetailWriter = new StreamWriter(GetCopyBlobDetailPath());
                copyBlobDetailWriter.Write(jsontext);
            }
            finally
            {
                if (copyBlobDetailWriter != null)
                {
                    copyBlobDetailWriter.Close();
                    copyBlobDetailWriter.Dispose();
                }
            }

            var instructionPath = Path.Combine(_OutputPath, "DeployInstructions.html");
            StreamWriter instructionWriter = null;
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "MigAz.Azure.Generator.AsmToArm.DeployDocTemplate.html";
                string instructionContent;

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    instructionContent = reader.ReadToEnd();
                }

                instructionContent = instructionContent.Replace("{subscriptionId}", _TargetSubscription.SubscriptionId.ToString());
                instructionContent = instructionContent.Replace("{templatePath}", GetTemplatePath());
                instructionContent = instructionContent.Replace("{blobDetailsPath}", GetCopyBlobDetailPath());
                instructionContent = instructionContent.Replace("{resourceGroupName}", _TargetResourceGroup.GetFinalTargetName());
                instructionContent = instructionContent.Replace("{location}", _TargetResourceGroup.Location.Name);
                instructionContent = instructionContent.Replace("{migAzPath}", AppDomain.CurrentDomain.BaseDirectory);
                instructionContent = instructionContent.Replace("{migAzMessages}", BuildMigAzMessages());

                if (_TargetSubscription.AzureEnvironment == AzureEnvironment.AzureCloud)
                    instructionContent = instructionContent.Replace("{migAzAzureEnvironmentSwitch}", String.Empty); // Default Azure Environment in Powershell, no AzureEnvironment switch needed
                else
                    instructionContent = instructionContent.Replace("{migAzAzureEnvironmentSwitch}", " -Environment \"" + _TargetSubscription.AzureEnvironment.ToString() + "\"");

                instructionWriter = new StreamWriter(instructionPath);
                instructionWriter.Write(instructionContent);
            }
            finally
            {
                if (instructionWriter != null)
                {
                    instructionWriter.Close();
                    instructionWriter.Dispose();
                }
            }
        }

        private string BuildMigAzMessages()
        {
            if (this.Messages.Count == 0)
                return String.Empty;

            StringBuilder sbMigAzMessageResult = new StringBuilder();

            sbMigAzMessageResult.Append("<p>MigAz has identified the following advisements during template generation for review:</p>");

            sbMigAzMessageResult.Append("<p>");
            sbMigAzMessageResult.Append("<ul>");
            foreach (string migAzMessage in this.Messages)
            {
                sbMigAzMessageResult.Append("<li>");
                sbMigAzMessageResult.Append(migAzMessage);
                sbMigAzMessageResult.Append("</li>");
            }
            sbMigAzMessageResult.Append("</ul>");
            sbMigAzMessageResult.Append("</p>");

            return sbMigAzMessageResult.ToString();
        }

        public bool IsProcessed(ArmResource resource)
        {
            return this.Resources.Contains(resource);
        }

        public void AddResource(ArmResource resource)
        {
            if (!IsProcessed(resource))
            {
                this.Resources.Add(resource);
                _logProvider.WriteLog("TemplateResult.AddResource", resource.type + resource.name + " added.");
            }
            else
                _logProvider.WriteLog("TemplateResult.AddResource", resource.type + resource.name + " already exists.");

        }

        internal object GetResource(Type type, string objectName)
        {
            foreach (ArmResource armResource in this.Resources)
            {
                if (armResource.GetType() == type && armResource.name == objectName)
                    return armResource;
            }

            return null;
        }
    }
}