using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RazorArchitectureDemo.Enums;

namespace RazorArchitectureDemo.Models
{
    public class Student
    {
        //Id, Name, Description, Price, ImageName
        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "E-mail is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Profession is required")]
        public Profession Profession { get; set; }
        
        public string ImageFileName { get; set; }

        public IFormFile ImageFile { get; set; }

        public Student() { }
        public Student(int id, string name, string email, Profession profession, string imageFileName)
        {
            Id = id;
            Name = name;
            Email = email;
            Profession = profession;
            ImageFileName = imageFileName;
        }

        public override string ToString()
        {
            return $"Id: {Id}, {Name}, {Profession.IT}";
        }
    }
}
