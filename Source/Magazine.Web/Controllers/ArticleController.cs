using Infotecs.Magazine.Infrastracture.Contracts.Service;
using Magazine.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Magazine.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        IEntityService<Article> _articleService;

        public ArticleController(IEntityService<Article> articleService)
        {
            _articleService = articleService;
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
            Article result = null;

            try
            {
                result = _articleService.Add(article);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }

            return Ok(result);
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