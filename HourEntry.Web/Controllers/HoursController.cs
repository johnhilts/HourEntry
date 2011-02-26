using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HourEntry.Services;
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
            ModelUtility modelUtility = new ModelUtility();
            PresenterService presenterService = new PresenterService();
            List<SelectListItem> hourList = modelUtility.GetHourSelectList("", presenterService.GetDefaultTimeSheet().HourList);
            ViewBag.StartHourList = hourList;

            List<SelectListItem> amPmList = new List<SelectListItem>
                       {
                           new SelectListItem {Selected = false, Text = "AM", Value = "AM"},
                           new SelectListItem {Selected = false, Text = "PM", Value = "PM"},
                       };

            ViewBag.AmPmList = amPmList;
            TimeSheetModel model = new TimeSheetModel
                                       {
                                           StartHour = 1,
                                           StartAmPm = "AM",
                                           StartDate = DateTime.Today,
                                           EndDate = DateTime.Today,
                                       };

            return View(model);
        }

    }
}
