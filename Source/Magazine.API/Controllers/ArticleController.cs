using Magazine.Domain.Entities;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq;

namespace Magazine.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        IUnitOfWork _unitOfWork;
        ILogger _logger;

        public ArticleController(IUnitOfWork unitOfWork,
                                 ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: api/article
        [HttpGet]
        public IActionResult Get()
        {
            var articles = _unitOfWork.ArticleRepository.Select(a => new Article() { Id = a.Id, Title = a.Title }).ToList();

            Response.Headers.Add("Access-Control-Allow-Origin", "*");

            return Ok(articles);
        }

        // GET: api/article/4
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var article = _unitOfWork.ArticleRepository.Include(a => a.Comments).ThenInclude(c => c.Account).SingleOrDefault(a => a.Id == id);

            Response.Headers.Add("Access-Control-Allow-Origin", "*");

            return Ok(article);
        }

        // POST: api/article
        [HttpPost]
        public IActionResult Add(Article article)
        {
            var dbArticle = _unitOfWork.ArticleRepository.SingleOrDefault(a => a.Title == article.Title);

            if (dbArticle != null)
                return BadRequest();

            var entry = _unitOfWork.ArticleRepository.Add(article);
            _unitOfWork.Commit();

            Response.Headers.Add("Access-Control-Allow-Origin", "*");

            return Ok(entry.Entity);
        }
    }
}