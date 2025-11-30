using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_PRJ_F.Data;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Models;
using MVC_PRJ_F.ModelView;
using MVC_PRJ_F.Utilites;

namespace MVC_PRJ_F.Controllers;


public class AccountController:Controller
{
    // public static Dictionary<string, List<string>> CountriesWithCities =
    //     new Dictionary<string, List<string>>()
    //     {
    //         { "Egypt", new List<string>
    //             {
    //                 "Cairo",
    //                 "Alexandria",
    //                 "Giza",
    //                 "Aswan",
    //                 "Asyut",
    //                 "Beheira",
    //                 "Beni Suef",
    //                 "Dakahlia",
    //                 "Damietta",
    //                 "Faiyum",
    //                 "Gharbia",
    //                 "Ismailia",
    //                 "Kafr El Sheikh",
    //                 "Luxor",
    //                 "Matrouh",
    //                 "Minya",
    //                 "Monufia",
    //                 "New Valley",
    //                 "North Sinai",
    //                 "Port Said",
    //                 "Qalyubia",
    //                 "Qena",
    //                 "Red Sea",
    //                 "Sharqia",
    //                 "Sohag",
    //                 "South Sinai",
    //                 "Suez"
    //             }
    //         },
    //         { "Saudi Arabia", new List<string>
    //             {
    //                 "Riyadh",
    //                 "Makkah",
    //                 "Medina",
    //                 "Eastern Province",
    //                 "Qassim",
    //                 "Ha'il",
    //                 "Tabuk",
    //                 "Northern Borders",
    //                 "Jazan",
    //                 "Najran",
    //                 "Asir",
    //                 "Al Bahah",
    //                 "Al Jawf"
    //             }
    //         }
    //     };
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IRepository<Otp> _iRepositories;

