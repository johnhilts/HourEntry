using System.Collections.Generic;
using HourEntry.Infrastructure.Database.Data;

namespace HourEntry.Infrastructure.Database
{
    public interface IHourEntryRepository
    {
        List<HourEntryData> GetHourEntryList();
        HourEntryData GetHourEntryDataByHourEntryId(int hourEntryId);
        void SaveHourEntryData(HourEntryData hourEntryData);
    }
}