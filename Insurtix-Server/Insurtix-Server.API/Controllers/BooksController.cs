using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Insurtix_Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetBooks()
        {
            return Ok();
        }
    }
}
