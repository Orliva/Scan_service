using Microsoft.AspNetCore.Mvc;
using Scan_service.Model;
using Scan_service.Service;
using System.Threading.Tasks;

namespace Scan_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        public StatusController() { }

        [HttpGet]
        public async Task<JsonResult> Get(int id, [FromServices] CommandContextService commandService)
        {
            CommandContext commandContext = commandService.GetCommandContext(id); //Получаем контекст сканирования
            if (commandContext != null)     //Если контекст есть, то проверь
            {
                if (commandContext.IsDone == true)  // закончено ли сканированиие?
                    return new JsonResult(commandContext.ConvertToStrArr()); //Создаем общий отчет и отправляем клиенту
                else
                    return new JsonResult(new string[] { "Scan task in progress, please wait" }); // Отправляем клиенту сообщение
            }
            else
                return new JsonResult(new string[] { $"Wrong Id = {id}, please retry" }); //Отправляем пользователю ошибку
        }
    }
}
