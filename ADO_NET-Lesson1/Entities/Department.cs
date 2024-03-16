using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ADO_NET_Lesson1.Entities
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? DeleteDt { get; set; }
        public Department()
        {
            Id = Guid.NewGuid();
            Name = null!;
        }
        public Department(SqlDataReader reader)
        {
            Id = reader.GetGuid("Id");
            Name = reader.GetString("Name");
            DeleteDt = reader.GetValue("DeleteDt") == DBNull.Value ? null : reader.GetDateTime("DeleteDt");
        }
        public override string ToString()
        {
            return $"{Id.ToString()[..4]} {Name}";
        }
    }
}