using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Abstractions;

namespace SurveyBasket.Controllers;


[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken) 
        =>  Ok(await _pollService.GetAllAsync(cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.GetAsync(id, cancellationToken);

        return result.IsSucceed
            ? Ok(result.Value)
            : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] PollRequest request, CancellationToken cancellationToken)
    {
        var response = await _pollService.AddAsync(request, cancellationToken); 

        return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request, CancellationToken cancellationToken)
    {
        var result = await _pollService.UpdateAsync(id, request, cancellationToken); 

        return result.IsSucceed 
            ? NoContent() 
            : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.DeleteAsync(id, cancellationToken);

        return result.IsSucceed
            ? NoContent()
            : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description);
    }

    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

        return result.IsSucceed
            ? NoContent()
            : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description);
    }



}
