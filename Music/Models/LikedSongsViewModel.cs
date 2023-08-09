using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Music
{
    public class LikedSongsViewModel
    {
        public int likeid { get; set; }
        public int userid { get; set; }
        public int songid { get; set; }
        public int singerid { get; set; }
        public string songname { get; set; }
        public string singername { get; set; }
    }
}