using System;
using System.Collections.Generic;

namespace BlogApp.Models;

public partial class MyBlog
{
    public string Title { get; set; } = null!;

    public string Content { get; set; }

    public string Author { get; set; }

    public int Views { get; set; }

    public DateTime DateCreated { get; set; }
}
