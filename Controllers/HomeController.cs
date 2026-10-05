using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using campusfix.Models;
using campusfix.Data;

namespace campusfix.Controllers;

public class HomeController : Controller
{
    private readonly CampusFixContext _context;

    public HomeController(CampusFixContext context)
    {
        _context = context;
    }

    // =========================
    // PUBLIC HOME
    // =========================

    public IActionResult Index()
    {
        return View();
    }

    // =========================
    // REPORT AN ISSUE
    // =========================

    public IActionResult Report()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Report(string Title, string Location, string Description)
    {
        var issue = new Issue
        {
            Title = Title,
            Location = Location,
            Description = Description
        };

        _context.Issues.Add(issue);
        _context.SaveChanges();

        return Content($"CampusFix: Report #{issue.Id} was saved successfully!");
    }

    // =========================
    // PUBLIC CAMPUS REPORTS
    // =========================

    [HttpGet]
    public IActionResult Reports()
    {
        var issues = _context.Issues
            .OrderByDescending(i => i.Id)
            .ToList();

        return View(issues);
    }

    // =========================
    // ADMIN LOGIN
    // =========================

    [HttpGet]
    public IActionResult AdminLogin()
    {
        if (IsAdmin())
        {
            return RedirectToAction("Admin");
        }

        return View();
    }

    [HttpPost]
    public IActionResult AdminLogin(string username, string password)
    {
        if (username == "admin" && password == "CampusFix2026!")
        {
            HttpContext.Session.SetString("AdminLoggedIn", "true");

            return RedirectToAction("Admin");
        }

        ViewBag.Error = "Invalid username or password.";

        return View();
    }

    // =========================
    // ADMIN DASHBOARD
    // =========================

    public IActionResult Admin()
    {
        if (!IsAdmin())
        {
            return RedirectToAction("AdminLogin");
        }

        var issues = _context.Issues
            .OrderByDescending(i => i.Id)
            .ToList();

        return View(issues);
    }

    // =========================
    // ADMIN - MANAGE INDIVIDUAL REPORT
    // =========================

    public IActionResult Manage(int id)
    {
        if (!IsAdmin())
        {
            return RedirectToAction("AdminLogin");
        }

        var issue = _context.Issues.Find(id);

        if (issue == null)
        {
            return NotFound();
        }

        return View(issue);
    }

    // =========================
    // ADMIN - UPDATE STATUS
    // =========================

    [HttpPost]
    public IActionResult UpdateStatus(int id, string status)
    {
        if (!IsAdmin())
        {
            return RedirectToAction("AdminLogin");
        }

        var issue = _context.Issues.Find(id);

        if (issue == null)
        {
            return NotFound();
        }

        issue.Status = status;
        _context.SaveChanges();

        return RedirectToAction("Manage", new { id = id });
    }

    // =========================
    // ADMIN - DELETE REPORT
    // =========================

    [HttpPost]
    public IActionResult DeleteReport(int id)
    {
        if (!IsAdmin())
        {
            return RedirectToAction("AdminLogin");
        }

        var issue = _context.Issues.Find(id);

        if (issue == null)
        {
            return NotFound();
        }

        _context.Issues.Remove(issue);
        _context.SaveChanges();

        return RedirectToAction("Admin");
    }

    // =========================
    // ADMIN - LOGOUT
    // =========================

    public IActionResult AdminLogout()
    {
        HttpContext.Session.Remove("AdminLoggedIn");

        return RedirectToAction("Index");
    }

    // =========================
    // ADMIN AUTH CHECK
    // =========================

    private bool IsAdmin()
    {
        return HttpContext.Session.GetString("AdminLoggedIn") == "true";
    }

    // =========================
    // PRIVACY / ABOUT
    // =========================

    public IActionResult Privacy()
    {
        return View();
    }

    // =========================
    // ERROR
    // =========================

    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}