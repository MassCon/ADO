using ADO_NET_Lesson1.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_NET_Lesson1.EFCore
{
    public class EFContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
               @"Data Source=ANDREW-LAPTOP;Initial Catalog=ADO-201;Integrated Security=True;");
        }
    }
}