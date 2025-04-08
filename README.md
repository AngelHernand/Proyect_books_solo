# 📚 Proyecto Solo Recommendation

Este es un proyecto desarrollado en **ASP.NET Core MVC** 🌐, diseñado para proporcionar un sistema de recomendaciones personalizadas, administración de usuarios y búsqueda de libros usando la API de OpenLibrary 📖. Además, incluye características avanzadas como autenticación, roles de usuario y personalización de perfiles.

---

## ✨ **Funcionalidades Principales**

### 👥 **Usuarios**
- 🔐 Registro de nuevos usuarios.
- 🔑 Inicio y cierre de sesión con autenticación JWT.
- 🛠️ Edición del perfil de usuario (incluyendo contraseña e imagen de perfil).
- 🎭 Roles:
  - **Administrador**.
  - **Usuario Estándar**.

### 🛡️ **Panel de Administración**
- ⚙️ Acceso restringido a administradores.
- 👤 Gestión de usuarios con sus roles asignados.

### 🔎 **Búsqueda de Libros**
- 🔍 Integración con la API de OpenLibrary.
- 📚 Resultados sobre libros en tiempo real directamente desde la API.

### 📄 **Vistas Incluidas**
- 🏠 **Inicio**: Página principal.
- 📊 **Dashboard**: Panel tras iniciar sesión.
- 🔒 **Privacy**: Declaración de privacidad.
- 📝 **Registro**: Crear perfil de usuario.
- 🔑 **Login**: Inicio de sesión.
- 🖋️ **Edición de Perfil**: Administración del perfil personal.
- ⚙️ **Admin Panel**: Gestión de usuarios (solo administradores).

---

## 💻 **Tecnologías Utilizadas**

### **Backend**
- 🖥️ **ASP.NET Core 8.0**: Framework usado para el desarrollo de la aplicación web.
- 📋 **Entity Framework Core**: ORM para la gestión de la base de datos.
- 🎫 **JWT**: Implementación de autenticación basada en tokens.

### **Frontend**
- 🌟 **Razor Views**: Motor de plantillas para renderizar HTML dinámico.

### **Integraciones**
- 🌍 **OpenLibrary API**: Para la búsqueda y obtención de datos de libros.

---

## 🚀 **Instalación**

### 1️⃣ **Clonar el Repositorio**
Clona el repositorio usando el siguiente comando en tu terminal:

```bash
git clone https://github.com/<tu-usuario>/Proyect_Solo_Recommendation.git
cd Proyect_Solo_Recommendation
```
2️⃣ Configurar la Base de Datos
Actualiza la configuración de la conexión con tu base de datos en el archivo appsettings.json:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProyectSoloRecommendation;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```
3️⃣ Inicializar la Base de Datos
Aplica las migraciones de Entity Framework Core para construir tu base de datos:
```bash
dotnet ef database update
```
4️⃣ Ejecutar la Aplicación
Lanza el servidor local con el siguiente comando:
```bash
dotnet run
```
5️⃣ Abrir en el Navegador
Accede a la aplicación visitando la siguiente URL en tu navegador:
```
http://localhost:5000.
```
🌐 API de OpenLibrary
Este proyecto consume la API de OpenLibrary para proporcionar datos detallados sobre libros. El servicio OpenLibraryService maneja estas integraciones y expone la funcionalidad bajo la ruta /SearchBooks.

🔎 Ejemplo de Llamada
Puedes realizar una búsqueda mediante la siguiente estructura en la URL:
```
GET /SearchBooks?query=<nombre_del_libro>
```

Ejemplo:
/SearchBooks?query=harry+potter

💡 Respuesta
Si la llamada es exitosa, se regresa un JSON con resultados:
```json
{
  "success": true,
  "books": [
    {
      "title": "Harry Potter and the Philosopher's Stone",
      "author": "J.K. Rowling",
      "coverUrl": "https://covers.openlibrary.org/b/id/12345-L.jpg"
    }
  ]
}
```

💻 Inicio (Home)
Muestra la página de aterrizaje principal para los usuarios no autenticados.


🔧 Admin Panel
Pantalla de administración para gestionar usuarios y roles (solo disponible para administradores).

✏️ Editar Perfil
Sección donde los usuarios pueden actualizar información personal, cambiar contraseñas y subir fotos de perfil.

