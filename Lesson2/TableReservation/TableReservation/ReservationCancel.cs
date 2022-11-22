using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableReservation;

namespace TableReservation
{
    public  class ReservationCancel
    {
        private readonly List<Table> _tables = new();
        public Task CheckReservation(Restaurant rest)
        {
            foreach (var table in rest.GetTables()) 
            {
                _tables.Add(table);
            }
            foreach(var c in _tables)
            {
                if (c.State == State.Blocked)
                {
                   CancelReservation.ReservationCancel(c);
                }
            }
            return Task.CompletedTask;
        }
    }

    public class CancelReservation
    {
        public static void ReservationCancel(Table table)
        {
           table.SetState(State.Free);
        }

    }
}
