﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CarRentingSystem.Areas.Admin.Constants.AdminConstants;

namespace CarRentingSystem.Areas.Admin.Controllers
{
  
 
    public class AdminController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
