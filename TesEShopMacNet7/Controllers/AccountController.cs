using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TestEShopMacNet7.Models.Account;
using TestEShopMacNet7.Services;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace TestEShopMacNet7.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ExtendedUser> userManager;
        private SignInManager<ExtendedUser> signInManager;
        private EmailSenderService emailSenderService;

        public AccountController(UserManager<ExtendedUser> userManager, SignInManager<ExtendedUser> signInManager, EmailSenderService emailSenderService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSenderService = emailSenderService;
        }

        // GET: /<controller>/
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel? model, string? returnUrl)
        {
            returnUrl = returnUrl ?? "/";
            if (model is not null && ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var result = await signInManager.PasswordSignInAsync(user, model.Password, model.StaySignedIn, false);
                    if (result.Succeeded)
                        return LocalRedirect(returnUrl);
                    ModelState.AddModelError("Password", "Nesprávné heslo");
                    return View();
                }
                ModelState.AddModelError("Email", "Uživatelský účet neexistuje");
                return View();
            }
            return View();
        }

        // GET: /<controller>/
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel? model, string? returnUrl)
        {
            returnUrl = returnUrl ?? "/";
            if (model is not null && ModelState.IsValid)
            {
                var user = new ExtendedUser { UserName = model.Email, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                       "ConfirmEmail", "Account",
                       new { userId = user.Id, token = confirmationToken },
                       protocol: Request.Scheme);

                    emailSenderService.SendTo(callbackUrl!, "Prosim potvrdit", user.Email);

                    return LocalRedirect(returnUrl);
                } else
                {
                    ModelState.AddModelError("Email", "Zkuste to znovu");
                }
            }
            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                await userManager.ConfirmEmailAsync(user, token);

                if (user.EmailConfirmed)
                    return View("ConfirmEmail", "EmailConfirmed");
            }
            return View("ConfirmEmail", "Something went wrong, try again later");
        }

        public IActionResult PasswordToReset(string userId, string token)
        {
            var resetPasswordViewModel = new ResetPasswordViewModel { userId = userId, resetToken = token };
            return View(resetPasswordViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PasswordToReset(string userId, string token, string firstPassword, string secondPassword)
        {
            if (firstPassword != secondPassword)
            {
                ModelState.AddModelError("firstPassword", "Hesla nejsou stejna");
                return View();

            }

            var user = await userManager.FindByIdAsync(userId ?? "");
            string oldId = user?.Id ?? "";
            if (ModelState.IsValid && firstPassword == secondPassword && user is not null)
            {
                var resultReset = await userManager.ResetPasswordAsync(user, token, secondPassword);
                if (resultReset.Succeeded)
                {
                    var resultSignIn = await signInManager.PasswordSignInAsync(user, firstPassword, true, false);
                    if (resultSignIn.Succeeded)
                        return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("secondPassword", "Hesla se neshoduji");
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string userEmail)
        {
            var user = await userManager.FindByEmailAsync(userEmail ?? "");
            if (ModelState.IsValid && user is not null)
            {
                var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action(
                   "PasswordToReset", "Account",
                   new { userId = user.Id, token = resetToken },
                   protocol: Request.Scheme);

                emailSenderService.SendTo(callbackUrl!, "Resetování hesla", user.Email!);
                return View("PasswordReseted", "Odesláno");

            }
            ModelState.AddModelError("userEmail", "Email není regisgtrován");
            return View();
        }

        // GET: /<controller>/
        public async Task<IActionResult> AccountSettings(string? Id)
        {
            if (Id is not null)
            {
                var user = await userManager.FindByIdAsync(Id);
                return View(user);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewUserName(string newUserName)
        {
            var user = (await userManager.FindByNameAsync(User?.Identity?.Name ?? "") ?? await userManager.FindByEmailAsync(User?.Identity?.Name ?? ""));
            string oldId = user?.Id ?? "";
            if (ModelState.IsValid && newUserName is not null && user is not null)
            {
                var result = await userManager.SetUserNameAsync(user, newUserName);
                if (result.Succeeded)
                    return RedirectToAction("AccountSettings", "Account", new { Id = user.Id });

                // Generate confirmation email token
            }
            return RedirectToAction("AccountSettings", "Account", new { Id = oldId });
        }

        [HttpPost]
        public async Task<IActionResult> NewEmail(string newEmail)
        {
            var user = (await userManager.FindByNameAsync(User?.Identity?.Name ?? "") ?? await userManager.FindByEmailAsync(User?.Identity?.Name ?? ""));
            string oldId = user?.Id ?? "";
            if (ModelState.IsValid && newEmail is not null && user is not null)
            {
                var result = await userManager.SetEmailAsync(user, newEmail);
                if (result.Succeeded)
                    return RedirectToAction("AccountSettings", "Account", new { Id = user.Id });
            }
            return RedirectToAction("AccountSettings", "Account", new { Id = oldId });
        }

        [HttpPost]
        public async Task<IActionResult> NewPhoneNumber(string newPhoneNumber)
        {
            var user = (await userManager.FindByNameAsync(User?.Identity?.Name ?? "") ?? await userManager.FindByEmailAsync(User?.Identity?.Name ?? ""));
            string oldId = user?.Id ?? "";
            if (ModelState.IsValid && newPhoneNumber is not null && user is not null)
            {
                var result = await userManager.SetPhoneNumberAsync(user, newPhoneNumber);
                if (result.Succeeded)
                    return RedirectToAction("AccountSettings", "Account", new { Id = user.Id });
            }
            return RedirectToAction("AccountSettings", "Account", new { Id = oldId });
        }

        [HttpPost]
        public async Task<IActionResult> NewPassword(string oldPassword, string newPassword)
        {
            var user = await userManager.FindByNameAsync(User?.Identity?.Name ?? "");
            string oldId = user?.Id ?? "";
            if (ModelState.IsValid && newPassword is not null && user is not null && oldPassword is not null)
            {
                if (await userManager.CheckPasswordAsync(user, oldPassword))
                {
                    var result = await userManager.ResetPasswordAsync(user, await userManager.GeneratePasswordResetTokenAsync(user), newPassword);
                    if (result.Succeeded)
                        return RedirectToAction("AccountSettings", "Account", new { Id = user.Id });
                }
            }
            return RedirectToAction("AccountSettings", "Account", new { Id = oldId });
        }

        [HttpPost]
        public async Task<IActionResult> NewBillingInfo(string Street, string City, int PostalCode)
        {
            var user = await userManager.FindByNameAsync(User?.Identity?.Name ?? "");
            string oldId = user?.Id ?? "";
            if (ModelState.IsValid && user is not null && Street is not null && City is not null)
            {
                user.Street = Street;
                user.City = City;
                user.PostalCode = PostalCode;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("AccountSettings", "Account", new { Id = user.Id });
            }
            return RedirectToAction("AccountSettings", "Account", new { Id = oldId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string Name)
        {
            if (ModelState.IsValid && Name is not null)
            {
                var user = await userManager.FindByEmailAsync(Name) ?? await userManager.FindByNameAsync(Name);
                if (user is not null)
                {
                    await signInManager.SignOutAsync();
                    await userManager.DeleteAsync(user);
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}

