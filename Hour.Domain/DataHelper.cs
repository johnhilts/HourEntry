using System;
using System.IO;
using System.Data;

namespace Bll.HourEntry.Dal
{
    public class DataHelper : IDataHelper
    {
        private string _DataPath;

        public DataHelper(string dataPath)
        {
            this._DataPath = dataPath + "\\";
        }

        DataTable IDataHelper.GetData(string dataObjectName)
        {
            // TODO: figure out how to embed reference to xsd in xml file so we can read using a datatable
            DataSet ds = new DataSet();
            ds.ReadXmlSchema(this._DataPath + dataObjectName + ".xsd");
            string xmlFileName = this._DataPath + dataObjectName + ".xml";
            this.CreateIfNew(xmlFileName, ds);
            ds.ReadXml(xmlFileName, XmlReadMode.ReadSchema);
            return ds.Tables[0];
        }
        private void CreateIfNew(string xmlFileName, DataSet ds)
        {
            if (File.Exists(xmlFileName)) return;

            DataRow dr = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(dr);
            ds.WriteXml(xmlFileName);
        }

        void IDataHelper.SetData(string dataObjectName, DataTable dt)
        {
            dt.WriteXml(this._DataPath + dataObjectName + ".xml");
        }
    }
}
