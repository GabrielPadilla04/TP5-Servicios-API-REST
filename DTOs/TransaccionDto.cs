namespace TP5_Servicios_API_REST.DTOs;

// --- DTO para Transacciones Simples (Ajustes de stock en TransaccionesController) ---
public class TransaccionDto
{
    public int ProductoId { get; set; }
    public string Tipo { get; set; } = string.Empty; // "Entrada" o "Salida"
    public int Cantidad { get; set; }
}

// --- DTOs para INGRESOS (Compras a Proveedores en IngresosController) ---
public class RegistrarIngresoDto
{
    public int ProveedorId { get; set; }
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
    public List<ItemSalidaDto> Detalles { get; set; } = new();
}

public class ItemSalidaDto
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioVenta { get; set; }
}