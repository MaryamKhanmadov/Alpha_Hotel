using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Models;
using Alpha_Hotel_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha_Hotel_Project.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public BlogController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        [HttpGet]
        public IActionResult BlogDetail(Guid id)
        {
            Blog blog = _appDbContext.Blogs.Include(x => x.BlogComments).FirstOrDefault(x => x.Id == id);
            if (blog == null) return View("Error");
            BlogComment blogComment = null;
            blog.ViewCount++;
            BlogViewModel blogVM = new BlogViewModel
            {
                Blog = blog,
                BlogComment = blogComment
            };
            _appDbContext.SaveChanges();
            return View(blogVM);
        }

        [HttpPost]
        public IActionResult BlogDetail(Guid id, BlogViewModel blogVM)
        {
            Blog blog = _appDbContext.Blogs.Include(x => x.BlogComments).FirstOrDefault(x => x.Id == id);
            if (blog == null) return View("Error");
            blogVM.BlogComment.BlogId = blog.Id;
            blogVM.Blog = blog;
            BlogViewModel blogViewModel = new BlogViewModel
            {
                //BlogPost = blogPosts,
                BlogComment = blogVM.BlogComment
            };

            BlogComment comment = blogVM.BlogComment;
            //if (!ModelState.IsValid) { return View(blogPostVM); }
            if (blogVM.BlogComment.CommentEmail is null)
            {
                ModelState.AddModelError("UserCommetMail", "Required");
                return View(blogVM);
            }
            if (blogVM.BlogComment.Comment is null)
            {
                ModelState.AddModelError("UserCommentMessage", "Required");
                return View(blogVM);
            }
            if (blogVM.BlogComment.FullName is null)
            {
                ModelState.AddModelError("UserFullName", "Required");
                return View(blogVM);
            }

            _appDbContext.BlogComments.Add(comment);
            _appDbContext.SaveChanges();
            return RedirectToAction("Blogpostdetail");
        }
    }
}
