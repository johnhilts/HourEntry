using System;
using System.Configuration;
using System.Data.Linq;

namespace HourEntry.Infrastructure.Database
{
    public class BaseRepository
    {
        protected string _connectionString;

        public BaseRepository()
        {
            if (ConfigurationManager.ConnectionStrings != null && ConfigurationManager.ConnectionStrings["HourEntryConnection"] != null)
            {
                this._connectionString = ConfigurationManager.ConnectionStrings["HourEntryConnection"].ToString() ?? "";
            }
            if (string.IsNullOrEmpty(this._connectionString))
            {
                throw new Exception("Not able to retrieve connection string");
            }
        }

        protected Table<T> GetData<T>() where T : class
        {
            Table<T> table = (new DataContext(this._connectionString)).GetTable<T>();

            return table;
        }

        public T InsertData<T>(T data) where T : class
        {
            var memberTable = this.GetData<T>();
            memberTable.InsertOnSubmit(data);
            memberTable.Context.SubmitChanges();

            return data;
        }

        public void UpdateData<T>(T data) where T : class
        {
            var table = this.GetData<T>();

            if (table.GetOriginalEntityState(data) == null)
            {
                // we're updating an existing record, but it's not attached to this data context,
                // so attach it and detect the changes
                this.UpdateData<T>(table, data);
            }
        }

        private void UpdateData<T>(Table<T> table, T data) where T : class
        {
            table.Attach(data);
            table.Context.Refresh(RefreshMode.KeepCurrentValues, data);
            table.Context.SubmitChanges();
        }

        public void DeleteData<T>(T data) where T : class
        {
            var table = this.GetData<T>();

            if (table.GetOriginalEntityState(data) == null)
            {
                table.Attach(data);
                table.DeleteOnSubmit(data);
                table.Context.SubmitChanges();
            }
        }

    }
}
