using System;

namespace API.Errors.Data.Repositories
{
    public class PhotoUploadFailedException : Exception
    {
        public static string UploadToCloudFailedMsg = "Failed to upload the photo to the cloud.";
        public static string UnknownIssueMsg = "Unknown issue caused the photo upload to fail.";
        public PhotoUploadFailedException(string message) : base(message)
        { }

        public PhotoUploadFailedException(string message, Exception inner)
            : base(message, inner)
        { }

        public static PhotoUploadFailedException CloudUploadFailedException(CloudinaryDotNet.Actions.Error cloudinaryError)
        {
            return new(UploadToCloudFailedMsg, new Exception(cloudinaryError.Message));
        }

        public static PhotoUploadFailedException UnknownIssueException()
        {
            return new(UnknownIssueMsg);
        }
    }
}
