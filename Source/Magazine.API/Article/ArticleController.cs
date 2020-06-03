using Infotecs.Magazine.Infrastracture.Contracts;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;

namespace Infotecs.Magazine.API.Article
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        readonly IEntityService<Domain.Article.Article> _articleService;
        readonly ILogger _logger;

        public ArticleController(IEntityService<Domain.Article.Article> articleService,
                                 ILogger logger)
        {
            _articleService = articleService;
            _logger = logger.ForContext<ArticleController>();
        }

        // GET: api/article
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _logger.Debug("Start get request");
                return Ok(_articleService.Get());
            }
            finally
            {
                _logger.Debug("Complete get request");
            }
        }

        // GET: api/article/4
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                _logger.Debug("Start get request for id = {id}", id);
                return Ok(_articleService.Get(id));
            }
            finally
            {
                _logger.Debug("Complete get request for id = {id}", id);
            }
        }

        // POST: api/article
        [HttpPost]
        public IActionResult Add(Domain.Article.Article article)
        {
            try
            {
                _logger.Debug("Start add request for {article}", article);
                return Ok(_articleService.Add(article));
            }
            catch (ArgumentException ex)
            {
                _logger.Warning(ex, "Warning while add request for {article}", article);
                return BadRequest(ex);
            }
            finally
            {
                _logger.Debug("Complete add request for {article}", article);
            }
        }

        // PUT: api/article/4
        [HttpPut("{id}")]
        public IActionResult Update(Domain.Article.Article article)
        {
            try
            {
                _logger.Debug("Start update request for {article}", article);
                return Ok(_articleService.Update(article));
            }
            finally
            {
                _logger.Debug("Complete update request for {article}", article);
            }
        }

        // DELETE: api/article/4
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _logger.Debug("Start delete request for id = {id}", id);
                return Ok(_articleService.Delete(id));
            }
            finally
            {
                _logger.Debug("Complete delete request for id = {id}", id);
            }
        }
    }
}
