using paymatesapi.Models;
using Azure.Identity;
using Azure.Storage.Blobs;
using paymatesapi.Contexts;

namespace paymatesapi.Services
{
    public class UserService(DataContext dataContext, IConfiguration configuration) : IUserService
    {

        private readonly DataContext _dataContext = dataContext;

        public async Task<BaseResponse<string>> UploadImage(IFormFile file, string uid)
        {

            try
            {
                if (file == null || file.Length == 0)
                {
                    return new BaseResponse<string> { Error = new Error { Message = "File not found" } };
                }

                string containerName = "photos";
                var blobServiceClient = new BlobServiceClient(
                    new Uri("https://paymates.blob.core.windows.net"), //TODO: replace this with an env
                    new DefaultAzureCredential()
                );
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

                var blobName = uid + Path.GetExtension(file.FileName);

                var blobClient = blobContainerClient.GetBlobClient(blobName);

                using (var stream = file.OpenReadStream())
                {
                    var user = _dataContext.Users.Find(uid);
                    if (user != null)
                    {
                        await blobClient.UploadAsync(stream, true);
                        user.PhotoUrl = blobName;
                        await _dataContext.SaveChangesAsync();
                    }
                    else
                    {
                        return new BaseResponse<string> { Error = new Error { Message = "User not found" } };
                    }
                }

                return new BaseResponse<string> { Data = blobName };
            }
            catch
            {
                return new BaseResponse<string> { Error = new Error { Message = "Error has occurred" } };
            }
        }


        public async Task<Stream> GetImageAsync(string blobName)
        {

            string azureContainer = configuration.GetSection("Urls:AzureContainer").Value!;
            string azureContainerName = configuration.GetSection("Urls:AzureContainerName").Value!;
            System.Console.WriteLine("logging azure containter: ", azureContainer);
            System.Console.WriteLine("logging azure containter name: ", azureContainerName);
            var _blobServiceClient = new BlobServiceClient(
            new Uri(azureContainer),
            new DefaultAzureCredential()
        );
            var containerClient = _blobServiceClient.GetBlobContainerClient(azureContainerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            var downloadInfo = await blobClient.DownloadAsync();

            return downloadInfo.Value.Content;
        }


    }
}
