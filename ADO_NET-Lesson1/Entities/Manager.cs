using System;
using System.Collections.Generic;
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
        public override string ToString()
        {
            return $"{Id.ToString()[..4]} {Surname} {Name} {Secname} {IdMainDep.ToString()[..4]}";
        }
    }
}
