using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Core.Services
{
    public interface IToDoService
    {
        void AddEntry(string title, string description);
        void RemoveEntry(string title);
        List<Models.ToDo> LoadEntries();
    }
}
