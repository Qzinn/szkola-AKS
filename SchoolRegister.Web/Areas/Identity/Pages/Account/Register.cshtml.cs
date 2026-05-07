// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public List<SelectListItem> Roles { get; set; }

        public List<SelectListItem> Groups { get; set; }

        public List<SelectListItem> Parents { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Role")]
            public string Role { get; set; }

            [Display(Name = "Group")]
            public int? GroupId { get; set; }

            [Display(Name = "Parent")]
            public int? ParentId { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            Roles = new List<SelectListItem>()
            {
                new SelectListItem("Student", "Student"),
                new SelectListItem("Teacher", "Teacher"),
                new SelectListItem("Parent", "Parent")
            };

            Groups = _context.Groups
                .Select(x => new SelectListItem(
                    x.Name,
                    x.Id.ToString()))
                .ToList();

            Parents = _context.Users
                .OfType<Parent>()
                .Select(x => new SelectListItem(
                    x.FirstName + " " + x.LastName,
                    x.Id.ToString()))
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            Roles = new List<SelectListItem>()
            {
                new SelectListItem("Student", "Student"),
                new SelectListItem("Teacher", "Teacher"),
                new SelectListItem("Parent", "Parent")
            };

            Groups = _context.Groups
                .Select(x => new SelectListItem(
                    x.Name,
                    x.Id.ToString()))
                .ToList();

            Parents = _context.Users
                .OfType<Parent>()
                .Select(x => new SelectListItem(
                    x.FirstName + " " + x.LastName,
                    x.Id.ToString()))
                .ToList();

            if (ModelState.IsValid)
            {
                User user;

                if (Input.Role == "Student")
                {
                    user = new Student()
                    {
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        Email = Input.Email,
                        UserName = Input.Email,
                        GroupId = Input.GroupId ?? 0,
                        ParentId = Input.ParentId ?? 0
                    };
                }
                else if (Input.Role == "Parent")
                {
                    user = new Parent()
                    {
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        Email = Input.Email,
                        UserName = Input.Email
                    };
                }
                else
                {
                    user = new Teacher()
                    {
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        Email = Input.Email,
                        UserName = Input.Email
                    };
                }

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Input.Role);

                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new
                        {
                            area = "Identity",
                            userId = userId,
                            code = code,
                            returnUrl = returnUrl
                        },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        Input.Email,
                        "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage(
                            "RegisterConfirmation",
                            new
                            {
                                email = Input.Email,
                                returnUrl = returnUrl
                            });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException(
                    "The default UI requires a user store with email support.");
            }

            return (IUserEmailStore<User>)_userStore;
        }
    }
}