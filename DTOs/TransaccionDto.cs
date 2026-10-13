namespace TP5_Servicios_API_REST.DTOs;

// --- DTOs para TRANSACCIONES (Auditoría / Historial) ---

// Para crear o registrar una Transacción
public class TransaccionDto
{
    public string Tipo { get; set; } = string.Empty; // "Ingreso" o "Salida"
    public int UsuarioId { get; set; }
    public int? IngresoId { get; set; } // Nullable si es una salida
    public int? SalidaId { get; set; }  // Nullable si es un ingreso
}

// Para devolver la información de Transacciones en los GET
public class TransaccionReadDto
{
    public int Id { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int UsuarioId { get; set; }
    public int? IngresoId { get; set; }
    public int? SalidaId { get; set; }
}


// --- DTOs para INGRESOS (Compras a Proveedores en IngresosController) ---

public class RegistrarIngresoDto
{
    public int ProveedorId { get; set; }
    public int UsuarioId { get; set; }
    public List<ItemIngresoDto> Detalles { get; set; } = new();
}

public class ItemIngresoDto
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioCosto { get; set; }
}


// --- DTOs para SALIDAS (Ventas a Clientes en SalidasController) ---

public class RegistrarSalidaDto
{
    public int ClienteId { get; set; }
    public int UsuarioId { get; set; }
    public List<ItemSalidaDto> Detalles { get; set; } = new();
}

public class ItemSalidaDto
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioVenta { get; set; }
}