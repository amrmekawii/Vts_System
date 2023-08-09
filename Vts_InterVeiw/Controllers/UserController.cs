using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vts.BL;
using Vts.DAL;

namespace Vts_InterVeiw.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserRepo _userRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManagers;

        public UserController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManagers , IUserRepo userRepo)
        {
            _userManager = userManager;
            _signInManagers = signInManagers;
            _userRepo = userRepo;
        }
        [Authorize]
        public IActionResult Index()
        {


            ViewBag.IP = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4()?.ToString();

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }


        [HttpPost]
        public IActionResult Login(LoginDto credentials)
        {
            var user = _userManager.FindByNameAsync(credentials.UserName).Result;
          // User? check = _userRepo.GetUserById(user.Id);
           // if (check.IpAddress == null)
           //check.IpAddress = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4()?.ToString();
            
            if (user == null)
            {
                ModelState.AddModelError("", "Credentials are not correct");
                return View();
            }

            var isAuthenticated = _userManager.CheckPasswordAsync(user, credentials.Password).Result;
            if (isAuthenticated == false)
            {
                ModelState.AddModelError("", "Credentials are not correct");
                return View();
            }

            var claims = _userManager.GetClaimsAsync(user).Result;
          
            _signInManagers.SignInWithClaimsAsync(user, true, claims).Wait();
            TempData["message"] = "Succes LOGIN";


            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }


        [HttpPost]
        public IActionResult Register(RegisterDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
            };

            //.Result to overcome async
            var creationResult = _userManager.CreateAsync(user, registerDto.Password).Result;
            if (!creationResult.Succeeded)
            {
                foreach (var item in creationResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }

            var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id),
            new ("Role", "User"),
        };

            var addingClaimsResult = _userManager.AddClaimsAsync(user, claims).Result;
            if (!addingClaimsResult.Succeeded)
            {
                foreach (var item in addingClaimsResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }


            return RedirectToAction("Login");

        }

        public IActionResult Logout()
        {
            _signInManagers.SignOutAsync().Wait();
            return RedirectToAction("Index", "Home");
        }

   

            public IActionResult ShowDialog() {

            return View();
        }
    }
}
