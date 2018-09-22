using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL.Models
{
    public class Collection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Source> Sources { get; set; }
    }
}
