using Microsoft.AspNetCore.Authorization;

namespace SurveyBasket.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken) 
    {
        var polls = await _pollService.GetAllAsync(cancellationToken); // Get all polls from database
        var response = polls.Adapt<IEnumerable<PollResponse>>(); // Map it to IEnumerable<PollResponse> to be sent as a response

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var poll = await _pollService.GetAsync(id, cancellationToken); // Get poll from database
        var response = poll.Adapt<PollResponse>(); // Map it to PollResponse to be sent as a response

        return response is null ? NotFound() : Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] PollRequest request
        , CancellationToken cancellationToken)
    {
        var poll = request.Adapt<Poll>(); // Map it to Poll to be added to database
        var newPoll = await _pollService.AddAsync(poll, cancellationToken); // newly created poll with new Id
        var response = newPoll.Adapt<PollResponse>(); // to be sent as a response

        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id,
        [FromBody] PollRequest request,
        CancellationToken cancellationToken)
    {
        var poll = request.Adapt<Poll>(); // Map it to Poll to be updated in database
        var isUpdated = await _pollService.UpdateAsync(id, poll, cancellationToken); // true if updated, false if not found

        return isUpdated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var isDeleted = await _pollService.DeleteAsync(id, cancellationToken); // true if deleted, false if not found

        return isDeleted ? NoContent() : NotFound();
    }

    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        var isUpdated = await _pollService.TogglePublishStatusAsync(id, cancellationToken); // true if updated, false if not found

        return isUpdated ? NoContent() : NotFound();
    }



}
