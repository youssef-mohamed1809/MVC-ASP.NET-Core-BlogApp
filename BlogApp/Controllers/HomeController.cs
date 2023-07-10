using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        //List<Blog> blogs;
        IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        string connectionString;
        SqlConnection con;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
            con = new SqlConnection(connectionString);
            con.Open();
        }

        public List<Blog> getAllBlogs()
        {
            List<Blog> myBlogs = new List<Blog>();
            string stmt = "select * from blogs";
            SqlCommand cmd = new SqlCommand(stmt, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {           
                    DateTime dt = (DateTime)reader["dateCreated"];
                    DateOnly d = DateOnly.FromDateTime(dt);
                    myBlogs.Add(new Blog((string)reader["title"], (string)reader["content"],
                        (string)reader["author"], (int)reader["views"], d));
            }

            return myBlogs;
           
        }


        public IActionResult Index()
        {
            List<Blog> myBlogs = getAllBlogs();
            return View("Index", myBlogs);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult FullBlog(string title)
        {
            List<Blog> blogs = getAllBlogs();

            for(int i = 0; i < blogs.Count; i++)
            {
                if (blogs[i].title == title)
                {
                    ViewData["Title"] = blogs[i].title;
                    ViewData["Content"] = blogs[i].content;
                    ViewData["Author"] = blogs[i].author;
                    ViewData["Date"] = blogs[i].dateCreated;
                    ViewData["Views"] = blogs[i].views + 1;
                }
            }

            string stmt = $"update blogs set views={(int)ViewData["Views"]} where title = \'{ViewData["Title"]}\'";
            SqlCommand cmd1 = new SqlCommand(stmt, con);
            cmd1.ExecuteNonQuery();


            return View();
        }

        public IActionResult AddBlog()
        {

            return View();
        }

        [HttpPost]
        public IActionResult AddBlogPost(Blog blog)
        {
            string title = blog.title;
            string content = blog.content;
            string author = blog.author;
            DateTime dt = DateTime.Now;



            string stmt = $"insert into blogs(title, content, author, views, dateCreated) values( @title , @content , @author, @views ,@dateCreated)";
            SqlCommand cmd = new SqlCommand(stmt, con);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@author", author);
            cmd.Parameters.AddWithValue("@views", 0);
            cmd.Parameters.AddWithValue("@dateCreated", dt);
            cmd.Parameters.AddWithValue("@content", content);
            cmd.ExecuteNonQuery();
             return Index();
            //Response.Redirect("/index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}