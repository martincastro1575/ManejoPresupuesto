﻿using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Servicios
{
    public interface IServiciosReportes
    {
        Task<IEnumerable<ResultadoObtenerPorSemana>> ObtenerReporteSemanal(int usuarioId, int mes, int anio, dynamic ViewBag);
        Task<ReporteTransaccionesDetalladas> ObtenerReporteTransaccionesDetalladas(int usuarioId, int mes, int anio, dynamic ViewBag);
        Task<ReporteTransaccionesDetalladas> ObtenerReporteTransaccionesDetalladasPorCuentas(int usuarioId, int CuentaId, int mes, int anio, dynamic ViewBag);
    }
    public class ServicioReportes: IServiciosReportes
    {
        private readonly IRepositorioTransacciones repositorioTransacciones;
        private readonly HttpContext httpContext;

        public ServicioReportes(IRepositorioTransacciones repositorioTransacciones, IHttpContextAccessor httpContextAccessor)
        {
            this.repositorioTransacciones = repositorioTransacciones;
            this.httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<IEnumerable<ResultadoObtenerPorSemana>> ObtenerReporteSemanal(int usuarioId, int mes, int anio, dynamic ViewBag)
        {
            (DateTime fechaInicio, DateTime fechaFin) = GenerarFechaInicioYFin(mes, anio);

            var parametro = new ParametroObtenerTransaccionesPorUsuario()
            {
                UsuarioId = usuarioId,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin

            };

            AsignarValoresAlViewBag(ViewBag, fechaInicio);
            var modelo = await repositorioTransacciones.ObtenerPorSemana(parametro);

            return modelo;
        }

        public async Task<ReporteTransaccionesDetalladas> ObtenerReporteTransaccionesDetalladas(int usuarioId, int mes, int anio, dynamic ViewBag)
        {
            (DateTime fechaInicio, DateTime fechaFin) = GenerarFechaInicioYFin(mes, anio);

            var parametro = new ParametroObtenerTransaccionesPorUsuario()
            {
                UsuarioId = usuarioId,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin

            };

            var transacciones = await repositorioTransacciones.ObtenerPorUsuarioId(parametro);

            var modelo = GenerarReporteTransaccionesDetalladas(fechaInicio, fechaFin, transacciones);

            AsignarValoresAlViewBag(ViewBag, fechaInicio);

            return modelo;


        }

        public async Task<ReporteTransaccionesDetalladas> 
            ObtenerReporteTransaccionesDetalladasPorCuentas(int usuarioId, int CuentaId, int mes, int anio, dynamic ViewBag)
        {
            (DateTime fechaInicio, DateTime fechaFin) = GenerarFechaInicioYFin(mes, anio);


            var obtenerTransaccionesPorCuenta = new ObtenerTransaccionesPorCuenta()
            {
                CuentaId = CuentaId,
                UsuarioId = usuarioId,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin
            };

            var transacciones = await repositorioTransacciones.ObtenerPorCuentaId(obtenerTransaccionesPorCuenta);
            var modelo = GenerarReporteTransaccionesDetalladas(fechaInicio, fechaFin, transacciones);

            AsignarValoresAlViewBag(ViewBag, fechaInicio);

            return modelo;

        }

        private void AsignarValoresAlViewBag(dynamic ViewBag, DateTime fechaInicio)
        {
            ViewBag.mesAnterior = fechaInicio.AddMonths(-1).Month;
            ViewBag.anioAnterior = fechaInicio.AddMonths(-1).Year;

            ViewBag.mesPosterior = fechaInicio.AddMonths(1).Month;
            ViewBag.anioPosterior = fechaInicio.AddMonths(1).Year;

            ViewBag.urlRetorno = httpContext.Request.Path + httpContext.Request.QueryString;
        }

        private static ReporteTransaccionesDetalladas GenerarReporteTransaccionesDetalladas(DateTime fechaInicio, DateTime fechaFin, IEnumerable<Transaccion> transacciones)
        {
            var modelo = new ReporteTransaccionesDetalladas();


            var transaccionesPorFecha = transacciones.OrderByDescending(x => x.FechaTransaccion)
                  .GroupBy(x => x.FechaTransaccion)
                  .Select(grupo => new ReporteTransaccionesDetalladas.TransaccionesPorFecha()
                  {
                      FechaTransaccion = grupo.Key,
                      Transacciones = grupo.AsEnumerable()
                  });

            modelo.TransaccionesAgrupadas = transaccionesPorFecha;
            modelo.Fechainicio = fechaInicio;
            modelo.FechaFin = fechaFin;
            return modelo;
        }

        private (DateTime fechaInicio, DateTime fechaFin) GenerarFechaInicioYFin(int mes, int anio)
        {
            DateTime fechaInicio;
            DateTime fechaFin;

            if (mes <= 0 || mes > 12 || anio <= 1900)
            {
                var hoy = DateTime.Today;
                fechaInicio = new DateTime(hoy.Year, hoy.Month, 1);
            }
            else
            {
                fechaInicio = new DateTime(anio, mes, 1);
            }

            fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

            return (fechaInicio, fechaFin);
        }
    }
}
