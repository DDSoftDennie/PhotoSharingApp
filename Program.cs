﻿using Controllers;
using DDControllers;
using PhotoSharingApp.Factories;
using PhotoSharingApp.Model;
using Repositories;

var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.local.json");
var configuration = builder.Build();
var connectionString = configuration.GetConnectionString("StorageAccount");               
var ui = new UIController();
var repo = new PhotoRepository();
var imageFileController = new ImageFileController();
var folderController = new FolderController();
var blobService = new DDBlobService(connectionString);
var tableService = new DDTableService(connectionString, "conferencepictures");
var formatFactory = new FormatFactory("C:\\Users\\dpcfr\\Documents\\Afbeeldingen\\format.csv");
string rootDirectory = "C:\\Users\\dpcfr\\Documents\\Afbeeldingen\\";
imageFileController.SetDirectory(rootDirectory);
Format format = formatFactory.getFormat();
bool folderTasksAreHandled = false;

ui.PrintWelcome();
ui.PrintDirectory(rootDirectory);

while (!folderTasksAreHandled)
{
    folderTasksAreHandled = FolderTasksAreHandled();
} 

bool FolderTasksAreHandled()
{
    FolderMenu FolderMenuChoice = ui.AskFolderMenuChoice();
    switch (FolderMenuChoice)
    {
        case FolderMenu.ListAllFolders:
        {
            PrintAllFolders();
            return false;
        }
        case FolderMenu.NavigateToFolder:
        {
            string newDirectory = folderController.GetFolderByNum(ui.AskFolderNumber());
            imageFileController.SetDirectory(newDirectory);
            HandleFiles();
            return false;
        }
        case FolderMenu.ChangeFolder:
        {
            var dir = ui.AskForDirectory();
            imageFileController.SetDirectory(dir);
            HandleFiles();
            return false;
        }
        case FolderMenu.Back:
        {
            imageFileController.SetDirectory(rootDirectory);
            return true;
        }
    }
    return false;
}

void HandleFiles()
{
    bool filesAreHandled = false;
    while (!filesAreHandled)
    {
        filesAreHandled = FileTasksAreHandled();
    }
}

bool FileTasksAreHandled()
{
    FileMenu FileMenuChoice = ui.AskFileMenuChoice();
    InitializeRepo();
    switch (FileMenuChoice)
    {
        case FileMenu.ListAllImages:
        {
            ListAllImages();
            return false;
        }
        case FileMenu.UploadImages:
        {
            AddImagesToStorageTable(UploadImagesAndGetStorageInfo());
            return false;
        }
        case FileMenu.SplitImagesOnFileType:
        {
            SplitImagesOnFileType();
            return false;
        }
        case FileMenu.Back:
        {
            EmptyRepo();
            imageFileController.SetDirectory(rootDirectory);
            return true;
        }
    }
    return false;
}

void InitializeRepo()
{
    repo = new PhotoRepository(imageFileController.GetPhotoFiles().ToList());
}

void EmptyRepo()
{
    repo.Empty();
}

void PrintAllFolders()
{
    var folders = folderController.GetFoldersWithNum();
    foreach (var folder in folders)
    {
        WriteLine(folder);
    }
}

void ListAllImages()
{
    foreach(var p in repo.GetAllPhotoNames())
    {
        WriteLine(p);
    }
}

(string?, string?, string?) UploadImagesAndGetStorageInfo()
{
    if(repo != null && blobService != null)
    {
        string containerName = ui.AskForContainerName();
        blobService.CreateContainer(containerName);
        string startTrim = ui.AskForStartTrim(format.StartTrim.ToString());
        string endTrim = ui.AskForEndTrim(format.EndTrim.ToString());

        foreach (string p in repo.GetAllPhotoNames())
        {
            blobService.UploadBlob(p);
            WriteLine($"Uploaded {p} to {containerName}");
        }
        return (containerName, startTrim, endTrim);
    }
    else
    {
        WriteLine("Error with uploading!");
    }
    return (null, null, null);

}

void AddImagesToStorageTable((string?, string?,string?) storageInfo)
{
    if(repo != null && tableService != null)
    {
        if(storageInfo.Item1 != null && storageInfo.Item2 != null && storageInfo.Item3 != null)
        {
            foreach (string p in repo.GetAllPhotoNames())
            {
				if(repo.GetPhoto(p)!= null)
                {
					tableService.InsertEntity(PhotoRepository.ToEntity(repo.GetPhoto(p), storageInfo.Item1, storageInfo.Item2, storageInfo.Item3));
				}
              //  var E = PhotoRepository.ToEntity(repo.GetPhoto(p), storageInfo.Item1, storageInfo.Item2, storageInfo.Item3);
                
            }
            WriteLine("Added images to storage table");
        }
        else
        {
            WriteLine("Error adding images to storage table");
        }
    }
}

void SplitImagesOnFileType()
{
    WriteLine("This are the JPG files:");
    foreach (var p in imageFileController.GetJpgs())
    {
        WriteLine(p);
    }

    WriteLine("This are the PNG files:");
    foreach (var p in imageFileController.GetPngs())
    {
        WriteLine(p);
    }
}