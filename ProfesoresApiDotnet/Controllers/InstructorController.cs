using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ProfesoresApiDotnet.Models;

using System.Data;
using System.Data.SqlClient;

namespace ProfesoresApiDotnet.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly string cadenaSQL;

        public InstructorController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Instructor> lista = new List<Instructor>();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_lista_instructores", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Instructor
                            {
                                Id = Convert.ToInt32(rd["Id"]),
                                Name = rd["Name"].ToString(),
                                DateBirth = (rd["DateBirth"].ToString()),
                                TypeInstructor = rd["TypeInstructor"].ToString(),
                                TypeCurrency = rd["TypeCurency"].ToString(),
                                PriceHour = (float)Convert.ToDouble(rd["PriceHour"])
                            });
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista });
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Instructor contenedor)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_guardar_instructor", conexion);
                    cmd.Parameters.AddWithValue("id", contenedor.Id);
                    cmd.Parameters.AddWithValue("name", contenedor.Name);
                    cmd.Parameters.AddWithValue("dateBirth", contenedor.DateBirth);
                    cmd.Parameters.AddWithValue("tipeInstructor", contenedor.TypeInstructor);
                    cmd.Parameters.AddWithValue("tipeCurrency", contenedor.TypeCurrency);
                    cmd.Parameters.AddWithValue("priceHour", contenedor.PriceHour);
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Instructor agregado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }
}
