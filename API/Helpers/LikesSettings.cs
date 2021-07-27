namespace API.Helpers
{
    public class LikesSettings : PaginationSettings
    {
        public int UserId { get; set; }

        public string Predicate { get; set; }
    }
}