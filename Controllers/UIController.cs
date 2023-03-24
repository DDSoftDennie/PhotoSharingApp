namespace DDControllers
{

    public class UIController
    {
 
  
        private string dir ="";
        private ImageFileService _imageFileService;
        
        public UIController(){
            _imageFileService = new ImageFileService();
        }

        public void PrintWelcome()
        {
            WriteLine("Welcome to DD Blob Uploader! ");
            WriteLine("-----------------------------------------------------");
        }

        public string AskForDirectory()
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

        public int AskFolderNumber()
        {
            Console.WriteLine("Please enter the folder number:");
            string? num = Console.ReadLine();
            if (num == null)
            {
                throw new ArgumentNullException("num");
            }
            else
            {
                return int.Parse(num);
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

        public string AskForStartTrim(string startTrim)
        {
            WriteLine($"Start trim: {startTrim}");
            WriteLine("Pres ENTER to continue or write another start trim");
            var answer = Console.ReadLine();
            if(answer != null && answer != "")
            {
                return answer;
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

        public string AskForEndTrim(string endTrim)
        {
            WriteLine($"End trim: {endTrim}");
            WriteLine("Pres ENTER to continue or write another end trim");
            var answer = Console.ReadLine();
            if (answer != null && answer != "")
            {
                return answer;
            }
            else
            {
                return endTrim;
            }
        }


        public FolderMenu AskFolderMenuChoice()
        {
            WriteLine("Please enter the number of the option you want to choose:");
            WriteLine("1. List all folders");
            WriteLine("2. Navigate to folder");
            WriteLine("3. Change PhotoSharingApp ROOT folder");
            WriteLine("4. Exit");
            FolderMenu MenuChoice = (FolderMenu)int.Parse(ReadLine());
            return MenuChoice;
        }
 
        public FileMenu AskFileMenuChoice()
        {
            WriteLine("Please enter the number of the option you want to select:");
            WriteLine("1. List all IMAGE files");
            WriteLine("2. Upload IMAGE files to blob storage");
            WriteLine("3. Split JPEG files into PNG files");
            WriteLine("4. Back");
            FileMenu MenuChoice = (FileMenu)int.Parse(ReadLine());
            return MenuChoice;
        }
        private int GetValidOption(string answer, int max)
        {
             if (answer == null)
            {
                throw new ArgumentNullException("answer");
            }
            else
            {
                int option = int.Parse(answer);
                if (option > max || option < 1)
                {
                    throw new ArgumentOutOfRangeException("option");
                }
                else
                {
                    return option;
                }
            }
        }
         private List<string> GetAllFiles()
         {
             List<string> files = _imageFileService.GetAllFiles();
            return files;
        }
        
        private List<string> GetAllFilesFromType(string type)
        {
             type = "." + type;
             List<string> files = _imageFileService.GetFilesFromExtension(type).ToList();
             return files;
         }

        public void PrintString(string str) => WriteLine(str);


        public void PrintAllFiles()
        {
           
            foreach (string file in GetAllFiles())
            {
                 WriteLine(file);
            }
        
        }

        public void PrintDirectory(string dir) => WriteLine($"Photo directory: {dir}");
            
        
        public void PrintSummary(string path, int pngCount, int jpgCount)=>
            WriteLine($"In {path} there are {pngCount} png files and {jpgCount} jpg files.");


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