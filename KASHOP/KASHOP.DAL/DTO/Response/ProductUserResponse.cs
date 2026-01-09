using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.DTO.Response
{
    public class ProductUserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }
        public double Rate { get; set; }
        public string MainImage { get; set; }
    }
}
