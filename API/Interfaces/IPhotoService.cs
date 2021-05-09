using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using API.Errors.Data.Repositories;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        // TODO: Make this interface more general, uncouple it from cloudinary, it should be "IPhotoStorageService", whatever the storage is
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        /// <summary>
        /// Deletes specified photo from the storage.
        /// </summary>
        /// <param name="storageId">Public ID of the photo.</param>
        /// <returns>Returns true if the deletion succeeds.</returns>
        /// <exception cref="PhotoDeletionFailedException">Throws if deletion from the storage failed.</exception>
        Task DeletePhotoAsync(string storageId);
    }
}
