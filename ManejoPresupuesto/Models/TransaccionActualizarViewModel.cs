﻿namespace ManejoPresupuesto.Models
{
    public class TransaccionActualizarViewModel: TransaccionCreacionViewModel
    {
        public int CuentaAnteriorId { get; set; }
        public decimal MontoAnterior { get; set; }

    }
}
