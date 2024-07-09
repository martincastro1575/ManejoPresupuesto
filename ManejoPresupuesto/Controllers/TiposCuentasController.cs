using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController: Controller

    {
        
        public TiposCuentasController()
        {
            
        }
        public IActionResult Crear() 
        { 
            
            return View();
        
        }

        [HttpPost]
        public IActionResult Crear(TipoCuenta tipocuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipocuenta);
            }

            return View();
        }
    }


}
