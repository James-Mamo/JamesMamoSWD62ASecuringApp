using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SecuringApplication.Controllers
{
    public class RolesManagementController : Controller
    {
        
        public IActionResult AllocateTeacherRoles()
        {
            
            return View();
        }
    }
}
