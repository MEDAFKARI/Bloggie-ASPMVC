namespace BlogApplication_From_scratch_.Models
{
    public class BlogPost
    {
        public BlogPost() {
            Tags = new List<PostTag>();
        }
        public Guid Id { get; set; }
        public string Heading {get;set;}
        public string Title { get;set;}
        public string Content { get;set;}
        public string ShortDescription { get;set;}
        public string FeaturedImgUrl { get;set;}
        public string UrlHandle { get;set;}
        public DateTime PublishedDate { get;set;}
        public string Author { get; set; }  
        public bool visible { get;set;}

        public ICollection<PostTag> Tags { get; set; }

    }
}
