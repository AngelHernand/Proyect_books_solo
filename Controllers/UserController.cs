using Microsoft.AspNetCore.Mvc;
using Proyect_Solo_Recommendation.Data;
using Proyect_Solo_Recommendation.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;


namespace Proyect_Solo_Recommendation.Controllers
{
    [Route("User")]
    [ApiController]
    public class UserController: Controller
    {
        private readonly ApplicationDBContext _context;

        public UserController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Mostrar formulario para crear un nuevo usuario
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // Procesar la creación de un nuevo usuario
        [HttpPost("Create")]
        public IActionResult Create([FromForm] User user, IFormFile profileImage)
        {
            if (ModelState.IsValid)
            {
                if (profileImage != null && profileImage.Length > 0)
                {
                    // Generar un nombre único para la imagen
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(profileImage.FileName);

                    // Ruta completa donde se guardará la imagen
                   var imageDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles");

                    if (!Directory.Exists(imageDir))
                    {
                        Directory.CreateDirectory(imageDir);
                    }

                    var filePath = Path.Combine(imageDir, fileName);

                    // Guardar la imagen
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        profileImage.CopyTo(stream);
                    }

                    // Guardar la ruta relativa en el modelo
                    user.ProfileImage = $"/images/profiles/{fileName}";
                }

                user.RoleId = 1; // Asignar siempre el valor 1 al campo RoleId
                _context.Users.Add(user);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "¡Usuario registrado exitosamente!";
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public IActionResult Login([FromForm] string username, [FromForm] string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
                return View();
            }

            // Crear el token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("EstaEsUnaClaveSuperSeguraDe32Caracteres!");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role?.Name ?? "User")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Guardar el token en una cookie segura
            Response.Cookies.Append("AuthToken", tokenString, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            TempData["SuccessMessage"] = "Inicio de sesión exitoso.";

            // Redirigir al Dashboard
            return RedirectToAction("Dashboard", "Home");
        }
     
      [HttpGet("EditProfile")]
[Authorize]
public IActionResult EditProfile()
{
    // Verificar si el usuario está autenticado
    var username = User.Identity?.Name;
    if (string.IsNullOrEmpty(username))
    {
        return Unauthorized(); // Devuelve un error 401 si no está autenticado
    }

    // Buscar el usuario en la base de datos
    var user = _context.Users.FirstOrDefault(u => u.Username == username);
    if (user == null)
    {
        return NotFound(); // Devuelve un error 404 si no se encuentra el usuario
    }

    // Convertir el usuario en el modelo de vista
    var model = new EditProfileViewModel
    {
        Firstname = user.Firstname,
        Lastname = user.Lastname,
        Description = user.Description,
        CurrentPassword = string.Empty // Initialize with an empty string or a default value
    };

    return View(model);
}


[HttpPost("EditProfile")]
[Authorize]
public IActionResult EditProfile([FromForm] EditProfileViewModel model)
{
    var username = User.Identity?.Name;
    var user = _context.Users.FirstOrDefault(u => u.Username == username);

    if (user == null)
    {
        return NotFound();
    }

    // ✅ 1. Verificar que la contraseña ingresada es correcta
    if (model.CurrentPassword != user.Password)
    {
        ModelState.AddModelError("CurrentPassword", "Contraseña incorrecta.");
        return View(model);
    }

    if (ModelState.IsValid)
    {
        // ✅ 2. Actualizar los datos
        user.Firstname = model.Firstname;
        user.Lastname = model.Lastname;
        user.Description = model.Description;

        // ✅ 3. Si se ingresa una nueva contraseña, actualizarla
        if (!string.IsNullOrEmpty(model.NewPassword))
        {
            user.Password = model.NewPassword;
        }

        // ✅ 4. Actualizar imagen si se sube una nueva
        if (model.ProfileImage != null && model.ProfileImage.Length > 0)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfileImage.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                model.ProfileImage.CopyTo(stream);
            }

            user.ProfileImage = $"/images/profiles/{fileName}";
        }

        _context.SaveChanges();
        TempData["SuccessMessage"] = "Perfil actualizado exitosamente.";
        return RedirectToAction("Dashboard", "Home");
    }

    return View(model);
}
        [HttpGet("Logout")]
public IActionResult Logout()
{
    // Eliminar la cookie del token
    Response.Cookies.Delete("AuthToken");

    // Redirigir al Index
    return RedirectToAction("Index", "Home");
}

    }
    
}
