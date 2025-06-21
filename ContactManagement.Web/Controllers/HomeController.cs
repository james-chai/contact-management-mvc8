using ContactManagement.Core;
using ContactManagement.Data.Repository;
using ContactManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ContactManagement.MVC.Controllers;

public class HomeController(
    IContactData contactData,
    ILogger<HomeController> logger) : Controller
{
    public string Message { get; set; } = string.Empty;
    public List<Contact> Contacts { get; set; } = [];

    public async Task<IActionResult> Index(string searchTerm)
    {
        try
        {
            ViewBag.SearchTerm = searchTerm;
            await LoadContactsAsync(searchTerm);

            return View(Contacts);
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("Contact search was cancelled");
            Message = "Request cancelled";
            return View(new List<Contact>());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching contacts for term: {SearchTerm}", searchTerm);
            Message = "Error loading contacts";
            return View(new List<Contact>());
        }
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
        => View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });

    private async Task LoadContactsAsync(string searchTerm)
    {
        logger.LogInformation("Fetching contacts for search term: {SearchTerm}", searchTerm);

        var contacts = await contactData.SearchContactsAsync(searchTerm, HttpContext.RequestAborted);

        Contacts = contacts.ToList();
    }
}