using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using TheiaEditor.Utility;

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

	class CreateProject : ViewModelBase
	{
		//TODO: Get path from installation location
		private readonly string _templatePath = $@"..\..\Data\ProjectTemplates";

		private string _projectName = "NewProject";
		public string ProjectName 
		{ 
			get { return _projectName; }
			set
			{
				if (_projectName != value)
				{
					_projectName = value;
					OnPropertyChanged(ProjectName);
				}
			}
		}

		private string _projectPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\TheiaProjects";
		public string ProjectPath
		{
			get => _projectPath;
			set
			{
				if (_projectPath != value)
				{
					_projectPath = value;
					OnPropertyChanged(ProjectPath);
				}
			}
		}

        private ObservableCollection<ProjectTemplate> _projectTemplates = [];
		public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates
		{ get; }

		public CreateProject()
		{
			ProjectTemplates = new ReadOnlyObservableCollection<ProjectTemplate>(_projectTemplates);

			try
			{
				var templateFiles = Directory.GetFiles(_templatePath, "Template.xml", SearchOption.AllDirectories);
                Debug.Assert(templateFiles.Length != 0);

				foreach (var file in templateFiles)
				{
					var template = Serializer.FromFile<ProjectTemplate>(file);
					template.IconPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "Icon.png"));
					template.Icon = File.ReadAllBytes(template.IconPath);
					template.ScreenshotPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "Screenshot.png"));
					template.Screenshot = File.ReadAllBytes(template.ScreenshotPath);
					template.ProjectPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), template.ProjectFile));

                    _projectTemplates.Add(template);
				}
			}
			catch (Exception ex)
			{
				//TODO: Logging
				Debug.WriteLine(ex.Message);
			}
		}
	}
}
