using DDControllers;

var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.local.json");
var configuration = builder.Build();
var connectionString = configuration.GetConnectionString("StorageAccount");               


var ui = new UIController();
var fc = new FileController(ui.GetDirectory());
var blobService = new DDBlobService(connectionString);
var tableService = new DDTableService(connectionString, "conferencepictures");

int MenuChoice = ui.AskMainMenu();

while (MenuChoice <3)
{
    switch (MenuChoice)
    {
        case 1:
        {
            var folders = fc.GetFoldersWithNum();
            foreach (var folder in folders)
            {
                ui.PrintString(folder);
            }
            break;
        }
        case 2:
        {
            fc = new FileController(fc.GetFolderByNum(ui.AskFolderNumber()));
            MenuChoice = 3;
            continue;

        }
    }
    MenuChoice = ui.AskMainMenu();
}



PhotoRepository repo = new PhotoRepository();
repo.LoadPhotos(fc.GetPhotoFiles().ToList());
ui.PrintSummary(fc.GetDirectoryPath(), fc.GetPngCount(), fc.GetJpgCount());



int Option = ui.AskOptions();
while (Option <4)
{

    switch(Option)
    {
    case 1:
        { 
            foreach( var p in repo.GetAllPhotoNames())
            {
                ui.PrintString(p);
            }
            break;
        }
    case 2:
        {
            string containerName = ui.AskForContainerName();
            blobService.CreateContainer(containerName);
            string year = ui.AskForYear();
            string startTrim = ui.AskForStartTrim();
            string endTrim = ui.AskForEndTrim();

            foreach(string p in repo.GetAllPhotoNames())
            {
                blobService.UploadBlob(p);
                ui.PrintString($"Uploaded {p} to {containerName}");
                var E = repo.ToEntity(year, repo.GetPhoto(p), containerName, startTrim, endTrim);
         
                tableService.InsertEntity(E);
            }
            break;
        }
    case 3:
        {
            ui.PrintString("This are the JPG files:");
            foreach (var p in fc.GetJpgs())
            {
                ui.PrintString(p);
            }
            ui.PrintString("This are the PNG files:");
            foreach (var p in fc.GetPngs())
            {
                ui.PrintString(p);
            }
            break;
        }
    }
    Option = ui.AskOptions();
} 
