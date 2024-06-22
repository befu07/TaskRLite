﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using TaskRLite.Data;
using TaskRLite.Services;
using TaskRLite.Views.Auth;
namespace TaskRLite.Controllers;

public class AuthController : Controller
{
    private readonly AccountService _accountService;

    public AuthController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync();

        return RedirectToAction(nameof(HomeController.Index), HomeController.Name);
    }


    [HttpPost]
    public async Task<IActionResult> Register(TaskRLite.Views.Auth.RegisterVm form)
    {
        if (ModelState.IsValid)
        {
            bool success = await _accountService.RegisterNewUserAsync(form.Username, form.Password, form.Email);
            if (!success)
            {
                TempData["ErrorMessage"] = "Registration failed";
                // Todo testln?
                return RedirectToAction(nameof(Register));
                return View();
            }
            Console.WriteLine($"Jemand hat sich mit {form.Username} und {form.Password} registriert");
            return RedirectToAction(nameof(Login));
        }
        else
        {
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var userCanLogIn = await _accountService.CanUserLogInAsync(email, password);
        Console.WriteLine($"Jemand hat sich mit {email} und {password} eingeloggt: {userCanLogIn}");

        if (userCanLogIn)
        {
            //Wir erkennen den Benutzer, und er darf sich wirklich "einloggen"
            var role = await LogUserIntoWebApp(email);
            if (role.RoleName == "Admin")
            {
                return RedirectToAction(nameof(AdminController.UserOverView), AdminController.Name);
            }
            // Free- und Premium-Tier Benutzer sollen auf die Übersichtsseite ihrer To - Do Listen weitergeleitet werden,
            return RedirectToAction(nameof(ToDoController.Index), ToDoController.Name);
        }
        else //Wenn wir den Benutzer NICHT erkennen, ...
        {
            //...wird er auf die Login-Seite zurückgeleitet
            TempData["ErrorMessage"] = "Zugangsdaten Falsch";            
            return RedirectToAction(nameof(Login));
        }
    }

    [NonAction]
    private async Task<AppUserRole> LogUserIntoWebApp(string email)
    {
        //1. Claims (Behauptungen) über den Benutzer zusammentragen
        //Ein Claim ist einfach nur ein Key-Value-Pair - Ein Wert mit einem bestimmten Namen
        var claim = new Claim("LastLogin", DateTime.Now.ToString());

        AppUser user = await _accountService.GetUserByEmail(email);

        //Damit das ASP.NET Core Auth-System mit Cookies funktioniert, ist ein Claim besonders wichtig: Der Name-Claim
        var nameClaim = new Claim(ClaimTypes.Name, user.Username);

        //AppRole role = await _accountService.GetRoleByUserNameAsync(email);
        //var roleClaim = new Claim(ClaimTypes.Role, role.RoleName);

        var roleClaim = new Claim(ClaimTypes.Role, user.AppRole.RoleName);

        //Alle Claims die dieser Liste (und damit der Identity bzw. dem Principal) hinzugefügt werden,
        //...werden dann im Auth-Cookie gespeichert (und können dort auch wieder ausgelesen werden)
        var claims = new List<Claim>()
        {
            claim, nameClaim,roleClaim
        };


        //2. Mit den Claims eine Identität erstellen
        //In unserem Fall benötigt der Principal eine Identität, da wir einzelne Benutzer voneinander unterscheiden wollen
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //3. Die Identität einem Rechteinhaber zuweisen
        //In der Microsoft bzw. .NET Welt ist ein "Principal" ein Rechteinhaber
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await Console.Out.WriteLineAsync(nameClaim.Value + " " + roleClaim.Value);
        //4. Den Rechteinhaber in der Anwendung "registrieren"
        //Um einen Benutzer in der Webanwendung als "eingeloggt" zu "markieren" (und ihm das Auth-Cookie zu schicken),
        //gibt es die Methode SignInAsync

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal
            );

        await Console.Out.WriteLineAsync(nameClaim.Value + " eingeloggt");

        //Der ClaimsPrincipal der hier erzeugt und "registriert" wird, ist nachher auch über eine spezielle Variable
        //...in Controllern und Views abrufbar. Die Variable heißt "User" und enthält neben allen Informationen
        //...die über die Claims hier gespeichert werden, auch nützliche Hilfsmethoden/-Eigenschaften.

        //zB
        //User.Identity.Name erlaubt Zugriff auf den abgespeicherten Namen
        //User.Identity.IsAuthenticated meldet, ob gerade ein Benutzer eingeloggt ist oder nicht (nützlich, um zB
        //...Interface-Elemente anzuzeigen bzw. zu verstecken, je nachdem ob ein eingeloggter oder anonymer Benutzer
        //...die View gerade ansieht).
        //User.Claims erlaubt Zugriff auf alle gespeicherten Claims
        return user.AppRole;
    }
}

