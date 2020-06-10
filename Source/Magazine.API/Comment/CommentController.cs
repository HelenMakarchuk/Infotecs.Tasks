using Infotecs.Magazine.Infrastracture.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;

namespace Infotecs.Magazine.API.Comment
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        readonly IEntityService<Domain.Comment.Comment> _commentService;
        readonly ILogger _logger;

        public CommentController(IEntityService<Domain.Comment.Comment> commentService,
                                 ILogger logger)
        {
            _commentService = commentService;
            _logger = logger.ForContext<CommentController>();
        }

        // GET: api/comment
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            try
            {
                _logger.Debug("Start get request");
                return Ok(_commentService.Get());
            }
            finally
            {
                _logger.Debug("Complete get request");
            }
        }

        // GET: api/comment/4
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult Get(int id)
        {
            try
            {
                _logger.Debug("Start get request for id = {id}", id);
                return Ok(_commentService.Get(id));
            }
            finally
            {
                _logger.Debug("Complete get request for id = {id}", id);
            }
        }

        // POST: api/comment
        [HttpPost]
        public IActionResult Add(Domain.Comment.Comment comment)
        {
            try
            {
                _logger.Debug("Start add request for {comment}", comment);
                return Ok(_commentService.Add(comment));
            }
            catch (ArgumentException ex)
            {
                _logger.Warning(ex, "Warning while add request for {comment}", comment);
                return BadRequest(ex);
            }
            finally
            {
                _logger.Debug("Complete add request for {comment}", comment);
            }
        }

        // PUT: api/comment/4
        [HttpPut("{id}")]
        public IActionResult Update(Domain.Comment.Comment comment)
        {
            try
            {
                _logger.Debug("Start update request for {comment}", comment);
                return Ok(_commentService.Update(comment));
            }
            finally
            {
                _logger.Debug("Complete update request for {comment}", comment);
            }
        }

        // DELETE: api/comment/4
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _logger.Debug("Start delete request for id = {id}", id);
                return Ok(_commentService.Delete(id));
            }
            finally
            {
                _logger.Debug("Complete delete request for id = {id}", id);
            }
        }
    }
}
