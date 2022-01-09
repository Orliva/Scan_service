using Scan;
using System.Collections.Generic;

namespace Scan_service.Model
{
    /// <summary>
    /// Контекст сканирования
    /// </summary>
    public class CommandContext
    {
        public int IdTask { get; private set; }    // id сканрирования
        public bool IsDone { get; set; }           // Статус готовности отчета о сканировании (закончили сканирование или нет)
        public List<Report> Reports { get; set; }  // Список отчетов (если в аргументах передано больше 1 директории)
        public string[] Args { get; }              // Аргументы переданные на сканирование

        public CommandContext(int id, string[] args, bool status = false)
        {
            IdTask = id;
            Args = args;
            Reports = new List<Report>();
            IsDone = status;
        }

        /// <summary>
        /// Преобразовываем все отчеты в один массив строк
        /// </summary>
        /// <returns>Готовый к дальнейшему выводу общий отчет</returns>
        public string[] ConvertToStrArr()
        {
            List<string> res = new List<string>();

            foreach(var i in Reports)
            {
                foreach (var s in i.Result)
                {
                    res.Add(s);
                }
            }
            return res.ToArray();
        }
    }
}
