using Feeder.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freeder.BLL.DTOs
{
    public class FeedDTO
    {
        public string Link { get; set; }
        public string Title { get; set; }
        //public string Author { get; set; }
        public string PublishDate { get; set; }
        public string Content { get; set; }
    }
}
