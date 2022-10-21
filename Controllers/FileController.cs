namespace DDControllers
{
    using static  DDControllers.UIController;

    public class FileController
    {
        private int pngCount = 0;
        private int jpgCount = 0;
        private int totalCount = 0;
        private int otherCount = 0;
        private string[] files;
        private string dir;

        public enum FileType
        {
            png,
            jpg,
            other
        }
        public FileController(string dir)
        {
            this.dir = dir;
            Directory.SetCurrentDirectory(dir);
            files = Directory.GetFiles(".");
            foreach (string file in files)
            {
                if (file.EndsWith(".png") || file.EndsWith(".PNG"))
                {
                    pngCount++;
                }
                else if (file.EndsWith(".jpg") || file.EndsWith(".JPG"))
                {
                    jpgCount++;
                }
                else
                {
                    otherCount++;
                }
                totalCount++;
            }
        }

        public IEnumerable<string> GetFolders()
        {
            var folders = Directory.GetDirectories(".");
            foreach (var folder in folders)
            {
                yield return folder;
            }

        }

        public string GetFolderByNum(int num)
        {
            var folders = GetFolders();
            var f = folders.ElementAt(num-1);
            return f;
        }

        public IEnumerable<string> GetFoldersWithNum()
        {
            var folders =  GetFolders();
            int i = 0;
            foreach (var folder in folders)
            {
                i++;
                yield return $"{i}| {folder}";
            }

        }
        
          
        public IEnumerable<string> GetPhotoFiles()
        {
         return GetFilesFromType(new List<string> {"jpg", "png"});
        }

        public IEnumerable<string> GetFilesFromType(string type)
        {
            foreach (string file in files)
            {
                if (file.EndsWith(type, StringComparison.OrdinalIgnoreCase))
                {
                    yield return file;
                }
            }
     
        }

        public IEnumerable<string> GetFilesFromType(IEnumerable<string> types)
        {  
            var output = new List<string>();
            foreach (string type in types)
            {
                output.AddRange(GetFilesFromType(type));
            }
            return output;
        }
        


        public List<string> GetAllFiles()
        {       
            List<string> allFiles = new List<string>();
            foreach (string file in files)
            {
                allFiles.Add(file);
            }
            return allFiles;
        }


    
        public IEnumerable<string> GetPngs()=> GetFilesFromType("png");
      

        // return GetFilesFromType("png").ToList();

        public IEnumerable<string> GetJpgs() => GetFilesFromType("jpg");
        //  return GetFilesFromType("jpg").ToList();
        public string GetDirectoryPath()=> dir;
        public int GetFileCount()=>totalCount;

        public int GetJpgCount()=> jpgCount;
        public int GetPngCount()=> pngCount;
      

        public void PrintSummary()
        {
            WriteLine($"In {dir} there are {pngCount} png files, {jpgCount} jpg files and {otherCount} other files");
        }   
    }

   

}
