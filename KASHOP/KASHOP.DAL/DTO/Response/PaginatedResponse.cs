using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.DTO.Response
{
    public class PaginatedResponse<T>
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public List<T> Data { get; set; }
    }
}
