namespace API.Helpers
{
    public class ViewsParams : PaginationParams
    {
        public int UserId { get; set; }
        public string Predicate { get; set; }
        public string OrderBy { get; set; } = "LastVisit";
    }
}