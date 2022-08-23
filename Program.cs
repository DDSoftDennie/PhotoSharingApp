using DDControllers;

var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.local.json");
var configuration = builder.Build();
var connectionString = configuration.GetConnectionString("StorageAccount");

var Photos = new PhotoData();
var ui = new UIController();
var fc = new FileController(ui.GetDirectory());
var blobService = new DDBlobService(connectionString);


Photos.AddPhotos(fc.GetPhotoFiles());
ui.PrintSummary(fc.GetDirectoryPath(), fc.GetPngCount(), fc.GetJpgCount());


int Action = ui.AskOptions();
while (Action <3)
{

    switch(Action)
    {
    case 1:
        { 
            foreach( var p in Photos.AllPhotoNames)
            {
                ui.PrintString(p);
            }
            break;
        }
    case 2:
        {
            string containerName = ui.AskForContainerName();
            blobService.CreateContainer(containerName);

            foreach(string p in Photos.AllPhotoNames)
            {
                blobService.UploadBlob(p);
                ui.PrintString($"Uploaded {p} to {containerName}");
            }
            break;
        }
    }
    Action = ui.AskOptions();
} 
