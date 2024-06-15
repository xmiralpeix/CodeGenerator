using System.Collections.Generic;

namespace DesktopCodeGenerator.Dac
{
    public class SourceFilesLink
    {
        public string VisualName { get; set; }
        public List<Folder> Folders { get; set; }

        public List<ReplaceContent> Replacements { get; set; }

        public SourceFilesLink() { }
    }
}
