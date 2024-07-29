using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Controllers
{
    public class Transacciones: Controller
    {
        private readonly IServiciosUsuarios serviciosUsuarios;
        private readonly IRepositorioCuentas repositorioCuentas;

        public Transacciones(IServiciosUsuarios serviciosUsuarios, IRepositorioCuentas repositorioCuentas)
        {
            this.serviciosUsuarios = serviciosUsuarios;
            this.repositorioCuentas = repositorioCuentas;
        }

        public async Task<IActionResult> Crear()
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var modelo = new TransaccionCreacionViewModel();

            modelo.Cuentas = await ObtenerCuentas(usuarioId);
            return View(modelo);

        }

        private async Task<IEnumerable<SelectListItem>> ObtenerCuentas(int usuarioId)
        {
            var cuentas = await repositorioCuentas.Buscar(usuarioId);

            return cuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }
    }
}
