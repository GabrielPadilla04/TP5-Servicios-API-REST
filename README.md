🚀 TP5 — Servicios API REST
<p align="center"> <strong>API REST desarrollada con ASP.NET Core 10, Entity Framework Core y MySQL</strong> </p> <p align="center"> <img src="https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 10"> <img src="https://img.shields.io/badge/ASP.NET%20Core-10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="ASP.NET Core"> <img src="https://img.shields.io/badge/MySQL-8-4479A1?style=for-the-badge&logo=mysql&logoColor=white" alt="MySQL"> <img src="https://img.shields.io/badge/Entity%20Framework%20Core-ORM-512BD4?style=for-the-badge" alt="Entity Framework Core"> <img src="https://img.shields.io/badge/JWT-Authentication-black?style=for-the-badge" alt="JWT"> </p>
📋 Descripción

Este proyecto consiste en una API REST desarrollada con ASP.NET Core 10, utilizando Entity Framework Core como ORM y MySQL como sistema de gestión de base de datos.

El sistema permite administrar:

📦 Productos e inventario

🏷️ Categorías

👥 Clientes

🚚 Proveedores

🔐 Usuarios y autenticación

🛒 Compras

💰 Ventas

🖼️ Imágenes de productos

📊 Movimientos y transacciones

Además, las operaciones de compras y ventas tienen impacto transaccional directo sobre el stock, permitiendo mantener actualizado el inventario.

🛠️ Tecnologías utilizadas
Tecnología	Uso
🟣 ASP.NET Core 10	Desarrollo de la API REST
🟣 Entity Framework Core	ORM y acceso a datos
🔵 MySQL	Base de datos relacional
🔐 JWT	Autenticación y autorización
📖 Swagger / OpenAPI	Documentación de la API
🧪 Bruno	Pruebas de endpoints
🗄️ Arquitectura y modelo de datos

La base de datos utiliza una estructura relacional, gestionada mediante AppDbContext y Entity Framework Core.

🔑 Entidades principales
👤 Usuarios

Gestiona:

Autenticación

Roles

Contraseñas almacenadas mediante hash

📦 Productos y categorías

Permite administrar el catálogo de productos y asociarlos con sus respectivas categorías e imágenes.

Las imágenes se almacenan en:

wwwroot/uploads/

👥 Clientes y proveedores

Representan las entidades involucradas en las diferentes operaciones comerciales del sistema.

📥 Ingresos y 📤 salidas

Las operaciones comerciales se dividen en:

Ingresos: representan compras realizadas a proveedores.

Salidas: representan ventas realizadas a clientes.

Ambas operaciones actualizan el stock de los productos involucrados.

📊 Transacciones

Permiten registrar y controlar los movimientos realizados sobre el inventario.

📐 Diagrama Entidad-Relación
<p align="center"> <img src="./docs/der.png" alt="Diagrama Entidad-Relación" width="900"> </p>
⚙️ Instalación y configuración
1️⃣ Prerrequisitos

Antes de ejecutar el proyecto, necesitás tener instalado:

.NET 10 SDK

MySQL Server

XAMPP o Docker, en caso de utilizar alguno de ellos

Bruno
 — opcional, para ejecutar las pruebas

2️⃣ Clonar el repositorio
git clone https://github.com/GabrielPadilla04/TP5-Servicios-API-REST.git

cd TP5-Servicios-API-REST

git checkout Gabirama

3️⃣ Configurar la base de datos

Abrí el archivo:

appsettings.json


y configurá la conexión a tu servidor MySQL.

🔗 Connection String
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=tp5_servicios_db;User=root;Password=tu_contraseña;"
  },
  "Jwt": {
    "Key": "TuClaveSecretaSuperSeguraDeAlMenos32Caracteres!",
    "Issuer": "TP5ApiREST",
    "Audience": "TP5ApiRESTUsers"
  }
}


⚠️ Importante: No compartas claves reales ni secretos JWT en repositorios públicos.

🗃️ Crear la base de datos

Una vez configurada la conexión, ejecutá las migraciones de Entity Framework Core:

dotnet ef database update


Esto creará las tablas y relaciones necesarias en la base de datos configurada.

Si no tenés instalado dotnet-ef:

dotnet tool install --global dotnet-ef

▶️ Ejecutar la API

Para iniciar el servidor de desarrollo:

dotnet run


La API estará disponible en:

🔒 HTTPS
https://localhost:7001

🌐 HTTP
http://localhost:5001

📖 Swagger

Una vez iniciada la aplicación, podés acceder a la documentación interactiva mediante:

<p align="center">
🔗 https://localhost:7001/swagger
</p>

Swagger permite visualizar y probar directamente los diferentes endpoints disponibles en la API.

🧪 Pruebas con Bruno

El proyecto incluye una colección de pruebas preparada para Bruno API Client mediante archivos .bru.

📂 Abrir la colección

Abrí Bruno.

Seleccioná Open Collection.

Elegí la carpeta correspondiente al proyecto.

Ejecutá las peticiones disponibles.

🔐 1. Autenticación

Primero ejecutá:

POST /api/Auth/login


Esta petición genera un JWT Bearer Token.

Luego utilizá el token obtenido para acceder a los endpoints protegidos.

Authorization
Authorization: Bearer <TOKEN>

📚 2. Catálogos

Podés probar las operaciones CRUD de:

/api/Categorias
/api/Productos
/api/Clientes
/api/Proveedores

🖼️ 3. Carga de imágenes

La creación de productos con imagen utiliza:

multipart/form-data


Las imágenes se almacenan en:

wwwroot/uploads/

🔄 Operaciones transaccionales

Una de las funcionalidades principales del sistema es el manejo automático del stock.

🛒 Compra
POST /api/Ingresos


Una compra genera un ingreso de stock.

Proveedor
    ↓
Ingreso
    ↓
Detalle del ingreso
    ↓
📦 STOCK ↑

💰 Venta
POST /api/Salidas


Una venta genera un egreso de stock.

Cliente
    ↓
Salida
    ↓
Detalle de la salida
    ↓
📦 STOCK ↓


⚡ Las operaciones de inventario se realizan de manera transaccional, manteniendo sincronizados los movimientos y el stock.

📁 Estructura del proyecto
TP5-Servicios-API-REST/
│
├── 📁 Controllers/
├── 📁 Models/
├── 📁 Data/
├── 📁 Migrations/
│
├── 📁 docs/
│   └── der.png
│
├── 📁 wwwroot/
│   └── uploads/
│
├── 📄 appsettings.json
├── 📄 Program.cs
└── 📄 README.md

✨ Funcionalidades principales
Funcionalidad	Estado
🔐 Autenticación JWT	✅
👤 Gestión de usuarios	✅
📦 Gestión de productos	✅
🏷️ Gestión de categorías	✅
👥 Gestión de clientes	✅
🚚 Gestión de proveedores	✅
🖼️ Carga de imágenes	✅
🛒 Registro de compras	✅
💰 Registro de ventas	✅
📊 Actualización automática de stock	✅
📖 Swagger	✅
🧪 Colección Bruno	✅
🗃️ Entity Framework Core	✅
🐬 MySQL	✅
👨‍💻 Equipo de desarrollo
<p align="center">
Gabriel Padilla
Valentín
</p>
<p align="center"> <strong>TP5 — Servicios API REST</strong><br> ASP.NET Core 10 · Entity Framework Core · MySQL </p>
