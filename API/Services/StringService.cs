﻿using System.Collections.Generic;

namespace API.Services
{
    public static class StringService
    {
        // TODO: verify data incoming from the client-side
        public static IEnumerable<string> AvailableGenders => new List<string>
        {
            "male",
            "female",
            "other"
        };

        public static class LikeRelationship
        {
            public const string Likes = "likes";

            public const string LikedBy = "likedBy";
        }
    }
}
