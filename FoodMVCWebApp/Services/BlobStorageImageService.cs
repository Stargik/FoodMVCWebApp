using System;
using Azure;
using Azure.Storage.Blobs;
using FoodMVCWebApp.Interfaces;
using Microsoft.Extensions.Options;
using Azure.Storage.Blobs.Models;
using System.Reflection.Metadata;

namespace FoodMVCWebApp.Services
{
	public class BlobStorageImageService : IImageService
    {
        private readonly BlobStaticFilesSettings imgSettings;

        public BlobStorageImageService(IOptions<BlobStaticFilesSettings> imgSettings)
        {
            this.imgSettings = imgSettings.Value;
        }

        public async Task<IFormFile> Download(string imgName)
        {
            BlobContainerClient container = new BlobContainerClient(imgSettings.ConnectionString, imgSettings.ContainerImagesName);

            BlobClient file = container.GetBlobClient(imgName);

            if (await file.ExistsAsync())
            {
                await using (Stream? stream = await file.OpenReadAsync())
                {
                    return new FormFile(stream, 0, stream.Length, imgName, imgName);
                }
            }
            return null;
        }

        public async Task<string> GetStoragePath()
        {
            BlobContainerClient container = new BlobContainerClient(imgSettings.ConnectionString, imgSettings.ContainerImagesName);
            return container.Uri.ToString();
        }

        public async Task Upload(IFormFile imgFile)
        {
            BlobContainerClient container = new BlobContainerClient(imgSettings.ConnectionString, imgSettings.ContainerImagesName);

            try
            {
                BlobClient client = container.GetBlobClient(imgFile.FileName);

                await using (Stream? stream = imgFile.OpenReadStream())
                {
                    await client.UploadAsync(stream);
                }
            }
            catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                throw new Exception("File is already exist.");
            }
        }
    }
}

