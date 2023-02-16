using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCDemo.Models
{
    public class DBDataModel
    {
        public class Student
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int UniqueID { get; set; }
            public string? StudentNum { get; set; }
            public string? Name { get; set; }
            public string? Birthday { get; set; }
            [EmailAddress(ErrorMessage ="不符合Email格式")]
            public string? Email { get; set; }
        }

        public class Class
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int UniqueID { get; set; }
            public string? ClassNum { get; set; }
            public string? Name { get; set; }
            public int Credit { get; set; }
            public string? Place { get; set; }
            public string? Teacher { get; set; }
        }

        public class SelectClass
        {
            public string? ClassNum { get; set; }
            public string? StudentNum { get; set; }
        }

        public class SelectClassDetail
        {
            public string? StudentNum { get; set; }
            public string? Name { get; set; }
            public string? ClassNum { get; set; }
            public string? ClassName { get; set; }
        }
    }
}