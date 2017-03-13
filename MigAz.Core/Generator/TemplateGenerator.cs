using System;
using System.Collections.Generic;
using System.IO;
using MigAz.Core.Interface;
using MigAz.Core.ArmTemplate;
using System.Threading.Tasks;

namespace MigAz.Core.Generator
{
    public abstract class TemplateGenerator
    {
        private Guid _ExecutionGuid = Guid.NewGuid();
        private List<ArmResource> _Resources = new List<ArmResource>();
        private Dictionary<string, ArmTemplate.Parameter> _Parameters = new Dictionary<string, ArmTemplate.Parameter>();
        private List<String> _Messages = new List<string>();
        private ILogProvider _logProvider;
        private IStatusProvider _statusProvider;
        private Dictionary<string, MemoryStream> _TemplateStreams = new Dictionary<string, MemoryStream>();

        public delegate Task AfterTemplateChangedHandler(TemplateGenerator sender);
        public event EventHandler AfterTemplateChanged;

        private TemplateGenerator() { }

        public TemplateGenerator(ILogProvider logProvider, IStatusProvider statusProvider)
        {
            _logProvider = logProvider;
            _statusProvider = statusProvider;
        }

        public ILogProvider LogProvider
        {
            get { return _logProvider; }
        }

        public IStatusProvider StatusProvider
        {
            get { return _statusProvider; }
        }

        public Guid ExecutionGuid
        {
            get { return _ExecutionGuid; }
        }

        public List<String> Messages
        {
            get { return _Messages; }
            set { _Messages = value; }
        }

        public List<ArmResource> Resources { get { return _Resources; } }
        public Dictionary<string, ArmTemplate.Parameter> Parameters { get { return _Parameters; } }
        public Dictionary<string, MemoryStream> TemplateStreams { get { return _TemplateStreams; } }

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

        public bool ResourceExists(Type type, string objectName)
        {
            object resource = GetResource(type, objectName);
            return resource != null;
        }

        public object GetResource(Type type, string objectName)
        {
            foreach (ArmResource armResource in this.Resources)
            {
                if (armResource.GetType() == type && armResource.name == objectName)
                    return armResource;
            }

            return null;
        }

        public abstract void UpdateArtifacts(IExportArtifacts artifacts);

        public void Write()
        {
            //if (!Directory.Exists(_OutputPath))
            //{
            //    throw new ArgumentException("Output path '" + _OutputPath + "' does not exist.");
            //}

            //StreamWriter templateWriter = null;
            //try
            //{
            //    templateWriter = new StreamWriter(GetTemplatePath());
            //    templateWriter.Write(GetTemplateString());
            //}
            //finally
            //{
            //    if (templateWriter != null)
            //    {
            //        templateWriter.Close();
            //        templateWriter.Dispose();
            //    }
            //}

            //// save blob copy details file
            //StreamWriter copyBlobDetailWriter = null;
            //try
            //{
            //    string jsontext = JsonConvert.SerializeObject(this.CopyBlobDetails, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });
            //    copyBlobDetailWriter = new StreamWriter(GetCopyBlobDetailPath());
            //    copyBlobDetailWriter.Write(jsontext);
            //}
            //finally
            //{
            //    if (copyBlobDetailWriter != null)
            //    {
            //        copyBlobDetailWriter.Close();
            //        copyBlobDetailWriter.Dispose();
            //    }
            //}

            //var instructionPath = Path.Combine(_OutputPath, "DeployInstructions.html");
            //StreamWriter instructionWriter = null;
            //try
            //{
            //    var assembly = Assembly.GetExecutingAssembly();
            //    var resourceName = "MigAz.Azure.Generator.AsmToArm.DeployDocTemplate.html";
            //    string instructionContent;

            //    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            //    using (StreamReader reader = new StreamReader(stream))
            //    {
            //        instructionContent = reader.ReadToEnd();
            //    }

            //    instructionContent = instructionContent.Replace("{subscriptionId}", _TargetSubscription.SubscriptionId.ToString());
            //    instructionContent = instructionContent.Replace("{templatePath}", GetTemplatePath());
            //    instructionContent = instructionContent.Replace("{blobDetailsPath}", GetCopyBlobDetailPath());
            //    instructionContent = instructionContent.Replace("{resourceGroupName}", _TargetResourceGroup.GetFinalTargetName());
            //    instructionContent = instructionContent.Replace("{location}", _TargetResourceGroup.Location.Name);
            //    instructionContent = instructionContent.Replace("{migAzPath}", AppDomain.CurrentDomain.BaseDirectory);
            //    instructionContent = instructionContent.Replace("{migAzMessages}", BuildMigAzMessages());

            //    if (_TargetSubscription.AzureEnvironment == AzureEnvironment.AzureCloud)
            //        instructionContent = instructionContent.Replace("{migAzAzureEnvironmentSwitch}", String.Empty); // Default Azure Environment in Powershell, no AzureEnvironment switch needed
            //    else
            //        instructionContent = instructionContent.Replace("{migAzAzureEnvironmentSwitch}", " -Environment \"" + _TargetSubscription.AzureEnvironment.ToString() + "\"");

            //    instructionWriter = new StreamWriter(instructionPath);
            //    instructionWriter.Write(instructionContent);
            //}
            //finally
            //{
            //    if (instructionWriter != null)
            //    {
            //        instructionWriter.Close();
            //        instructionWriter.Dispose();
            //    }
            //}
        }



        //The event-invoking method that derived classes can override.
        protected virtual void OnTemplateChanged()
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler handler = AfterTemplateChanged;
            if (handler != null)
            {
                handler(this, null);
            }
        }
    }
}