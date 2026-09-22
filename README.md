🚀 TP5 — Servicios API REST
---

## 📋 Descripción

Este proyecto consiste en una API REST desarrollada con ASP.NET Core 10, utilizando Entity Framework Core como ORM y MySQL como sistema de gestión de base de datos[cite: 1].

El sistema permite administrar:
- 📦 **Productos e inventario**
- 🏷️ **Categorías**
- 👥 **Clientes**
- 🚚 **Proveedores**
- 🔐 **Usuarios y autenticación**
- 🛒 **Compras**
- 💰 **Ventas**
- 🖼️ **Imágenes de productos**
- 📊 **Movimientos y transacciones**

Además, las operaciones de compras y ventas tienen impacto transaccional directo sobre el stock, permitiendo mantener actualizado el inventario.

---

## 🛠️ Tecnologías Utilizadas

| Tecnología | Uso |
| :--- | :--- |
| 🟣 **ASP.NET Core 10** | Desarrollo de la API REST[cite: 1] |
| 🟣 **Entity Framework Core** | ORM y acceso a datos[cite: 1] |
| 🔵 **MySQL** | Base de datos relacional[cite: 1] |
| 🔐 **JWT** | Autenticación y autorización[cite: 1] |
| 📖 **Scalar / OpenAPI** | Documentación interactiva de la API |
| 🧪 **Bruno** | Pruebas de endpoints (`.bru`)[cite: 1, 4] |

---

## 🗄️ Arquitectura y Modelo de Datos

La base de datos utiliza una estructura relacional, gestionada mediante `AppDbContext` y Entity Framework Core.

### 🔑 Entidades Principales

* 👤 **Usuarios:** Gestión de autenticación, roles y contraseñas almacenadas mediante hash.
* 📦 **Productos y Categorías:** Administración del catálogo de productos y asociación con sus respectivas categorías e imágenes. Las imágenes se almacenan físicamente en `wwwroot/uploads/`.
* 👥 **Clientes y Proveedores:** Entidades involucradas en las diferentes operaciones comerciales del sistema.
* 📥 **Ingresos y 📤 Salidas:** 
  * **Ingresos:** Compras realizadas a proveedores.
  * **Salidas:** Ventas realizadas a clientes.
  * *Ambas operaciones actualizan el stock de los productos involucrados de forma atómica.*
* 📊 **Transacciones:** Registro y control de movimientos e historial de auditoría sobre el inventario.

### 📐 Diagrama Entidad-Relación

<img width="1178" height="582" alt="image" src="https://github.com/user-attachments/assets/cdda54a0-d822-4427-851a-c2d019b783bd" />


---

## ⚙️ Instalación y Configuración

### 1️⃣ Prerrequisitos

Antes de ejecutar el proyecto, necesitas tener instalado:
* [.NET 10 SDK](https://dotnet.microsoft.com/)
* Servidor **MySQL** (vía MySQL Server, XAMPP o Docker)
* [Bruno](https://www.usebruno.com/) *(opcional, para ejecutar la colección de pruebas)*

### 2️⃣ Clonar el Repositorio

```bash
git clone [https://github.com/GabrielPadilla04/TP5-Servicios-API-REST.git](https://github.com/GabrielPadilla04/TP5-Servicios-API-REST.git)
cd TP5-Servicios-API-REST
git checkout Gabirama

```

### 3️⃣ Configurar la Base de Datos

Abre el archivo `appsettings.json` y configura la conexión a tu servidor MySQL:

```json
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

```

> ⚠️ **Importante:** No compartas claves reales ni secretos JWT en repositorios públicos.

### 4️⃣ Crear la Base de Datos

Una vez configurada la conexión, ejecuta las migraciones de Entity Framework Core:

```bash
dotnet ef database update

```

*Si no tienes instalada la herramienta global de EF Core:*

```bash
dotnet tool install --global dotnet-ef

```

### 5️⃣ Ejecutar la API

Para iniciar el servidor de desarrollo:

```bash
dotnet run

```

La API estará disponible en:

* 🔒 **HTTPS:** `https://localhost:7235`
* 🌐 **HTTP:** `http://localhost:5235`

---

## 📖 Documentación Interactiva (Scalar)

Una vez iniciada la aplicación, puedes acceder a la documentación interactiva y probar los endpoints mediante **Scalar API Reference**:

Scalar permite visualizar, explorar y probar directamente los diferentes endpoints disponibles en la API de forma moderna e interactiva.

---

## 🧪 Pruebas con Bruno

El proyecto incluye la colección de pruebas preparada para **Bruno API Client** (`.bru`).

### 📂 Abrir la colección

1. Abre **Bruno**.
2. Selecciona **Open Collection**.
3. Elige la carpeta del proyecto.
4. Ejecuta las peticiones disponibles.

### 🔐 1. Autenticación

Ejecuta la petición para generar el JWT Bearer Token:

```http
POST /api/Auth/login

```

Copia el token obtenido e inclúyelo en las cabeceras de autorización para acceder a los endpoints protegidos:

```http
Authorization: Bearer <TOKEN>

```

### 📚 2. Catálogos

Puedes probar las operaciones CRUD de:

* `/api/Categorias`
* `/api/Productos`
* `/api/Clientes`
* `/api/Proveedores`

### 🖼️ 3. Carga de Imágenes

La creación de productos con imagen utiliza `multipart/form-data`. Las imágenes se almacenan automáticamente en la ruta física:

```text
wwwroot/uploads/

```

### 🔄 4. Operaciones Transaccionales

Una de las funcionalidades principales del sistema es el manejo automático e incremental del stock:

```text
🛒 COMPRA (POST /api/Ingresos)
Proveedor ──► Ingreso ──► Detalle Ingreso ──► 📦 STOCK ↑

💰 VENTA (POST /api/Salidas)
Cliente ──► Salida ──► Detalle Salida ──► 📦 STOCK ↓

```

⚡ *Las operaciones de inventario se realizan de manera transaccional, manteniendo sincronizados los movimientos y el stock.*

---

## 📁 Estructura del Proyecto

```text
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

```

---

## ✨ Funcionalidades Principales

| Funcionalidad | Estado |
| --- | --- |
| 🔐 Autenticación JWT | ✅ |
| 👤 Gestión de Usuarios | ✅ |
| 📦 Gestión de Productos | ✅ |
| 🏷️ Gestión de Categorías | ✅ |
| 👥 Gestión de Clientes | ✅ |
| 🚚 Gestión de Proveedores | ✅ |
| 🖼️ Carga de Imágenes (`wwwroot/uploads`) | ✅ |
| 🛒 Registro de Compras (Ingresos) | ✅ |
| 💰 Registro de Ventas (Salidas) | ✅ |
| 📊 Actualización automática de stock | ✅ |
| 📖 Documentación con Scalar | ✅ |
| 🧪 Colección de pruebas en Bruno (`.bru`) | ✅ |
| 🗃️ Entity Framework Core | ✅ |
| 🐬 MySQL | ✅ |

---

## 👨‍💻 Equipo de Desarrollo
<p align="center">
Gabriel Padilla -
Valentín Montes
</p>
