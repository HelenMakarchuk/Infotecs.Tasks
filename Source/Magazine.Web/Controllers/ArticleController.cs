using Infotecs.Magazine.Infrastracture.Contracts.Service;
using Magazine.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Magazine.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        IEntityService<Article> _articleService;
        ILogger _logger;

        public ArticleController(IEntityService<Article> articleService,
                                 ILogger logger)
        {
            _articleService = articleService;
            _logger = logger;
        }

        // GET: api/article
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_articleService.Get());
        }

        // GET: api/article/4
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_articleService.Get(id));
        }

        // POST: api/article
        [HttpPost]
        public IActionResult Add(Article article)
        {
            return Ok(_articleService.Add(article));
        }

        // PUT: api/article/4
        [HttpPut("{id}")]
        public IActionResult Update(Article article)
        {
            return Ok(_articleService.Update(article));
        }

        // DELETE: api/article/4
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return Ok(_articleService.Delete(id));
        }
    }
}