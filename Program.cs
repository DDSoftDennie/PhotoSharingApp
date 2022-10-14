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
PhotoRepository repo = new PhotoRepository();
repo.LoadPhotos(fc.GetPhotoFiles().ToList());
ui.PrintSummary(fc.GetDirectoryPath(), fc.GetPngCount(), fc.GetJpgCount());


int Action = ui.AskOptions();
while (Action <4)
{

    switch(Action)
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
    Action = ui.AskOptions();
} 
