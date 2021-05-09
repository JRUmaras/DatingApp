using System.Threading.Tasks;
using API.Errors.Data.Repositories;
using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        private readonly Transformation _transformation;

        public PhotoService(IOptions<CloudinarySettings> settings)
        {
            var account = new Account(settings.Value.CloudName, settings.Value.ApiKey, settings.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);

            _transformation = new Transformation(settings.Value.Transformation);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            if (file.Length <= 0) return new ImageUploadResult();

            await using var fileStream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, fileStream),
                Transformation = _transformation
            };

            return await _cloudinary.UploadAsync(uploadParams);
        }

        public async Task DeletePhotoAsync(string storageId)
        {
            var deleteParams = new DeletionParams(storageId);

            var deletionResult = await _cloudinary.DestroyAsync(deleteParams);

            if (deletionResult.Error is not null) throw PhotoDeletionFailedException.DeletionInStorageFailedException(deletionResult.Error.Message);
        }
    }
}
