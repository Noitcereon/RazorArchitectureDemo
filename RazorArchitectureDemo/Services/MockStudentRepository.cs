using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorArchitectureDemo.Enums;
using RazorArchitectureDemo.Interfaces;
using RazorArchitectureDemo.Models;

namespace RazorArchitectureDemo.Services
{
    public class MockStudentRepository : IStudentRepository
    {
        private static Dictionary<int, Student> _students;

        public MockStudentRepository()
        {
            _students = new Dictionary<int, Student>
            {
                { 1, new Student(1, "White Thomas", "tbawhite@live.dk", Profession.IT, "white.jpg") },
                { 2, new Student(2, "Red Thomas", "tbared@live.dk", Profession.IT, "red.jpg") },
                { 3, new Student(3, "Black Thomas", "tbablack@live.dk", Profession.Engineering, "black.jpg") },
                { 4, new Student(4, "Green Thomas", "tbagreen@live.dk", Profession.Administration, "green.jpg") },
                { 5, new Student(5, "Blue Thomas", "tbablue@live.dk", Profession.Business, "blue.jpg") },
            };
        }

        public bool Add(Student student)
        {
            student.Id = NextId();
            return _students.TryAdd(student.Id, student);
        }

        public Dictionary<int, Student> Get()
        {
            return _students;
        }

        public Student Get(int id)
        {
            _students.TryGetValue(id, out Student student);

            return student;
        }
        /// <summary>
        /// Filters the dictionary based on the Student name values (case insensitive)
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns>A filtered <c>Dictionary</c> or <c>null</c> if the searchCriteria is null or white space.</returns>
        public Dictionary<int, Student> GetWithFilter(string searchCriteria)
        {
            if (String.IsNullOrWhiteSpace(searchCriteria)) return _students;

            searchCriteria = searchCriteria.ToLowerInvariant();
            Dictionary<int, Student> output = new Dictionary<int, Student>();
            IEnumerable<Student> filteredStudents = _students.Values.Where(x => x.Name.ToLower().Contains(searchCriteria));

            foreach (var student in filteredStudents)
            {
                output.TryAdd(student.Id, student);
            }
            return output;
        }

        public void Update(int id, Student newStudent)
        {
            if (StudentExists(id))
            {
                _students[id] = newStudent;
            }
        }

        public bool Delete(int id)
        {
            return _students.Remove(id);
        }

        public bool StudentExists(int id)
        {
            return _students.ContainsKey(id);
        }

        private static int NextId()
        {
            int nextId = 1;
            if (_students.Values.Count > 0)
            {
                nextId = _students.Values.Max(s => s.Id) + 1;
            }
            return nextId;
        }
    }
}
