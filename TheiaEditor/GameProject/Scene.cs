﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TheiaEditor.GameProject
{
    [DataContract]
    class Scene : ViewModelBase
    {
        public Scene(Project project, string name) 
        {
            Debug.Assert(project != null);
            Project = project;
            Name = name;
        }

        
        private string _name;
        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }

            }
        }

        [DataMember]
        public Project Project { get; set; }

    }
}
