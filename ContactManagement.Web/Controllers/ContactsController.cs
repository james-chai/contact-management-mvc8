using ContactManagement.Core;
using ContactManagement.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagement.MVC.Controllers;

public class ContactsController(
    IContactData contactData,
    ILogger<ContactsController> logger) : Controller
{
    private const string HomeIndexAction = nameof(HomeController.Index);

    // GET: ContactsController
    public IActionResult Index() => RedirectToAction(HomeIndexAction, "Home");

    // GET: ContactsController/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        var contact = await GetContactOrNotFoundAsync(id);

        return contact is null ? NotFound() : View(contact);
    }

    // GET: ContactsController/Create
    public IActionResult Create() => View(new Contact());

    // POST: ContactsController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Contact contact)
    {
        if (!ModelState.IsValid)
            return View(contact);

        InitializeNewContact(contact);
        contactData.Add(contact);
        await contactData.CommitAsync(HttpContext.RequestAborted);

        return RedirectToHome();
    }

    // GET: ContactsController/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        var contact = await GetContactOrNotFoundAsync(id);

        return contact is null ? NotFound() : View(contact);
    }

    // POST: ContactsController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Contact updatedContact)
    {
        if (!ModelState.IsValid)
            return View(updatedContact);

        var existingContact = await GetContactOrNotFoundAsync(id);
        if (existingContact is null)
            return NotFound();

        contactData.Update(updatedContact);

        await contactData.CommitAsync(HttpContext.RequestAborted);

        return RedirectToHome();
    }

    // GET: ContactsController/Delete/5
    public async Task<IActionResult> Delete(Guid id)
    {
        var contact = await GetContactOrNotFoundAsync(id);

        return contact is null ? NotFound() : View(contact);
    }

    // POST: ContactsController/DeleteConfirmed/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            var contact = await GetContactOrNotFoundAsync(id);
            if (contact is null)
                return NotFound();

            await contactData.DeleteAsync(id, HttpContext.RequestAborted);

            return RedirectToHome();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting contact {ContactId}", id);
            ModelState.AddModelError(string.Empty, "Failed to delete contact");

            return await Delete(id);
        }
    }

    private async Task<Contact?> GetContactOrNotFoundAsync(Guid id)
        => await contactData.GetByIdAsync(id, HttpContext.RequestAborted);

    private static void InitializeNewContact(Contact contact)
    {
        contact.Id = Guid.NewGuid();
        contact.CreatedAt = DateTime.UtcNow;
        contact.UpdatedAt = contact.CreatedAt;
    }

    private IActionResult RedirectToHome()
        => RedirectToAction(HomeIndexAction, "Home");
}