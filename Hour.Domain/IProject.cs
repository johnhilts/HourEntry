using System;
using System.Data;

namespace Bll.HourEntry
{
    public interface IProject
    {
        string DataPath { set;}

        DataTable List();
        DataTable GetRecord(int id);
        void Update(int id, string description);
        int Add(string description);
    }
}
