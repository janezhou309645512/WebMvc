using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMvc.Areas.Web.Models
{
    public class PageOut<T>
    {
        public List<T> list { get; set; }//每页显示数据的集合 
        public int total { get; set; }


    }
}