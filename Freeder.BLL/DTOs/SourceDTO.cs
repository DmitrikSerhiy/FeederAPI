using System;
using System.Collections.Generic;
using System.Text;

namespace Freeder.BLL.DTOs
{
    public class SourceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public IEnumerable<FeedDTO> Feeds { get; set; }
    }
}
