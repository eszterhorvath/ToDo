using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using SQLite;

namespace ToDo.Core.Models
{
    public class ToDo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } = 0;

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
