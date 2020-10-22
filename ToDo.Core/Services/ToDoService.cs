using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Core.Services
{
    public class ToDoService : IToDoService
    {
        public void AddEntry(string title, string description)
        {
            // save the entry to the db here
        }

        public void RemoveEntry(string title)
        {
            // remove the entry from the db here
        }

        public List<Models.ToDo> LoadEntries()
        {
            return new List<Models.ToDo>();
        }
    }
}
