using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        //List<Blog> blogs;
        
        private readonly ILogger<HomeController> _logger;
        private readonly BlogDbContext _dbContext;
        

        public HomeController(ILogger<HomeController> logger, BlogDbContext blogDbContext)
        {
            _logger = logger;
            _dbContext = blogDbContext;
        }

        public List<MyBlog> getAllBlogs()
        {
            List<MyBlog> myBlogs = _dbContext.MyBlogs.ToList();
/*
            string stmt = "select * from myBlogs";
            SqlCommand cmd = new SqlCommand(stmt, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {           
                    DateTime dt = (DateTime)reader["dateCreated"];
                    DateOnly d = DateOnly.FromDateTime(dt);
                    myBlogs.Add(new Blog((string)reader["title"], (string)reader["content"],
                        (string)reader["author"], (int)reader["views"], d));
            }
*/
            return myBlogs;
           
        }


        public IActionResult Index()
        {
            List<MyBlog> myBlogs = getAllBlogs();
            return View("Index", myBlogs);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult FullBlog(string title)
        {
            List<MyBlog> blogs = getAllBlogs();

            MyBlog blog = new MyBlog();
            for(int i = 0; i < blogs.Count; i++)
            {
                if (blogs[i].Title == title)
                {
                    
                    ViewData["Title"] = blogs[i].Title;
                    ViewData["Content"] = blogs[i].Content;
                    ViewData["Author"] = blogs[i].Author;
                    ViewData["Date"] = blogs[i].DateCreated;
                    ViewData["Views"] = blogs[i].Views + 1;

                    blog = _dbContext.MyBlogs.Find(blogs[i].Title);
                }
            }

            blog.Views += 1;
            _dbContext.SaveChanges();


            return View(blog);
        }

        public IActionResult AddBlog()
        {

            return View();
        }

        [HttpPost]
        public IActionResult AddBlogPost(MyBlog blog)
        {
            string title = blog.Title;
            string content = blog.Content;
            string author = blog.Author;
            DateTime dt = DateTime.Now;


            MyBlog newBlog = new MyBlog();

            newBlog.Title = title;
            newBlog.Content = content;
            newBlog.Author = author;
            newBlog.DateCreated = dt;
            newBlog.Views = 0;

            _dbContext.MyBlogs.Add(newBlog);
            _dbContext.SaveChanges();

             return Index();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}