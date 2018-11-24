using HK.Pub.Externd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMvc.Areas.Web.Dal
{
    public class Test
    {
        static HK.Pub.Dal.Sql dbMmsDataCollection = new HK.Pub.Dal.Sql(HK.ConfigFile.ConnectiongString_Get("MMS"));


        public static string MQueryList(int start, int limit)
        {


            return dbMmsDataCollection.QueryDataTable(@"select top 30 * from dbo.mms_material").ToJson();

        }






    }
}