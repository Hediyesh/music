using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Music
{
    public class SingerMusicsViewModel
    {
        public int singerid { get; set; }
        public string singername { get; set; }
        public int singergpid { get; set; }
        public int singergenreid { get; set; }
        public string singergenreName { get; set; }
        public int musicid { get; set; }
        public string musicname { get; set; }
        public int musicgenreid { get; set; }
        public string musicgenreName { get; set; }
        public int musicgpid { get; set; }
        public int musiclanid { get; set; }
        public string musiclanName { get; set; }
        public int musiclikes { get; set; }
        public int musicviews { get; set; }
        public string musicdate { get; set; }
        public string musicfilelocation { get; set; }
        public string musicnote { get; set; }
        public string musicpic { get; set; }
    }
}