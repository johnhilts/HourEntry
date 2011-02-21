using System;
using System.Configuration;
using System.Web;

namespace Bll.HourEntry
{
    public class BllBase
    {
        protected string _DataPath;
        public string DataPath
        {
            set { _DataPath = value; }
        }

        protected Dal.IDataHelper _DataHelper;
        public Dal.IDataHelper DataHelper
        {
            set { this._DataHelper = value; }
        }

        public BllBase()
            : this("", true)
        {
        }
        public BllBase(string dataPath)
            : this(dataPath, false)
        {
        }
        private BllBase(string dataPath, bool useHttp)
        {
            //if (useHttp)
            //    dataPath = this.GetDataPath();

            this._DataHelper = new Dal.DataHelper(dataPath);
        }
        //private string GetDataPath()
        //{
        //    HttpContext context = this.GetHttpContext();
        //    string root = context.Server.MapPath("~/");
        //    string dataPath = root + ConfigurationManager.AppSettings["DataPath"];
        //    return dataPath;
        //}
        //private HttpContext GetHttpContext()
        //{
        //    if (HttpContext.Current == null)
        //        throw new ApplicationException("Parameterless ctor requires HTTP Context");

        //    return HttpContext.Current;
        //}
    }
}
