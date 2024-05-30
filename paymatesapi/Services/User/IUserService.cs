using paymatesapi.Models;

namespace paymatesapi.Services
{
  public interface IUserService
  {
    public Task<BaseResponse<string>> UploadImage(IFormFile file, string uid);
  }
}
