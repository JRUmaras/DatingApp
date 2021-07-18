namespace API.Entities
{
    public class UserLike
    {
        public AppUser Liker { get; set; }

        public int LikerId { get; set; }

        public AppUser Likee { get; set; }

        public int LikeeId { get; set; }
    }
}
