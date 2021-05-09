using System;

namespace API.Errors.Data.Repositories
{
    public class PhotoDeletionFailedException : Exception
    {
        public PhotoDeletionFailedException(string message) : base(message)
        { }

        public PhotoDeletionFailedException(string message, Exception inner)
            : base(message, inner)
        { }

        public static PhotoDeletionFailedException DeletionInStorageFailedException(string innerMessage = "")
        {
            return new("Failed to delete the photo from the cloud.", new Exception(innerMessage));
        }

        public static PhotoDeletionFailedException UnknownIssueException()
        {
            return new("Unknown issue caused the photo deletion to fail.");
        }

        public static PhotoDeletionFailedException PhotoNotFoundException()
        {
            return new("Could not find the photo.");
        }
    }
}
