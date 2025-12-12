# Busticket
BusTicket – Sistema de Gestión de Rutas y Venta de Boletos

BusTicket es una aplicación web desarrollada en webforms
que permite gestionar rutas de transporte, visualizar información detallada de cada ruta, seleccionar asientos disponibles y generar compras.

Incluye integración con Cloudinary, manejo de carrito de compras, sesiones, migraciones con Entity Framework Core, y un diseño moderno usando TailwindCSS.

🚀 Tecnologías utilizadas

ASP.NET webforms


Entity Framework Core

SQL Server

TailwindCSS



C#

JavaScript

HTML / CSS

✨ Características principales

✔ Gestión de rutas
✔ Visualización de buses y asientos
✔ Carrito de compra para los asientos seleccionados
✔ Cálculo automático del total
✔ Sistema de ventas
✔ Integración con Cloudinary para subir imágenes
✔ Sesiones para mantener los datos del carrito
✔ Migraciones automáticas con EF Core

📦 Instalación y configuración
1️⃣ Clonar el repositorio
git clone https://github.com/carlos1barragan/BusTicket1.git

2️⃣ Restaurar dependencias

Al abrir la solución en Visual Studio, este restaurará automáticamente los paquetes NuGet.

También puedes hacerlo manualmente:

dotnet restore

🔧 Configuración de la base de datos
3️⃣ Configura tu cadena de conexión

En appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=BusTicketDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}



🛠 Aplicar migraciones

Si aún no tienes la base de datos creada:

dotnet ef database update


Para crear nuevas migraciones:

dotnet ef migrations add NombreMigracion

▶️ Ejecutar el proyecto
dotnet run


O desde Visual Studio presiona F5.

🗂 Estructura del proyecto
BusTicket1/
│── Controllers/
│── Models/
│── Views/
│── Services/
│── Data/
│── wwwroot/
│── appsettings.json

💻 Funcionalidades destacadas
🚌 Gestión de rutas

Ver rutas disponibles

Ver ciudades, precios, descripciones e imágenes

🎫 Selección de asientos

Los usuarios seleccionan asientos interactivos

Se guardan en el carrito usando sesiones

🛒 Carrito de compras

Mostrar cantidad de asientos

Calcular total

Guardar ventas en la base de datos

🤝 Contribuciones

¡Las contribuciones son bienvenidas!
Puedes crear issues o hacer pull requests.

📄 Licencia

Proyecto para aprendizaje — uso libre.
