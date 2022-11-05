public class ImageFileService
{
        private string[] _files;
        private string _directoryPath;
        private FileCounter _fileCounter;

        public ImageFileService()
        {
            _fileCounter = new FileCounter();
        }

      public void SetDirectory(string dir)
        {
            _directoryPath = dir;
            Directory.SetCurrentDirectory(dir);
            _files = Directory.GetFiles(".");
            _fileCounter.CountFiles(_files);
            
        }

        public IEnumerable<string> GetPngs()=> GetFilesFromExtension("png");
        public IEnumerable<string> GetJpgs() => GetFilesFromExtension("jpg");
        public IEnumerable<string> GetPhotoFiles() =>GetFilesFromExtensions(new List<string> {"jpg", "png"});


        public IEnumerable<string> GetFilesFromExtension(string extension)
        {
            foreach (string file in _files)
            {
                if (file.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                {
                    yield return file;
                }
            }
     
        }

        public IEnumerable<string> GetFilesFromExtensions(IEnumerable<string> extensions)
        {  
            var output = new List<string>();
            foreach (string extension in extensions)
            {
                output.AddRange(GetFilesFromExtension(extension));
            }
            return output;
        }

        public List<string> GetAllFiles()
        {       
            List<string> allFiles = new List<string>();
            foreach (string file in _files)
            {
                allFiles.Add(file);
            }
            return allFiles;
        }
}