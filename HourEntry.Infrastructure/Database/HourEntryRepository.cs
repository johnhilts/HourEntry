using System.Collections.Generic;
using System.Linq;
using HourEntry.Infrastructure.Database.Data;

namespace HourEntry.Infrastructure.Database
{
    public class HourEntryRepository : BaseRepository, IHourEntryRepository
    {
        public List<HourEntryData> GetHourEntryList()
        {
            var hourEntryTable = base.GetData<HourEntryData>();

            IQueryable<HourEntryData> hourEntryList = hourEntryTable;

            return hourEntryList.ToList();
        }

        public HourEntryData GetHourEntryDataByHourEntryId(int hourEntryId)
        {
            var hourEntryTable = base.GetData<HourEntryData>();

            IQueryable<HourEntryData> hourEntryData = hourEntryTable;

            return hourEntryData.Where(m => m.HourEntryId.Equals(hourEntryId)).SingleOrDefault();
        }

        public void SaveHourEntryData(HourEntryData hourEntryData)
        {
            if (this.GetHourEntryDataByHourEntryId(hourEntryData.HourEntryId) == null)
            {
                base.InsertData(hourEntryData);
            }
            else
            {
                base.UpdateData(hourEntryData);
            }
        }
    }
}