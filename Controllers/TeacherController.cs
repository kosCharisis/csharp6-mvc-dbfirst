using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.DTO;
using SchoolApp.Models;
using SchoolApp.Services;

namespace SchoolApp.Controllers
{
    public class TeacherController : Controller
    {
        private readonly IApplicationService _applicationService;
        public List<Error> ErrorArray { get; set; } = new();

        public TeacherController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(TeacherSignUpDTO teacherSignUpDTO)
        {
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState.Values)
                {
                    foreach (var error in entry.Errors)
                    {
                        ErrorArray.Add(new Error("", error.ErrorMessage, ""));
                    }
                }
                return View(teacherSignUpDTO);
            }

            try
            {
                await _applicationService.TeacherService.SignUpUserAsync(teacherSignUpDTO);
                return RedirectToAction("Login", "User");
            }
            catch (Exception ex)
            {
                ErrorArray.Add(new Error("", ex.Message, ""));
                ViewData["ErrorArray"] = ErrorArray;
                return View();
            }
        }
    }
}
