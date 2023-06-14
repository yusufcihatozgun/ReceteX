using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using ReceteX.Repository.Shared.Concrete;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReceteX.Web.Controllers
{
    
    public class UserController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //[Route("User/Index/{customerId?}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            //Guid deneme = id;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult GetAll([FromQuery(Name = "customerId")] string? customerId)
        {
            if (customerId == null)
            {
                return Json(new { Data = unitOfWork.Users.GetAll().Include(u => u.Customer) });
            }
            else
            {
            //    var ceysın= Json(new { Data = unitOfWork.Users.GetAll(c => c.Id == Guid.Parse(customerId)).Include(u => u.Customer) });
                return Json(new { Data = unitOfWork.Users.GetAll(c=>c.CustomerId == Guid.Parse(customerId)).Include(u => u.Customer)});
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult GetById(Guid Id)
        {
            AppUser usr = unitOfWork.Users.GetFirstOrDefault(u=> u.Id == Id);
            return Json(usr);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(AppUser appUser)
        {
            unitOfWork.Users.Add(appUser);
            unitOfWork.Save();
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Update(AppUser appUser)
        {
            AppUser asil = unitOfWork.Users.GetFirstOrDefault(u => u.Id == appUser.Id);

            appUser.Id = asil.Id;
            appUser.Id = asil.Id;
            appUser.Id = asil.Id;
            appUser.Id = asil.Id;
            appUser.Id = asil.Id;
            appUser.Id = asil.Id;
            appUser.Id = asil.Id;




            unitOfWork.Users.Update(appUser);
            unitOfWork.Save();
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            unitOfWork.Users.Remove(id);
            unitOfWork.Save();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UserChangeState(Guid id)
        {
            AppUser asil = unitOfWork.Users.GetById(id);
            if (asil.isActive)
            {
                asil.isActive = false;
                unitOfWork.Users.Update(asil);
                unitOfWork.Save();
                return Ok();
            }
            else
            {
                asil.isActive = true;
                unitOfWork.Users.Update(asil);
                unitOfWork.Save();
                return Ok();
            }
        }
       
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(AppUser user)
        { 
            if (user != null) 
            {
                AppUser usr = unitOfWork.Users.GetFirstOrDefault(u => u.Email == user.Email && u.Password == user.Password && u.isActive && !u.IsDeleted);
                if (usr != null)
                {
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, usr.Name + " " + usr.Surname));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, usr.Id.ToString()));
                    if (usr.isAdmin)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "User"));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                     
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties { IsPersistent = usr.isRememberMe});
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
    }
}
