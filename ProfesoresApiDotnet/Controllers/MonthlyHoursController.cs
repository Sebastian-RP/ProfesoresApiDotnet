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
    public class MonthlyHoursController : ControllerBase
    {
        private readonly string cadenaSQL;

        public MonthlyHoursController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("lista")]
        public IActionResult Lista()
        {
            List<monthlyHours> lista = new List<monthlyHours>();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_lista_instructor_mensualidad", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while(rd.Read())
                        {
                            lista.Add(new monthlyHours
                            {
                                Id = Convert.ToInt32(rd["Id"]),
                                Name = rd["Name"].ToString(),
                                DateBirth = (rd["DateBirth"].ToString()),
                                TypeInstructor = rd["TypeInstructor"].ToString(),
                                TypeCurrency = rd["TypeCurency"].ToString(),
                                PriceHour = (float)Convert.ToDouble(rd["PriceHour"]),
                                DurationLesson = (float)Convert.ToDouble(rd["DurationLesson"])
                            });
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch(Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista });
            }
        }
    }
}
