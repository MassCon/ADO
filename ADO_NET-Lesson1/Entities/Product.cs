using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace ADO_NET_Lesson1.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public DateTime? DeleteDt { get; set; }
        public Product()
        {
            Id = Guid.NewGuid();
            Name = null!;
            DeleteDt = null;
        }
        public Product(SqlDataReader reader)
        {
            Id = reader.GetGuid("Id");
            Name = reader.GetString("Name");
            Price = reader.GetDouble("Price");
            DeleteDt = reader.GetValue("DeleteDt") == DBNull.Value ? null : reader.GetDateTime("DeleteDt");
        }
    }
}
