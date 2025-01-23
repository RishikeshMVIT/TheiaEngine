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
	class CreateProject : ViewModelBase
	{
        private bool ValidateProjectPath()
        {
            var path = ProjectPath;
			if (!Path.EndsInDirectorySeparator(path)) { path += @"\"; }
			path += $@"{ProjectName}";

			IsValid = false;
			if (string.IsNullOrEmpty(ProjectName.Trim()))
			{
				ErrorMessage = "Enter Project Name";
			}
			else if (ProjectName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				ErrorMessage = "Project Name contains invalid characters";
			}
			else if (ProjectPath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				ErrorMessage = "Project Patrh contains invalid characters";
			}
			else if (Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
			{
				ErrorMessage = "Project Folder already exists";
			}
			else
			{
				ErrorMessage = string.Empty;
				IsValid = true;
			}

			return IsValid;
			
        }

		public string CreateProjectFromTemplate(ProjectTemplate template)
		{
			ValidateProjectPath();
			if (!IsValid)
			{
				return string.Empty;
			}

			if (!Path.EndsInDirectorySeparator(ProjectPath)) { ProjectPath += @"\"; }

			var path = $@"{ProjectPath}{ProjectName}\";

			try
			{
				if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

                foreach (var folder in template.ProjectFolders)
                {
					Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path), folder)));
                }

				var dirInfo = new DirectoryInfo(path + @".Theia\");
				dirInfo.Attributes |= FileAttributes.Hidden;

				File.Copy(template.IconPath, Path.GetFullPath(Path.Combine(dirInfo.FullName, "Icon.png")));
                File.Copy(template.ScreenshotPath, Path.GetFullPath(Path.Combine(dirInfo.FullName, "Screenshot.png")));

				//var project = new Project(ProjectName, path);
				//Serializer.ToFile(project, path + ProjectName + Project.Extension);

                var project = File.ReadAllText(template.ProjectPath);
                var projectPath = Path.GetFullPath(Path.Combine(path, $"{ProjectName}{Project.Extension}"));
                project = string.Format(project, ProjectName, path);

                File.WriteAllText(projectPath, project);


                return path;

            }
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				//TODO: Logging
			}

			return string.Empty;
		}


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

				ValidateProjectPath();
			}
			catch (Exception ex)
			{
				//TODO: Logging
				Debug.WriteLine(ex.Message);
			}
		}


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
                    ValidateProjectPath();
                    OnPropertyChanged(nameof(ProjectName));
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
                    ValidateProjectPath();
                    OnPropertyChanged(nameof(ProjectPath));
                }
            }
        }

        private bool _isValid = false;
        public bool IsValid
        {
            get => _isValid;
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnPropertyChanged(nameof(IsValid));
                }

            }
        }

        private string _errorMessage = "";
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }

            }
        }

        private ObservableCollection<ProjectTemplate> _projectTemplates = [];
        public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates
        { get; }

    }
}
