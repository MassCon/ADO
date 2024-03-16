using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_NET_Lesson1.EFCore
{
    public class Sale
    {
        public Guid Id { get; set; }
        public DateTime SaleDt { get; set; }
        public Guid Product_Id { get; set; }
        public int Quantity { get; set; }
        public Guid Manager_Id { get; set; }
        public DateTime? DeleteDt { get; set; }
    }
}