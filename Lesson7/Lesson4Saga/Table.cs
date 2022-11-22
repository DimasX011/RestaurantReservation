using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson4Saga
{
    public class Table
    {
        public TableState State { get; private set; }
        public int SeatsCount { get; }
        public int Id { get; }

        public Table(int id)
        {
            Id = id; 
            State = TableState.Free; 
            SeatsCount = Random.Next(2, 5); 
        }

        public bool SetState(TableState state)
        {
            lock (_lock)
            {
                if (state == State)
                    return false;

                State = state;
                return true;
            }
        }

        private readonly object _lock = new object();
        private static readonly Random Random = new();

    }
}
