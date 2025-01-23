using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TheiaEditor.GameProject
{
    [DataContract(Name = "Game")]
    class Project: ViewModelBase
    {
        public Project(string name, string path) 
        {
            Name = name;
            Path = path;

            _scenes.Add(new Scene(this, "Default Scene"));
        }

        public static string Extension { get; } = ".theia";

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public string Path { get; private set; }

        [DataMember(Name = "Scenes")]
        private ObservableCollection<Scene> _scenes = new ObservableCollection<Scene>();
        public ReadOnlyCollection<Scene> Scenes { get; }

        public string FullPath => $"{Path}{Name}{Extension}";
    }
}
