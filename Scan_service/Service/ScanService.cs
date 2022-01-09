using Scan;
using Scan_service.Model;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Scan_service.Service
{
    /// <summary>
    /// Интерфейс сервиса сканирования директорий
    /// </summary>
    public interface IScanService
    {
        Task Scan(string[] args, CommandContext context);
    }

    /// <summary>
    /// Реализация сервиса сканирования директорий
    /// </summary>
    public class ScanService : IScanService
    {
        private readonly string header;
        private readonly string footer;
        private readonly char separator;

        public ScanService() 
        {
            separator = '=';
            header = new string(separator, 12) + " Scan result " + new string(separator, 12);
            footer = new string(separator, 37);
        }

        /// <summary>
        /// Сканируем директории
        /// </summary>
        /// <param name="args">Аргументы для сканирования</param>
        /// <param name="context">Контекст для отслеживания окончание сканирования</param>
        /// <returns></returns>
        public async Task Scan(string[] args, CommandContext context)
        {
            context.Reports = Scaner(args);
            context.IsDone = true;
        }

        /// <summary>
        /// Сканируем директории
        /// </summary>
        /// <param name="args">Аргументы для сканирования</param>
        /// <returns></returns>
        private List<Report> Scaner(string[] args)
        {
            List<Report> reports = new List<Report>();
            Queue<Report> queueDir = new Queue<Report>();
            //Создаем различные виды "подозрительных" файлов
            DubiousFile jsFile = new InnerStrDubiousFile("JS detects: ", "<script>evil_script()</script>", ".js");
            DubiousFile rmFile = new InnerStrDubiousFile("rm -rf detects: ", @"rm -rf %userprofile%\Documents");
            DubiousFile runDllFile = new InnerStrDubiousFile("Rundll32 detects: ", @"Rundll32 sus.dll SusEntry");
            //Устанавливаем цепочку обработчиков
            jsFile.Successor = rmFile;
            rmFile.Successor = runDllFile;

            foreach(string s in args)
            {
                if (Directory.Exists(s))
                {
                    reports.Add(new Report(dirPath: s, startChainNode: jsFile));
                    queueDir.Enqueue(reports[^1]);
                }
            }
            while (queueDir.Count > 0)
            {
                Report tmp = queueDir.Dequeue();
                List<string> tmpList = tmp.GetReport();
                tmpList.Add(footer);
                tmpList.Insert(0, header);
                jsFile.Clear();
                rmFile.Clear();
                runDllFile.Clear();
                FileReader.Clear();
            }
            return reports;
        }
    }
}
