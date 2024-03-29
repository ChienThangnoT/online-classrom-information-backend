﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class AddCategoryModel
    {
        [Required(ErrorMessage = "CategoryName is required!")]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "CategoryDescription is required!")]
        public string CategoryDescription { get; set; }
    }
}
