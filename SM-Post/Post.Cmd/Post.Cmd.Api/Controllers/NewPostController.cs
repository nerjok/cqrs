using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.DTOs;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class NewPostController : ControllerBase
{
    private readonly ILogger<NewPostController> _logger;
    private readonly ICommandDispatcher _commandDispatcher;

    public NewPostController(ILogger<NewPostController> logger, ICommandDispatcher commandDispatcher)
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
    }

    [HttpPost]
    public async Task<ActionResult> NewPostAsync(PostCommand command)
    {
        try
        {
            var id = Guid.NewGuid();
            command.Id = id;
            await _commandDispatcher.SendAsync(command);
            return StatusCode(StatusCodes.Status201Created, new NewPostResponse
            {
                Message = "New post creation request completed succesfully!"
            });
        }
        catch (System.Exception ex)
        {
            _logger.Log(LogLevel.Warning, "Client made a wrong request");
            // return StatusCode(StatusCodes.Status400BadRequest);
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
    }
    [HttpGet("kuku")]
    public string TestMethod()
    {
        return "testMethod";
    }
}
