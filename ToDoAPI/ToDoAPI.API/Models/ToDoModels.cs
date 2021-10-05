using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ToDoAPI.DATA.EF;

namespace ToDoAPI.API.Models
{
    public class TodoViewModel
    {
        [Key]
        public int TodoId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "** CANNOT EXCEED 50 CHARACTERS **")]
        public string Action { get; set; }

        [Required]
        public bool Done { get; set; }


        public Nullable<int> CategoryId { get; set; }
    }

    public class CategoryViewModel
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "** CANNOT EXCEED 50 CHARACTERS **")]
        public string Name { get; set; }

        [StringLength(2500, ErrorMessage = "** CANNOT EXCEED 2500 CHARACTERS **")]
        public string Description { get; set; }
    }
}