namespace DDControllers
{
    public class UIController
    {
        private FileController? fc;
       // private BlobController? bc;

        private string dir ="";
        public UIController(){
            WriteLine("Welcome to DD Blob Uploader! ");
            WriteLine("-----------------------------------------------------");
            dir = AskForDirectory();
        }

        private string AskForDirectory()
        {
            Console.WriteLine("Please enter the directory you want to upload from:");
            string? dir = Console.ReadLine();
            if (dir == null)
            {
                throw new ArgumentNullException("dir");
            }
            else
            {
                return dir;
            }
        }

        public string GetDirectory()
        {
           return dir; 
        }
        private string AskForFileType()
        {
            Console.WriteLine("Please enter the file type:");
            string? type = Console.ReadLine();
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            else
            {
                return type;
            }
            
        }
        public string AskForContainerName()
        {
            Console.WriteLine("Please enter the container name:");
            string? name = Console.ReadLine();
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else
            {
                return name;
            }   
        }

        public void AskIfUserWantsToSeeEachFile()
        {
            WriteLine("Do you want to see each file? (y/n)");
            string? answer = Console.ReadLine();
            if (answer == null)
            {
                throw new ArgumentNullException("answer");
            }
            else if (answer == "y")
            {
                PrintAllFiles();
            }
            else
            {
                WriteLine("Okay, no files will be printed.");
            }
        }

        public string AskForYear()
        {
            Console.WriteLine("Please enter the year:");
            string? year = Console.ReadLine();
            if (year == null)
            {
                throw new ArgumentNullException("year");
            }
            else
            {
                return year;
            }
        }

        public string AskForStartTrim()
        {
            Console.WriteLine("Please enter the start trim:");
            string? startTrim = Console.ReadLine();
            if (startTrim == null)
            {
                throw new ArgumentNullException("startTrim");
            }
            else
            {
                return startTrim;
            }
        }

        public string AskForEndTrim()
        {
            Console.WriteLine("Please enter the end trim:");
            string? endTrim = Console.ReadLine();
            if (endTrim == null)
            {
                throw new ArgumentNullException("endTrim");
            }
            else
            {
                return endTrim;
            }
        }
        public int AskOptions()
        {
            WriteLine("Please enter the number of the option you want to select:");
            WriteLine("1. List all IMAGE files");
            WriteLine("2. Upload IMAGE files to blob storage");
            WriteLine("3. Split JPEG files into PNG files");
            WriteLine("4. Exit");
            string? answer = Console.ReadLine();
            if (answer == null)
            {
                throw new ArgumentNullException("answer");
            }
            else
            {
                return int.Parse(answer);
            }
        }
        private List<string> GetAllFiles()
        {
            List<string> files = fc.GetAllFiles();
            return files;
        }
        
        private List<string> GetAllFilesFromType(string type)
        {
            type = "." + type;
            List<string> files = fc.GetFilesFromType(type).ToList();
            return files;
        }

        public void PrintString(string str)
        {
            Console.WriteLine(str);
        }

        public void PrintAllFiles()
        {
           
            foreach (string file in GetAllFiles())
            {
                 WriteLine(file);
            }
        
        }

        public void PrintSummary(string path, int pngCount, int jpgCount)
        {
            WriteLine($"In {path} there are {pngCount} png files and {jpgCount} jpg files.");
        }

        public void PrintAllFilesInList(List<string> files)
        {
            foreach (string file in files)
            {
                WriteLine(file);
            }
        }

        public void PrintAllFilesFromType()
        {
           
            foreach (string file in GetAllFilesFromType(AskForFileType()))
            {
                WriteLine(file);
            }
        }
        

    }
}