using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HourEntry.Infrastructure.Database;
using HourEntry.Infrastructure.Database.Data;

namespace HourEntry.Services
{
    public class HourEntryService
    {
        private readonly IHourEntryRepository _hourEntryRepository;

        public HourEntryService(IHourEntryRepository hourEntryRepository)
        {
            _hourEntryRepository = hourEntryRepository;
        }

        public List<HourEntryData> GetListOfAllHourEntries()
        {
            return this._hourEntryRepository.GetHourEntryList();
        }

        public HourEntryData GetHourEntryDataByHourEntryId(int hourEntryId)
        {
            return this._hourEntryRepository.GetHourEntryDataByHourEntryId(hourEntryId);
        }

        public void SaveHourEntryData(HourEntryData hourEntryData)
        {
            this._hourEntryRepository.SaveHourEntryData(hourEntryData);
        }
    }
}
