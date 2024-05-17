using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using paymatesapi.Entities;
using paymatesapi.Models;
using paymatesapi.Services;

namespace paymatesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserAuthService userAuthService) : ControllerBase
    {
        private readonly IUserAuthService _userAuthService = userAuthService;

        [HttpPost("get-user"), Authorize]
        public ActionResult<BaseResponse<User>> GetUser(UserRequest userRequest)
        {
            BaseResponse<User> response = _userAuthService.GetUser(userRequest.Uid);
            return response.Error != null ? BadRequest(response) : Ok(response);
        }

        [HttpPost("update-user"), Authorize]
        public async Task<ActionResult<BaseResponse<bool>>> UpdateUser(
            UserUpdateRequest userRequest
        )
        {
            BaseResponse<bool> response = await _userAuthService.UpdateUser(userRequest);
            return response.Error != null ? BadRequest(response) : Ok(response);
        }

        [HttpPost("upload-photo")]
        public async Task<IActionResult> UploadBlob(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("File is empty");
                string containerName = "photos";
                // Get a reference to the container
                var blobServiceClient = new BlobServiceClient(
                    new Uri("https://paymates.blob.core.windows.net"),
                    new DefaultAzureCredential()
                );
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Generate a unique blob name or use a predefined name
                var blobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                // Get a reference to the blob
                var blobClient = blobContainerClient.GetBlobClient(blobName);

                // Upload the file to the blob storage
                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                // Return the blob URI
                var blobUri = blobClient.Uri.ToString();
                return Ok(blobUri);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
