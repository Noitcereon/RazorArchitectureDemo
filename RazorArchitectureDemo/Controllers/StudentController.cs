using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RazorArchitectureDemo.Interfaces;
using RazorArchitectureDemo.Models;
using RazorArchitectureDemo.ViewModels;

namespace RazorArchitectureDemo.Controllers
{
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentRepository _studentRepo;
        private readonly IWebHostEnvironment _webHostEnv;

        public StudentController(ILogger<StudentController> logger, IStudentRepository studentRepository, IWebHostEnvironment webHostEnv)
        {
            _studentRepo = studentRepository;
            _logger = logger;
            _webHostEnv = webHostEnv;
        }

        public IActionResult ShowStudents(string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            _logger.Log(LogLevel.Information, "ShowStudents() called");

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSort"] = String.IsNullOrWhiteSpace(sortOrder) ? "desc" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            Dictionary<int, Student> students = _studentRepo.GetWithFilter(searchString);
            StudentVm vm = new StudentVm();
            switch (sortOrder)
            {
                case "desc":
                    students = students.OrderByDescending(s => s.Value.Name).ToDictionary(s => s.Key, s => s.Value);
                    break;
                default:
                    students = students.OrderBy(s => s.Value.Name).ToDictionary(s => s.Key, s => s.Value);
                    break;
            }

            List<Student> tempStudents = new List<Student>(students.Values);
            int pageSize = 3;
            vm.PaginatedStudents = PaginatedList<Student>.Create(tempStudents, pageNumber ?? 1, pageSize);
            return View(vm);
        }

        public IActionResult CreateStudentPage()
        {
            return View();
        }

        public IActionResult CreateStudent(Student student)
        {
            if (!ModelState.IsValid) return View(nameof(CreateStudentPage));

            if (student.ImageFile == null)
            {
                student.ImageFileName = "default-image.jpg";
            }
            else
            {
                string uniqueFileName = $"{Guid.NewGuid()}_{student.ImageFile.FileName}";
                string imageFolder = Path.Combine(_webHostEnv.WebRootPath, "images");
                string filePath = Path.Combine(imageFolder, uniqueFileName);
                using FileStream fs = new FileStream(filePath, FileMode.Create);

                student.ImageFile.CopyTo(fs);
                student.ImageFileName = uniqueFileName;
            }
            _studentRepo.Add(student);
            return RedirectToAction(nameof(ShowStudents));

        }

        public IActionResult EditStudentPage(int id)
        {
            if (StudentIdExists(id))
            {
                Student student = _studentRepo.Get(id);
                return View(student);
            }

            return RedirectToAction(nameof(ShowStudents));
        }

        public IActionResult EditStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(student.ImageFileName))
                {
                    student.ImageFileName = _studentRepo.Get(student.Id).ImageFileName;
                }
                _studentRepo.Update(student.Id, student);
                return RedirectToAction(nameof(ShowStudents));
            }

            return EditStudentPage(student.Id);
        }

        public IActionResult DeleteStudent(int id)
        {
            if (StudentIdExists(id))
            {
                _studentRepo.Delete(id);
            }

            return RedirectToAction(nameof(ShowStudents));
        }

        private bool StudentIdExists(int id)
        {
            Dictionary<int, Student> students = _studentRepo.Get();
            return students.ContainsKey(id);
        }
        
    }
}
