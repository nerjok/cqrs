using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
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
    private readonly IEventStoreRepository _eventStoreRepository;

    public NewPostController(ILogger<NewPostController> logger, ICommandDispatcher commandDispatcher, IEventStoreRepository eventStoreRepository)
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
        _eventStoreRepository = eventStoreRepository;
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
                Id = command.Id,
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

    [HttpPut("{id}")]
    public async Task<ActionResult> EditMessageAsync(Guid id, EditMessageCommand command)
    {
        try
        {
            command.Id = id;
            await _commandDispatcher.SendAsync(command);

            return Ok(new BaseResponse
            {
                Message = "Edit message request completed successfully!"
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Client made a bad request!");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (AggregateNotFoundException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Could not retrieve aggregate, client passed an incorrect post ID targetting the aggregate!");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to edit the message of a post!";
            _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = SAFE_ERROR_MESSAGE
            });
        }
    }

    [HttpDelete("remove")]
    public async Task<ActionResult> DeletePostAsync(Guid id, DeletePostCommand command)
    {
        try
        {
            command.Id = id;
            await _commandDispatcher.SendAsync(command);

            return Ok(new BaseResponse
            {
                Message = "Edit message request completed successfully!"
            });
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to delete post!";
            _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = SAFE_ERROR_MESSAGE
            });
        }
    }

    [HttpGet("testcall")]
    public string TestMethod()
    {
        return "testMethod";
    }

    [HttpGet("{id}/events")]
    public async Task<IEnumerable<object>> GetAggregateEvents(Guid id)
    {
        var events = await _eventStoreRepository.FindByAggregateId(id);
        return events.Select(x => {
            var data = x.EventData;
            return data;
            });
    }
}
