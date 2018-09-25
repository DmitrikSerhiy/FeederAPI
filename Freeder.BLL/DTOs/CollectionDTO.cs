using System;
using System.Collections.Generic;
using System.Text;

namespace Freeder.BLL.DTOs
{
    public class CollectionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<SourceDTO> Sources { get; set; }
    }
}
