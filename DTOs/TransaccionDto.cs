namespace TP5_Servicios_API_REST.DTOs;

// --- DTOs para INGRESOS (Compras a Proveedores) ---
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

// --- DTOs para SALIDAS (Ventas a Clientes) ---
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