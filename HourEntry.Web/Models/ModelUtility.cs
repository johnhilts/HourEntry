using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HourEntry.Web.Models
{
    public class ModelUtility
    {
        public List<SelectListItem> GetSelectList<T>(string selected, List<T> list)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (T item in list)
            {
                selectList.Add(new SelectListItem
                             {
                                 Selected = false,
                                 Text = item.ToString(),
                                 Value = item.ToString(),
                             });
            }

            return selectList; // SetSelectedListItem(selected, selectList);
        }

        //public List<SelectListItem> SetSelectedListItem(string selected, List<SelectListItem> selectList)
        //{
        //    if (string.IsNullOrEmpty(selected))
        //        return selectList;

        //    int index = selectList.FindIndex(x => x.Value == selected);
        //    if (index >= 0)
        //        selectList[index].Selected = true;

        //    return selectList;
        //}
    }
}