using Microsoft.AspNetCore.Mvc;
using TaskServiceVer._2._0.Services;

namespace TaskServiceVer._2._0.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TaskControllerWithQueue : ControllerBase
    {
        private readonly TaskProcessor _processor;

        public TaskControllerWithQueue(TaskProcessor taskProcessor)
        {
            _processor = taskProcessor;
        }

        [HttpPost]
        public IActionResult CreateTask()
        {
            var task = _processor.CreateTask();
            return Accepted(new { id = task.Id });
        }

        [HttpGet("{id}")]
        public IActionResult GetTask(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest("Invalid GUID format.");
            }

            var task = _processor.GetTaskById(guid);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                id = task.Id,
                status = task.Status.ToString(),
                updated = task.Timestamp
            });
        }
    }
}
