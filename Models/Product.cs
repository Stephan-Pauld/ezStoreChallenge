using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ezStore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int Cost { get; set; }
        public string Description { get; set; }
        public bool is_active { get; set; }
    }
}
