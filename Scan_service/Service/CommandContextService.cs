using Scan_service.Model;
using System;
using System.Collections.Generic;

namespace Scan_service.Service
{
    /// <summary>
    /// Сервис для создания и работы с контекстами сканирования
    /// </summary>
    public class CommandContextService
    {
        private List<CommandContext> commandContexts { get; set; } // Список контекстов
        private List<int> listId { get; set; }                     // Список id
        private Random rnd;
        private static object locker = new object();

        public CommandContextService()
        {
            commandContexts = new List<CommandContext>();
            listId = new List<int>();
            rnd = new Random();
        }

        /// <summary>
        /// По Id определяет контекст сканирования
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CommandContext GetCommandContext(int id)
        {
            lock (locker)
            {
                foreach (var i in commandContexts)
                {
                    if (i.IdTask == id)
                        return i;
                }
            }
            return null;
        }

        /// <summary>
        /// Создаем контекст для сканирования
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public CommandContext CreateCommandContext(string[] args)
        {
            int id;
            CommandContext tmp;

            lock (locker)
            {
                tmp = new CommandContext((id = GenerateId()), args);
                commandContexts.Add(tmp);
                listId.Add(id);
            }
            return tmp;
        }

        /// <summary>
        /// Получаем уникальный Id
        /// </summary>
        /// <returns></returns>
        private int GenerateId()
        {
            int tmp;
            while (listId.Contains((tmp = rnd.Next(1_000)))) ;

            return tmp;
        }
    }
}
