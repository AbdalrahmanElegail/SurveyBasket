
using System.Collections;

namespace SurveyBasket.Controllers;


[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    [HttpGet("")]
    public IActionResult GetAll() 
    {
        var polls = _pollService.GetAll(); // Get all polls from database
        var response = polls.Adapt<IEnumerable<PollResponse>>(); // Map it to IEnumerable<PollResponse> to be sent as a response

        return Ok(response);
    }

    [HttpGet("{id}")]
    public IActionResult Get([FromRoute] int id)
    {
        var poll = _pollService.Get(id); // Get poll from database
        var response = poll.Adapt<PollResponse>(); // Map it to PollResponse to be sent as a response

        return response is null ? NotFound() : Ok(response);
    }

    [HttpPost]
    public IActionResult Add([FromBody] CreatePollRequest request)
    {
        var poll = request.Adapt<Poll>(); // Map it to Poll to be added to database
        var newPoll = _pollService.Add(poll); // newly created poll with new Id
        var response = newPoll.Adapt<PollResponse>(); // to be sent as a response

        return CreatedAtAction(nameof(Get), new {id = newPoll.Id}, response);
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] int id,[FromBody] CreatePollRequest request)
    {
        var poll = request.Adapt<Poll>(); // Map it to Poll to be updated in database
        var isUpdated = _pollService.Update(id, poll); // true if updated, false if not found

        return isUpdated ? NoContent() : NotFound(); 
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    { 
        var isDeleted = _pollService.Delete(id); // true if deleted, false if not found

        return isDeleted ? NoContent() : NotFound();
    }
}
