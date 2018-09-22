using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL.Models
{
    public class Feed
    {
        public int Id { get; set; }
        string Link { get; set; }
        string Title { get; set; }
        public FeedType Type { get; set; }
        public string Author { get; set; }
        public string PublishDate { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public int SourceId { get; set; }
        public Source Source { get; set; }

    }
}
