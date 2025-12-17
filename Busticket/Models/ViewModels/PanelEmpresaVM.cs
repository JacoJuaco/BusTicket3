using Busticket.Models;

namespace Busticket.Models.ViewModels
{
    public class PanelEmpresaVM
    {
        public Empresa Empresa { get; set; }
        public List<Venta> Ventas { get; set; }
    }
}
