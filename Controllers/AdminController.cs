using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyect_Solo_Recommendation.Data;
using Proyect_Solo_Recommendation.Models; // Add the namespace where EditProfileViewModel is defined

[Authorize]
[Route("Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDBContext _context;

    public AdminController(ApplicationDBContext context)
    {
        _context = context;
    }

    [HttpGet("Users")]
    public IActionResult Users()
    {
        var username = User.Identity?.Name;
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user == null || user.RoleId != 2) // Verifica directamente el RoleId
        {
            return Forbid(); // Devuelve un error 403 si no es administrador
        }

        // Cargar usuarios con sus roles
        var users = _context.Users.Include(u => u.Role).ToList();

        return View(users);
    }

    [HttpGet("EditUser/{id}")]
    public IActionResult EditUser(int id)
    {
        // Buscar el usuario por su ID
        var user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            return NotFound(); // Si no se encuentra el usuario, devolver 404
        }

        // Crear un modelo de vista para la edición
        var model = new EditProfileViewModel
        {
            Id = user.Id, // Asigna el Id aquí
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Description = user.Description,
            CurrentPassword = user.Password, // Solo para referencia, no se debe mostrar en el formulario
            ProfileImage = null // No se envía la imagen actual al formulario
        };

        ViewData["Roles"] = _context.Roles.ToList(); // Pasar los roles disponibles a la vista
        return View(model);
    }

    [HttpPost("EditUser/{id}")]
    public IActionResult EditUser(int id, EditProfileViewModel model, IFormFile? profileImage)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Roles"] = _context.Roles.ToList();
            return View(model);
        }

        var user = _context.Users.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        // Actualizar los datos del usuario
        user.Firstname = model.Firstname;
        user.Lastname = model.Lastname;
        user.Description = model.Description;
        user.RoleId = model.RoleId;

        // Actualizar la contraseña si se proporciona una nueva
        if (!string.IsNullOrEmpty(model.NewPassword))
        {
            user.Password = model.NewPassword; // Asegúrate de aplicar hashing si es necesario
        }

        // Manejar la actualización de la imagen de perfil
        if (profileImage != null && profileImage.Length > 0)
        {
            var uploadDir = Path.Combine("wwwroot", "images", "profiles");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(profileImage.FileName);
            var filePath = Path.Combine(uploadDir, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                profileImage.CopyTo(stream);
            }

            user.ProfileImage = $"/images/profiles/{uniqueFileName}";
        }

        _context.SaveChanges();
        return RedirectToAction("Users");
    }
}