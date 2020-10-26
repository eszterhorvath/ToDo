using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using SQLite;

namespace ToDo.Core.Models
{
    public enum State
    {
        Pending,
        Done
    };

    public class ToDo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } = 0;

        public string Title { get; set; }

        public string Description { get; set; }

        public State State { get; set; }
    }

    public class TodoComparer : IComparer<ToDo>
    {
        // todo: if a done is changed to pending: it should be the last element of the pending list
        // todo: and if a pending item is changed to done: it should be on the top of the done list
        int IComparer<ToDo>.Compare(ToDo t1, ToDo t2)
        {
            if (t1.State == State.Done && t2.State == State.Done)
            {
                return 0;
            }
            else if (t1.State == State.Pending && t2.State == State.Done)
            {
                return -1;
            }
            else if (t1.State == State.Done && t2.State == State.Pending)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
