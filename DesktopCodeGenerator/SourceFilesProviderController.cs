using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DesktopCodeGenerator
{
    public class SourceFilesProviderController
    {
        private System.IO.DirectoryInfo Path { get; }
        private List<Dac.SourceFilesLink> _links;
        public SourceFilesProviderController()
        {
            Path = new System.IO.DirectoryInfo("P:\\SourceFiles");
            _links = new List<Dac.SourceFilesLink>();
        }

        internal void LoadLinks()
        {
            _links.Clear();
            foreach (var file in Path.GetFiles("*.json"))
            {
                string fileContent = System.IO.File.ReadAllText(file.FullName);
                _links.Add(JsonConvert.DeserializeObject<Dac.SourceFilesLink>(fileContent));
            }

        }

        public Dac.SourceFilesLink[] Links
        {
            get { return _links.ToArray(); }
        }

        internal string[] BuildSourceFiles(Dac.Folder oFolder)
        {
            List<string> files = new List<string>();

            foreach (var file in oFolder.SourceFiles)
            {
                files.Add(System.IO.Path.Combine(Path.FullName, file));
            }

            return files.ToArray();
        }
    }
}
