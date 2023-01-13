using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lab4.Data;
using lab4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab4.Controllers
{
    public class HomeController : Controller
    {
        private readonly SchoolCommunityContext _context;

        public HomeController(SchoolCommunityContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //throw new Exception();
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
