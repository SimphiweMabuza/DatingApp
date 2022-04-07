using System.Threading.Tasks;
using CoudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IPhotoServices 
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<IDeletionResult> DeletePhotoAsync(string publicId);
    }
}