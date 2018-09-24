using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL.Models
{
    public class Feed : IModel
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public FeedType Type { get; set; }
        public string Author { get; set; }
        public string PublishDate { get; set; }
        public string Content { get; set; }
        public int SourceId { get; set; }
        public Source Source { get; set; }

    }
}
