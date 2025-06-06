using Microsoft.AspNetCore.Mvc;

namespace TaskService.Controllers
{
    [ApiController]
    [Route("task")]
    public class TaskController : ControllerBase
    {
        private readonly TaskProcessor _processor;

        public TaskController(TaskProcessor processor)
        {
            _processor = processor;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask()
        {
            var id = await _processor.CreateTaskAsync();
            return Accepted(new { id });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(string id)
        {
            if (!Guid.TryParse(id, out var guid))
                return BadRequest("Invalid GUID");

            var task = await _processor.GetTaskAsync(guid);
            if (task == null)
                return NotFound();

            return Ok(new { status = task.Status.ToString() });
        }
    }
}