    public AccountController(UserManager<ApplicationUser> userManager, IEmailSender emailSender,
        SignInManager<ApplicationUser> signInManager, IRepository<Otp> repositories)
    {
        _userManager = userManager;
        _emailSender = emailSender;
        _signInManager = signInManager;
        _iRepositories = repositories;
    }
    [HttpGet]
    public IActionResult Login()
    {
        var vm = new LoginVM();
        if (TempData["Email"] != null)
        {
            ViewBag.Email = TempData["Email"];
        }
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM vm)
    {
        if (!ModelState.IsValid)
        {
            TempData ["Error"]  = "Username or password is incorrect";
            return View(vm);
        }
        var log =await _userManager.FindByEmailAsync(vm.UserName);
        if (log == null)
        {
            TempData ["Error"]  = "Username or password is incorrect";
            return View(vm);
        }
        var userName = log.UserName ?? log.Email ?? vm.UserName;
        var Don = await _signInManager.PasswordSignInAsync(userName, vm.Password, isPersistent: vm.RememberMe, lockoutOnFailure: true);

        if (!Don.Succeeded)
        {
            if (Don.IsLockedOut)
            {
                // إلغاء قفل الحساب تلقائياً إذا انتهت مدة القفل
                if (log.LockoutEnd.HasValue && log.LockoutEnd.Value <= DateTimeOffset.UtcNow)
                {
                    await _userManager.SetLockoutEndDateAsync(log, null);
                    await _userManager.ResetAccessFailedCountAsync(log);
                    // إعادة المحاولة
                    Don = await _signInManager.PasswordSignInAsync(userName, vm.Password, isPersistent: vm.RememberMe, lockoutOnFailure: true);
                    if (Don.Succeeded)
                    {
                        TempData["Success"] = "Login Successful";
                        return RedirectToAction("Index", "Home");
                    }
                }
                
                var lockoutEnd = log.LockoutEnd?.LocalDateTime;
                if (lockoutEnd.HasValue && lockoutEnd.Value > DateTime.Now)
                {
                    var remainingTime = lockoutEnd.Value - DateTime.Now;
                    TempData["Error"] = $"Your account is locked. Please try again after {remainingTime.Minutes} minutes and {remainingTime.Seconds} seconds.";
                }
                else
                {
                    TempData["Error"] = "Your account is locked. Please try again later.";
                }
                ViewBag.Email = vm.UserName; // حفظ البريد لعرضه في نموذج إلغاء القفل
            }
            else if (Don.IsNotAllowed)
            {
                TempData["Error"] = "Please confirm your email first.";
            }
            else
            {
                TempData["Error"] = "Username or password is incorrect";
            }
            return View(vm);
        }
        TempData ["Success"]  = "Login Successful";
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public JsonResult GetCities(string country)
    {
        if (Dictionary.CountriesWithCities.ContainsKey(country))
            return Json(Dictionary.CountriesWithCities[country]);

        return Json(new List<string>());
    }

    
    
    [HttpGet]
    public IActionResult Register(){
        var vm = new RegisterVM
        {
            CountryList = Dictionary.CountriesWithCities.Keys.ToList(),  // غيرت من Countries
            CityList = new List<string>()  // غيرت من Cities
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        var user = vm.Adapt<ApplicationUser>();
        user.UserName = vm.Email;
        var Don = await _userManager.CreateAsync(user,vm.Password);
        
        if (Don.Succeeded)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(Confirm),"Account",new{token=token,id=user.Id},Request.Scheme);
            var htmlMessage = $@"
                        <html>
                            <body style='margin: 0; padding: 0; background-color: #f3f6fb; font-family: Arial, sans-serif;'>
                                <table width='100%' cellspacing='0' cellpadding='0' style='background-color: #f3f6fb; padding: 40px 0;'>
                                    <tr>
                                        <td align='center'>
                                            <!-- Main Card -->
                                            <table width='600' cellspacing='0' cellpadding='0' 
                                                   style='background: #ffffff; border-radius: 10px; padding: 40px; box-shadow: 0px 4px 15px rgba(0,0,0,0.1);'>
                                                <tr>
                                                    <td align='center'>
                                                        <h2 style='color: #1e3a8a; margin-bottom: 10px; font-size: 26px;'>
                                                            Welcome to Our Website!
                                                        </h2>
                                                        <p style='color: #555; font-size: 16px; margin-bottom: 30px;'>
                                                            Thanks for joining us! Please confirm your email address by clicking the button below.
                                                        </p>
                                                        
                                                        <!-- Button -->
                                                        <a href='{link}' style='display: inline-block; padding: 14px 28px; background: linear-gradient(90deg, #2563eb, #1e40af); color: #ffffff; text-decoration: none; font-size: 16px; border-radius: 6px; font-weight: bold; box-shadow: 0px 3px 8px rgba(37, 99, 235, 0.4);'>
                                                            Confirm Email
                                                        </a>
                                                        
                                                        <p style='color: #777; margin-top: 35px; font-size: 14px; line-height: 1.6;'>
                                                            If you did not request this email, simply ignore it.<br>
                                                            This link will expire shortly for your security.
                                                        </p>
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                            <!-- Footer -->
                                            <p style='color: #888; margin-top: 25px; font-size: 12px;'>
                                                © 2025 Cinema — All Rights Reserved
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                            </body>
                        </html>";

            await _emailSender.SendEmailAsync(vm.Email, "Confirm your email",htmlMessage);
            TempData["Success"] = "Please Confirm Your Email";
            return RedirectToAction(nameof(Mail));
            
        }
        TempData ["Error"]  = "User creation failed";
        return View(vm);
        
    }

    [HttpGet]
    public IActionResult Mail()
    {
        return View("confirm-mail");
    }
    
    
    
