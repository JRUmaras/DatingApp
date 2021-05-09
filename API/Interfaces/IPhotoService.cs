using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        // TODO: Make this interface more general, uncouple it from cloudinary, it should be "IPhotoStorageService", whatever the storage is
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        Task<bool> DeletePhotoAsync(string storageId);
    }
}
