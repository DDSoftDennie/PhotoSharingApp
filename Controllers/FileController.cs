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

        public List<string> GetPhotoFiles()
        {
            List<string> photoFiles = new List<string>();
            foreach (string file in files)
            {
                if (file.EndsWith(".png") || file.EndsWith(".PNG") || file.EndsWith(".jpg") || file.EndsWith(".JPG"))
                {
                    photoFiles.Add(file);
                }
            }
            return photoFiles;
        }

        public List<string> GetFilesFromType(string type)
        {
            string typeExtNoCaps = "." + type.ToLower();
            List<string> filesOfType = new List<string>();
            foreach (string file in files)
            {
                if (file.ToLower().EndsWith(typeExtNoCaps))
                {
                    filesOfType.Add(file);
                }
            }
            return filesOfType;
        }

        public List<string> GetPngs()
        {
            List<string> pngs = GetFilesFromType("png");
            return pngs;
        }

        public List<string> GetJpgs()
        {
            List<string> jpgs = GetFilesFromType("jpg");
            return jpgs;
        }
  
        public string GetDirectoryPath()
        {
            return dir;
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

        public int GetFileCount()
        {
            return totalCount;
        }

        public int GetJpgCount()
        {
            return jpgCount;
        }

        public int GetPngCount()
        {
            return pngCount;
        }

        public void PrintSummary()
        {
            WriteLine($"In {dir} there are {pngCount} png files, {jpgCount} jpg files and {otherCount} other files");
        }   
    }

   

}
