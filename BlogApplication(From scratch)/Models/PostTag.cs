namespace BlogApplication_From_scratch_.Models
{
    public class PostTag
    {
        public PostTag()
        {
            Posts = new List<BlogPost>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public ICollection<BlogPost> Posts { get; set; }
    }
}
