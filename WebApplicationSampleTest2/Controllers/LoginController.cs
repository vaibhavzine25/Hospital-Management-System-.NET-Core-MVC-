using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationSampleTest2.Models;
using WebApplicationSampleTest2.Models.Classs;
using WebApplicationSampleTest2.Repository;

namespace WebApplicationSampleTest2.Controllers
{
    public class LoginController : Controller
    {
        private readonly Ipatient _patientRepo;

        public LoginController(Ipatient patientRepo)
        {
            _patientRepo = patientRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult Signup()
        {
            return View();
        }

        public IActionResult LoginClick([Bind] Users _users)
        {

            ViewBag.Username = _users.username;

            // Check if username and password entered
            if (!string.IsNullOrEmpty(_users.username) && !string.IsNullOrEmpty(_users.password))
            {
                // Repository call to validate user
                var patient = _patientRepo.Login(_users.username, _users.password);

                if (patient != null)
                {
                    // Valid user → redirect to dashboard
                    return RedirectToAction("Index", "DashBord");
                }
                else
                {
                    // Invalid user → show login page with error message
                    ViewBag.Error = "Invalid Username or Password";
                    return View("Index"); // explicitly return Login page
                }
            }
            else
            {
                // Empty fields → show login page with message
                ViewBag.Error = "Please enter both Username and Password";
                return View("Index"); // explicitly return Login page
            }
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                ViewBag.Error = "Please enter email";
                return View(model);
            }

            bool emailExists = _patientRepo.CheckEmail(model.Email);

            if (!emailExists)
            {
                ViewBag.Error = "Email not found";
                return View(model);
            }

            // ✅ Email verified here
            ViewBag.ShowModal = true;
            return View(model);
        }

        [HttpPost]
        public IActionResult ResetPassword(ForgotPasswordModel model)
        {
            if (model.NewPassword != model.ConfirmPassword)
            {
                ViewBag.Error = "Password mismatch";
                ViewBag.ShowModal = true;
                return View("ForgotPassword", model);
            }

            _patientRepo.UpdatePassword(model.Email, model.NewPassword);
            return RedirectToAction("ForgotPassword");
        }

        //[HttpPost]
        //public IActionResult Signup(Patient patient)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        TempData["Error"] = "Please fill all required fields";
        //        return View(patient);
        //    }

        //    bool result = _patientRepo.RegisterPatient(patient);

        //    if (result)
        //    {
        //        TempData["Success"] = "Registration successful";
        //        return RedirectToAction("Signup", "Login");
        //    }

        //    TempData["Error"] = "Registration failed";
        //    return View(patient);
        //}

        [HttpPost]
        public IActionResult Signup(Patient model)
        {
            // 4️⃣ Server-side basic validation
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill all required fields";
                return View(model);
            }

            // 5️⃣ Call Repository method
            bool result = _patientRepo.SignupPatient(model, out string message);

            // 6️⃣ Success / Failure handling
            if (result)
            {
                TempData["Success"] = message; // "Patient registered successfully"
                return RedirectToAction("Signup", "Login"); // back to login page
            }
            else
            {
                TempData["Error"] = message; // Email/Mobile already exists
                return View(model);
            }
        }


    }
}
