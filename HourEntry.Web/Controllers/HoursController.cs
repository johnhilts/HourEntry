using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HourEntry.Services;
using HourEntry.Services.Data;
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
            DefaultTimeSheet defaultTimeSheet = presenterService.GetDefaultTimeSheet(DateTime.Now);
            ViewBag.StartHourList = modelUtility.GetSelectList(defaultTimeSheet.Hour.ToString(), defaultTimeSheet.HourList);
            ViewBag.StartMinuteList = modelUtility.GetSelectList("", defaultTimeSheet.MinuteList);
            ViewBag.StartAmPmList = modelUtility.GetSelectList("", defaultTimeSheet.AmPmList);
            ViewBag.EndHourList = modelUtility.GetSelectList("", defaultTimeSheet.HourList);
            ViewBag.EndMinuteList = modelUtility.GetSelectList("", defaultTimeSheet.MinuteList);
            ViewBag.EndAmPmList = modelUtility.GetSelectList("", defaultTimeSheet.AmPmList);

            TimeSheetModel model = new TimeSheetModel
                                       {
                                           StartHour = defaultTimeSheet.Hour,
                                           StartMinute = 1,
                                           StartAmPm = "AM",
                                           EndHour = defaultTimeSheet.Hour,
                                           EndMinute = 1,
                                           EndAmPm = "AM",
                                           StartDate = DateTime.Today,
                                           EndDate = DateTime.Today,
                                       };

            return View(model);
        }

    }
}
