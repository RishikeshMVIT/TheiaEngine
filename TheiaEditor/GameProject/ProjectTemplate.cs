using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TheiaEditor.GameProject
{
    [DataContract]
    public class ProjectTemplate
    {
        [DataMember]
        public string ProjectType { get; set; }
        [DataMember]
        public string ProjectFile { get; set; }
        [DataMember]
        public List<string> ProjectFolders { get; set; }
        [DataMember]
        public string ProjectPath { get; set; }
        [DataMember]
        public string IconPath { get; set; }
        [DataMember]
        public string ScreenshotPath { get; set; }

        public byte[] Icon { get; set; }
        public byte[] Screenshot { get; set; }

    }
}
