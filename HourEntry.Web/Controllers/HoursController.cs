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
            return View(new TimeSheetModel { StartDate = DateTime.Today });
        }

    }
}
