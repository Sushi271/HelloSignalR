using HelloSignalR.Clients;
using HelloSignalR.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace HelloSignalR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        IHubContext<HelloHub, IHelloClient> _helloHub;

        public HelloController(IHubContext<HelloHub, IHelloClient> helloHub)
        {
            _helloHub = helloHub;
        }

        [HttpGet("{team}/{message}")]
        public async Task<IActionResult> SendMessageToTeam([FromRoute]string team, [FromRoute]string message)
        {
            await _helloHub.Clients.Group(team).UpdateText($"ADMIN MESSAGE\n{message}");
            return Ok();
        }
    }
}