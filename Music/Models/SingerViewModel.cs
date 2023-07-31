using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Music
{
    public class SingerViewModel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int gpid { get; set; }
        public int gnreid { get; set; }
        public int languageid { get; set; }
        public string languageName{ get; set; }
        public string genreName { get; set; }
    }
}