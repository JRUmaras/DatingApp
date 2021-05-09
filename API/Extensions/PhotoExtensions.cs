using System.Collections.Generic;
using API.Entities;

namespace API.Extensions
{
    // TODO: Create PhotoCollections class instead, but check how it interacts with EF and DB
    public static class PhotoExtensions
    {
        /// <summary>
        /// Update the main photo of a set of Photos.
        /// Assumptions:
        /// - Each Photo.Id is unique in the set.
        /// - There is at most one main photo in the set.
        /// </summary>
        /// <param name="userPhotos">Set of photos.</param>
        /// <param name="newMainPhotoId">Photo.Id of the new main photo.</param>
        /// <returns></returns>
        public static bool SetNewMainPhoto(this IEnumerable<Photo> userPhotos, int newMainPhotoId)
        {
            Photo oldMainPhoto = null;
            Photo newMainPhoto = null;

            foreach (var photo in userPhotos)
            {
                if (photo.IsMain)
                {
                    oldMainPhoto = photo;
                    oldMainPhoto.IsMain = false;

                    if (newMainPhoto is not null) return true;
                }

                // ReSharper disable once InvertIf
                if (photo.Id == newMainPhotoId)
                {
                    newMainPhoto = photo;
                    newMainPhoto.IsMain = true;

                    if (oldMainPhoto is not null) return true;
                }
            }

            // In case there was no main photo in the set to begin with
            if (newMainPhoto is not null) return true;

            // If we got down to here, the update failed
            // Reset old photo as main if possible
            if (oldMainPhoto is not null) oldMainPhoto.IsMain = true;

            return false;
        }
    }
}
