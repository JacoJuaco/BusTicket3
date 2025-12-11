namespace Busticket.Models
{
    public class RutaDetalleViewModel
    {
        public Ruta Ruta { get; set; }

        public List<Asiento> Asientos { get; set; } = new List<Asiento>();

        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
