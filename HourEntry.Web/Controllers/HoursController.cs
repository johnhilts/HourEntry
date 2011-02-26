using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HourEntry.Web.Models;

namespace HourEntry.Web.Controllers
{
    /// <summary>
    /// Hours Controller - works with UI Service to communicate between view and domain
    /// </summary>
    public class HoursController : Controller
    {
        /// <summary>
        /// GET Hours/TimeSheet
        /// </summary>
        /// <returns>Model</returns>
        public ViewResult TimeSheet()
        {
            List<SelectListItem> amPmList = new List<SelectListItem>
                       {
                           new SelectListItem {Selected = false, Text = "AM", Value = "AM"},
                           new SelectListItem {Selected = false, Text = "PM", Value = "PM"},
                       };

            ViewBag.AmPmList = amPmList;
            TimeSheetModel model = new TimeSheetModel
                                       {
                                           AmPm = "AM",
                                           StartDate = DateTime.Today,
                                           EndDate = DateTime.Today,
                                       };

            return View(model);
        }

    }
}
