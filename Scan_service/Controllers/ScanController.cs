using Microsoft.AspNetCore.Mvc;
using Scan_service.Model;
using Scan_service.Service;
using System.Threading.Tasks;

namespace Scan_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScanController : ControllerBase
    {
        public ScanController() { }

        /// <summary>
        /// Запускаем процесс сканирования
        /// </summary>
        /// <param name="id">id сканирования</param>
        /// <returns></returns>
        [HttpGet]
        public async Task Get(int id, [FromServices] CommandContextService commandService, [FromServices] IScanService scanService)
        {
            CommandContext commandContext = commandService.GetCommandContext(id);
         
            scanService.Scan(commandContext.Args, commandContext);
        }

        
        /// <summary>
        /// Создаем контекст для сканирования
        /// </summary>
        /// <param name="args">Аргументы для сканирования</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Get(string[] args, [FromServices] CommandContextService commandService)
        {
            CommandContext commandContext = commandService.CreateCommandContext(args);

            return new JsonResult("Scan task was created with ID: " + commandContext.IdTask.ToString());
        }
    }
}
