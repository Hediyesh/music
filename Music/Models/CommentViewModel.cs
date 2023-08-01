using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Music
{
    public class CommentViewModel
    {
        public int id { get; set; }
        public DateTime createDate { get; set; }
        public string createDateString { get; set; }
        public int songid { get; set; }
        public string text { get; set; }
        public string userpic { get; set; }
        public string username { get; set; }
        public int userid { get; set; }
        public int replied { get; set; }
    }
}