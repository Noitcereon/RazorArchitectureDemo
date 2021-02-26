using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorArchitectureDemo.Models;

namespace RazorArchitectureDemo.Interfaces
{
    public interface IStudentRepository
    {
        // Create, Read, Update, Delete

        bool Add(Student student);

        Dictionary<int, Student> Get();

        Student Get(int id);

        Dictionary<int, Student> GetWithFilter(string searchCriteria);

        void Update(int id, Student newStudent);

        bool Delete(int id);
    }
}
