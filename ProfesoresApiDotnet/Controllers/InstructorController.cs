using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ProfesoresApiDotnet.Models;

using System.Data;
using System.Data.SqlClient;

namespace ProfesoresApiDotnet.Controllers
{
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
                                DateBirth = Convert.ToDateTime(rd["DateBirth"]),
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
    }
}
