using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Structure.Data.Common;
using Structure.MediatR.Student.Commands;
using Structure.MediatR.Student.Queries;

namespace Structure.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[Produces("application/json")]
public class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(
        IMediator mediator,
        ILogger<StudentsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest(ApiResponse<object>.Failure(
                    "Invalid pagination parameters",
                    new List<string>
                    {
                        "Page number and page size must be greater than 0"
                    }));
            }

            pageSize = Math.Min(pageSize, 100);

            var result = await _mediator.Send(new GetAllStudentsQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving students");

            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.Failure(
                    "Error retrieving students",
                    new List<string> { ex.Message }));
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                return BadRequest(ApiResponse<object>.Failure(
                    "Invalid student ID",
                    new List<string> { "Student ID cannot be empty" }));
            }

            var result = await _mediator.Send(new GetStudentByIdQuery(id));

            if (!result.IsSuccess)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving student");

            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.Failure(
                    "Error retrieving student",
                    new List<string> { ex.Message }));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CreateStudentCommand command)
    {
        try
        {
            if (command == null)
            {
                return BadRequest(ApiResponse<object>.Failure(
                    "Invalid request body",
                    new List<string> { "Request body cannot be empty" }));
            }

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Data?.Id },
                result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating student");

            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.Failure(
                    "Error creating student",
                    new List<string> { ex.Message }));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateStudentCommand command)
    {
        try
        {
            if (id == Guid.Empty)
            {
                return BadRequest(ApiResponse<object>.Failure(
                    "Invalid student ID",
                    new List<string> { "Student ID cannot be empty" }));
            }

            if (command == null)
            {
                return BadRequest(ApiResponse<object>.Failure(
                    "Invalid request body",
                    new List<string> { "Request body cannot be empty" }));
            }

            command.Id = id;

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                if (result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                {
                    return NotFound(result);
                }

                return BadRequest(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating student");

            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.Failure(
                    "Error updating student",
                    new List<string> { ex.Message }));
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                return BadRequest(ApiResponse<object>.Failure(
                    "Invalid student ID",
                    new List<string> { "Student ID cannot be empty" }));
            }

            var result = await _mediator.Send(new DeleteStudentCommand
            {
                Id = id
            });

            if (!result.IsSuccess)
            {
                return NotFound(result);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting student");

            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.Failure(
                    "Error deleting student",
                    new List<string> { ex.Message }));
        }
    }
}