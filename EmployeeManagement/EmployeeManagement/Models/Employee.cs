using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    [Table("employees")] 
    public class Employee
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("patronymic")]
        public string Patronymic { get; set; }

        [Column("birthdate")]
        public DateTime BirthDate { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("department")]
        public string Department { get; set; }

        [Column("aboutme")]
        public string AboutMe { get; set; }
    }
}
