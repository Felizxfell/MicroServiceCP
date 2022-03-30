using MicroServiceCP.Data;
using MicroServiceCP.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceCP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesPagosController : Controller
    {
        private readonly TablaClientePagosAnterior tabla;

        /// <summary>
        /// Cargamos el objeto que nos da acceso a los metodos de nuestra tabla
        /// </summary>
        /// <param name="table"></param>
        public ClientesPagosController(TablaClientePagosAnterior table)
        {
            tabla = table;
        }

        [HttpGet("insertaPagosAnterior")]
        public async Task<IActionResult> CreateCliente()
        {
            var datenow = DateTime.Now;
            var dia = (int)datenow.DayOfWeek;
            var hora = $"{datenow.Hour}:{datenow.Minute}";
            var diaAnterior = datenow.AddDays(-1).ToShortDateString();

            if (dia <= 1 && dia >= 5)
                return BadRequest("No se encuentra en un dia de la semana valido");
            if (!hora.Equals("8:25"))
                return BadRequest("No se encuentra en un horario valido");

            var created = await tabla.InsertPagoAnterior(diaAnterior);

            return Created("created", created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClientes()
        {
            return Ok(await tabla.GetAllPaymentsAnterior());
        }
    }
}
