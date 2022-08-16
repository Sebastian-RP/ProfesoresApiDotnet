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
    public class LessonController : ControllerBase
    {
        private readonly string cadenaSQL;

        public LessonController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("lecciones")]
        public IActionResult Lista()
        {
            List<Lesson> lista = new List<Lesson>();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_lista_lecciones", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Lesson
                            {
                                Id = Convert.ToInt32(rd["Id"]),
                                LessonDate = rd["LessonDate"].ToString(),
                                DurationLesson = Convert.ToInt32(rd["DurationLesson"]),
                                InstructorId = Convert.ToInt32(rd["InstructorId"])
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

        [HttpGet]
        //[Route("Obtener")] // => Obtener?idProducto=13 | ampersand  //obtener todas las clases de un mismo instructor
        [Route("InstructorLecciones/{InstructorId:int}")]
        public IActionResult Obtener(int InstructorId)
        {

            List<Lesson> lista = new List<Lesson>();
            List<Lesson> olesson = new();

            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_lista_lecciones", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {

                        while (rd.Read())
                        {
                            lista.Add(new Lesson
                            {
                                Id = Convert.ToInt32(rd["Id"]),
                                LessonDate = rd["LessonDate"].ToString(),
                                DurationLesson = Convert.ToInt32(rd["DurationLesson"]),
                                InstructorId = Convert.ToInt32(rd["InstructorId"])
                            });
                        }

                    }
                }

                olesson = lista.FindAll(item => item.InstructorId == InstructorId).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = olesson });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = olesson });

            }
        }
    }
}
