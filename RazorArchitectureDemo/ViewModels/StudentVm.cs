using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorArchitectureDemo.Models;

namespace RazorArchitectureDemo.ViewModels
{
    public class StudentVm
    {
        public PaginatedList<Student> PaginatedStudents { get; set; }

    }
}
