//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class LikedSongText
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public int SongId { get; set; }
    
        public virtual Karbar Karbar { get; set; }
        public virtual Song Song { get; set; }
    }
}
