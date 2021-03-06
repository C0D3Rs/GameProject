﻿using GameProject.Enums;
using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameProject.Areas.Admin.ViewModels
{
    public class SetImageViewModel
    {
        public List<Image> Images { get; set; }
        public int Id { get; set; }
        public ImageCategory Category { get; set; }
    }
}
