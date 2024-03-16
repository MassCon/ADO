using ADO_NET_Lesson1.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_NET_Lesson1.Entities
{
    public class Manager
    {
        public Guid Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Secname { get; set; }
        public Guid IdMainDep { get; set; }
        public Guid? IdSecDep { get; set; }
        public Guid? IdChief { get; set; }
        public DateTime? DeleteDt { get; set; }

        public Manager()
        {
            Id = Guid.NewGuid();
            Surname = null!;
            Name = null!;
            Secname = null!;
        }
        public Manager(DbDataReader reader)
        {
            Id = reader.GetGuid("Id");
            Surname = reader.GetString("Surname");
            Name = reader.GetString("Name");
            Secname = reader.GetString("Secname");
            IdMainDep = reader.GetGuid("Id_main_dep"); IdSecDep = reader.GetValue("Id_sec_dep") == DBNull.Value ? null : reader.GetGuid("Id_sec_dep");
            IdChief = reader.IsDBNull("Id_chief") ? null : reader.GetGuid("Id_chief");
            DeleteDt = reader.IsDBNull("DeleteDt") ? null : reader.GetDateTime("DeleteDt");
        }
        public override string ToString()
        {
            return $"{Id.ToString()[..4]} {Surname} {Name} {Secname} {IdMainDep.ToString()[..4]}";
        }

        //////////////////////////////////// Navigation Properties ////////////////////////////////////
        internal DataContext? dataContext;
        public Department? MainDep
        {
            get
            {
                return dataContext?
                    .Departments
                    .GetAll()
                    .Find(d => d.Id == this.IdMainDep);
            }
        }
        public Department? SecDep
        {
            get
            {
                if (IdSecDep is null) return null;
                else
                    return dataContext?
                        .Departments
                        .GetAll()
                        .Find(d => d.Id == this.IdSecDep);
            }
        }
        public Manager? Chief
        {
            get
            {
                if (IdChief is null) return null;
                else
                    return dataContext?
                        .Managers
                        .GetAll()
                        .Find(m => m.Id == this.IdChief);
            }
        }
        public List<Manager>? Subordinates
        {
            get => dataContext?
                .Managers
                .GetAll()
                .Where(m => m.IdChief == this.Id)
                .ToList();
        }
    }
}