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

        public List<string> GetPngs()
        {
            List<string> pngs = new List<string>();
            int i = 0;
            foreach (string file in files)
            {
                if (file.EndsWith(".png") || file.EndsWith(".PNG"))
                {
                    pngs.Add(file);
                    i++;
                }
            }
            return pngs;
        }

        public List<string> GetJpgs()
        {
            List<string> jpgs = new List<string>();
            int i = 0;
            foreach (string file in files)
            {
                if (file.EndsWith(".jpg") || file.EndsWith(".JPG"))
                {
                    jpgs.Add(file);
                    i++;
                }
            }
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

        public List<string> GetFilesFromType(string type)
        {
            List<string> filteredFiles = new List<string>(); 
            foreach (string file in files)
            {
                if (file.EndsWith(type))
                {
                    filteredFiles.Add(file);
                }
            }
            return filteredFiles;
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
