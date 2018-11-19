using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace WebMvc.Areas.Web.Dal
{
   
    public class FileOperation
    {
        static  FtpClient ftp = new FtpClient("120.79.146.123", "jane.zhou1", "309645512");
        public static string UploadFile(HttpPostedFileBase file)
        {
          bool a=  ftp.WebUpload(file);
            if (a)
            {

                return "OK";


            }else
            {

                return "NO";
            }


            

        }
        public static string AllFiles()
        {
            List<string> ftpFiles = ftp.GetFileDetails();
            string str = "";
            for (int i = 0; i < ftpFiles.Count; i++)
            {

                str += ftpFiles[i];



            }



            return str;




        }



    }



}