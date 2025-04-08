using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyect_Solo_Recommendation.Models;
using Proyect_Solo_Recommendation.Data; // Asegúrate de incluir este namespace
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Proyect_Solo_Recommendation.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDBContext _context; // Inyección del contexto

    public HomeController(ILogger<HomeController> logger, ApplicationDBContext context)
    {
        _logger = logger;
        _context = context; // Asignar el contexto inyectado
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Authorize]
    public IActionResult Dashboard()
    {
        // Deshabilitar el almacenamiento en caché
        Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        Response.Headers["Pragma"] = "no-cache";
        Response.Headers["Expires"] = "0";

        // Extraer los datos del usuario autenticado
        var username = User.Identity?.Name;

        // Obtener el usuario desde la base de datos
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user == null)
        {
            return RedirectToAction("Login", "User");
        }

        // Verificar si el usuario es administrador
        var isAdmin = user.RoleId == 2;

        // Pasar los datos a la vista
        ViewData["Username"] = user.Username;
        ViewData["Role"] = isAdmin ? "Admin" : "Usuario";
        ViewData["ProfileImage"] = user.ProfileImage;
        ViewData["IsAdmin"] = isAdmin;

        return View();
    }
    
    [HttpGet("SearchBooks")]
    public async Task<IActionResult> SearchBooks(string query, [FromServices] OpenLibraryService openLibraryService)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return Json(new { success = false, message = "La consulta de búsqueda está vacía." });
        }

        var books = await openLibraryService.SearchBooksAsync(query);
        return Json(new { success = true, books });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
