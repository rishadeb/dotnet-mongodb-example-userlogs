using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserLogApi.Models;
using UserLogApi.Services;

namespace UserLogApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserLogsController : ControllerBase
    {
        private readonly LogsService _logsService;

        public UserLogsController(LogsService logsService) =>
            _logsService = logsService;

        [HttpGet]
        public async Task<List<UserLog>> Get() =>
            await _logsService.GetAsync();
        
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<UserLog>> Get(string id)
        {
            var log = await _logsService.GetAsync(id);
            if(log is null)
            {
                return NotFound();
            }
            return log;
        }

        [HttpGet("tags")]
        public async Task <ActionResult<List<UserLog>>> GetByTags([FromQuery]string tags)
        {
            var log = await _logsService.GetByTagAsync(tags);
            if(log is null)
            {
                return NotFound();
            }
            return log;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserLogInput log)
        {
            UserLog newLog =  new UserLog(){
                User = log.User,
                Title = log.Title,
                StartDate = log.StartDate,
                StopDate = log.StopDate,
                Tags = log.Tags,
                Content = new List<Content>(){new Models.Content(){User = log.User, Log = log.logMessage, Updated = log.StartDate}}
            };
            await _logsService.CreateAsync(newLog);
            return CreatedAtAction(nameof(Get), new {id = newLog.Id}, newLog);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, UserLogDto updateLog)
        {
            var log = await _logsService.GetAsync(id);
            if(log is null)
            {
                return NotFound();
            }
            await _logsService.UpdateAsync(id, updateLog);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var log = await _logsService.GetAsync(id);
            if(log is null)
            {
                return NotFound();
            }
            await _logsService.RemoveAsync(id);
            return NoContent();
        }
    }
}