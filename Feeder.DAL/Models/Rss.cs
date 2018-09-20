using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL.Models
{
    public class Rss
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
    }
}
