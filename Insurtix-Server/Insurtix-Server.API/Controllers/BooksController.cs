using Insurtix_Server.BL.Services;
using Insurtix_Server.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Insurtix_Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BooksService booksService;
        public BooksController(BooksService _booksService)
        {
            booksService = _booksService;
        }
        [HttpGet]
        public ActionResult<List<Book>> GetBooks()
        {
            List<Book> result = booksService.GetAllBooks();
            return Ok(result);
        }
    }
}
