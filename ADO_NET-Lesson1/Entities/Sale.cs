using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ADO_NET_Lesson1.Entities
{
    public class Sale
    {
        public Guid Id { get; set; }
        public DateTime SaleDt { get; set; }
        public Guid Product_Id { get; set; }
        public int Quantity { get; set; }
        public Guid Manager_Id { get; set; }
        public DateTime? DeleteDt { get; set; }
        public Sale()
        {
            Id = Guid.NewGuid();
            SaleDt = DateTime.Now;
            Quantity = 1;
        }
        public Sale(SqlDataReader reader)
        {
            Id = reader.GetGuid("Id");
            SaleDt = reader.GetDateTime("SaleDt");
            Product_Id = reader.GetGuid("Product_Id");
            Quantity = reader.GetInt32("Quantity");
            Manager_Id = reader.GetGuid("Manager_Id");
            DeleteDt = reader.GetValue("DeleteDt") == DBNull.Value ? null : reader.GetDateTime("DeleteDt");
        }
    }
}