﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleRESTful.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Filepath { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ProjectType { get; set; }
    }
}
