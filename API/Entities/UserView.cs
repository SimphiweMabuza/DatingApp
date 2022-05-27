namespace API.Entities
{
    public class UserView
    {
        public AppUser SourceUser { get; set; }
        public int SourceUserId { get; set; }

        public AppUser ViewedUser { get; set; }
        public int ViewedUserId { get; set; }
    }
}