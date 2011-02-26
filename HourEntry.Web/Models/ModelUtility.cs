using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HourEntry.Web.Models
{
    public class ModelUtility
    {
        public List<SelectListItem> GetHourSelectList(string selected, List<short> hourList)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (short hour in hourList)
            {
                list.Add(new SelectListItem
                             {
                                 Selected = false,
                                 Text = hour.ToString(),
                                 Value = hour.ToString(),
                             });
            }

            return SetSelectedListItem(selected, list);
        }

        public List<SelectListItem> SetSelectedListItem(string selected, List<SelectListItem> list)
        {
            if (string.IsNullOrEmpty(selected))
                return list;

            int index = list.FindIndex(x => x.Value == selected);
            if (index >= 0)
                list[index].Selected = true;

            return list;
        }
    }
}