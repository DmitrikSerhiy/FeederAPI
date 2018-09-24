using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL.Models
{
    public class Source : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? CollectionId { get; set; }
        public Collection Collection { get; set; }
        public IEnumerable<Feed> Feeds { get; set; }
    }
}
