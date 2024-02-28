using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using API.Models;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;
namespace MVC.Controllers
{
    public class MvcAdminController : Controller
    {
        private readonly ILogger<MvcAdminController> _logger;
        private readonly IAdminRepositories _adminrepo;

        public MvcAdminController(ILogger<MvcAdminController> logger, IAdminRepositories adminrepo)
        {
            _logger = logger;
            _adminrepo = adminrepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.IsAuthenticated = true;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}