    [HttpGet]
    public async Task<IActionResult> Confirm(string token, string id)
    {
        
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();
        var result = await _userManager.ConfirmEmailAsync(user, token);

        TempData  ["Success"] = "Email Confirmed";
        return RedirectToAction(nameof(Login));
        
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View("forgot-password");
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string Email)
    {
        var user = await _userManager.FindByEmailAsync(Email);
        
        if (user != null)
        {
            
            string otpCode = new Random().Next(100000, 999999).ToString();
            var otp = new Otp(Email, otpCode);
            await _iRepositories.CreatAsync(otp);
            
            
            
            
            // var link = Url.Action(nameof(Confirm),"Account",new{token=token,id=user.Id},Request.Scheme);
             var htmlMessage = $@"
            <html>
                <body style='margin: 0; padding: 0; background-color: #f3f6fb; font-family: Arial, sans-serif;'>
                    <table width='100%' cellspacing='0' cellpadding='0' style='background-color: #f3f6fb; padding: 40px 0;'>
                        <tr>
                            <td align='center'>
                                <table width='600' cellspacing='0' cellpadding='0' 
                                       style='background: #ffffff; border-radius: 10px; padding: 40px; box-shadow: 0px 4px 15px rgba(0,0,0,0.1);'>
                                    <tr>
                                        <td align='center'>
                                            <h2 style='color: #1e3a8a; margin-bottom: 10px; font-size: 26px;'>
                                                Reset Your Password
                                            </h2>
                                            <p style='color: #555; font-size: 16px; margin-bottom: 30px;'>
                                                We received a request to reset your password.<br>
                                                Please use the OTP code below:
                                            </p>

                                            <div style='font-size: 32px; font-weight: bold; color: #1e40af; letter-spacing: 3px; margin-bottom: 30px;'>
                                                {otpCode}
                                            </div>

                                            <p style='color: #777; margin-top: 20px; font-size: 14px; line-height: 1.6;'>
                                                This OTP is valid for a short time only.<br>
                                                If you did not request a password reset, please ignore this email.
                                            </p>
                                        </td>
                                    </tr>
                                </table>

                                <p style='color: #888; margin-top: 25px; font-size: 12px;'>
                                    © 2025 Cinema — All Rights Reserved
                                </p>
                            </td>
                        </tr>
                    </table>
                </body>
            </html>";

             TempData["Success"] = "Please Write Otp Password";
            await _emailSender.SendEmailAsync(Email, "Reset Password",htmlMessage);
            TempData["Email"]= Email;
            return View("confirm-otp");
            
        }
        TempData ["Error"]  = "User creation failed";
        return RedirectToAction(nameof(ForgotPassword));
        
    }

    [HttpPost]
    public async Task<IActionResult> VerifyOTP(string OtpCode,string Email)
    {
        var otp = await _iRepositories.GetOtpAsync(Email, OtpCode);
        if (otp == null)
        {
            return View("confirm-otp", otp);
        }
        bool Don = await _iRepositories.IsOtpExpiredAsync(Email, OtpCode);
        if (Don)
        {
            otp.IsUsed = true;
            await _iRepositories.UpdateAsync(otp);
            TempData["Email"]= Email;
            var vm = new ResetPasswordVM();
            return View("reset-password",vm);
        }
        TempData["Error"]= "Otp code is invalid";
        return View("confirm-otp");
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user == null)
            {
                return View("reset-password");
            }
            var dummy = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user,dummy, vm.Password);
            return View(nameof(Login));
        }
        return View("reset-password");
        
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return View("logout");
    }
    
    // إلغاء قفل الحساب يدوياً (للمسؤولين أو للاختبار)
    [HttpPost]
    public async Task<IActionResult> UnlockAccount(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            TempData["Error"] = "Email is required";
            return RedirectToAction(nameof(Login));
        }
        
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            TempData["Error"] = "User not found";
            return RedirectToAction(nameof(Login));
        }
        
        await _userManager.SetLockoutEndDateAsync(user, null);
        await _userManager.ResetAccessFailedCountAsync(user);
        
        TempData["Success"] = "Account unlocked successfully. You can now login.";
        return RedirectToAction(nameof(Login));
    }
    
    
    
    
    
    
}