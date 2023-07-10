namespace BlogApp.Models
{
    public class Blog
    {
        public string title { get; set; } 
        public string content { get; set; }
        public string author { get; set; }
        public int views { get; set; }
        public DateOnly dateCreated { get; set; }

        public Blog(string title, string content, string author, int views, DateOnly dateCreated)
        {
            this.title = title;
            this.content = content;
            this.author = author;
            this.views = views;
            this.dateCreated = dateCreated;
        }
        public Blog() { }

    }
}
