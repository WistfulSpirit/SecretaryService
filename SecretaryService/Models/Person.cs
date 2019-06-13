﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecretaryService.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }


        public string DataTextFieldLabel
        {
            get
            {
                return string.Format("{0} ({1})", Name, Email);
            }
        }
    }
}