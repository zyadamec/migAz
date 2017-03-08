using System;
using System.Collections.Generic;
using System.IO;
using MigAz.Core.Interface;
using MigAz.Core.ArmTemplate;

namespace MigAz.Core.Generator
{
    public class TemplateResult
    {
        private Guid _ExecutionGuid = Guid.NewGuid();
        private String _OutputPath = AppDomain.CurrentDomain.BaseDirectory;
        private List<ArmResource> _Resources = new List<ArmResource>();
        private Dictionary<string, ArmTemplate.Parameter> _Parameters = new Dictionary<string, ArmTemplate.Parameter>();
        private List<String> _Messages = new List<string>();
        private ILogProvider _logProvider;
        private Dictionary<string, MemoryStream> _TemplateStreams;

        private TemplateResult() { }

        public TemplateResult(ILogProvider logProvider, string outputPath)
        {
            _OutputPath = outputPath;
            _logProvider = logProvider;
            _TemplateStreams = new Dictionary<string, MemoryStream>();
        }

        public ILogProvider LogProvider
        {
            get { return _logProvider; }
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

        public object GetResource(Type type, string objectName)
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