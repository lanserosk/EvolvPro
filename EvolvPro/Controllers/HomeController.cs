using EvolvPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EvolvPro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        

        public IActionResult Index()
        {
            EvolvProContext evolv = new EvolvProContext();
            evolv.SaveChangesAndNotify();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Recu_Contra()
        {
            return View();
        }
        public IActionResult DetEstado()
        {
            return View();
        }

        public IActionResult Usuarios()
        {
            ViewBag.sesion = HttpContext.Session.GetString("VarSesion1");
            return View();
        }
        public IActionResult Menu()
        {
            ViewBag.sesion = HttpContext.Session.GetString("VarSesion1");
            return View();
        }

        [HttpPost]
        public IActionResult Login(string correo, string contra)
        {
            EvolvProContext _DbContext = new EvolvProContext();
            var buscarNombre = _DbContext.Usuarios.FirstOrDefault(x => x.CorreoUsu == correo);


            if (buscarNombre != null)
            {
                if (buscarNombre != null && buscarNombre.ContrasenaUsu == contra)
                {
                    string nivel = buscarNombre.FkTipousu.ToString();
                    //TempData["NivelUsu"] = nivel; 
                    HttpContext.Session.SetString("VarSesion1", nivel);
                    return RedirectToAction("Menu", "Home");

                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            TempData["Resultado"] = "Intente de nuevo";
            return View();
        }

        [HttpPost]
        public IActionResult mostrarDestalleEst()
        {
            EvolvProContext contexto = new EvolvProContext();
            var query = from detalleEstado in contexto.DetalleEstados
                        join estado in contexto.Estados on detalleEstado.FkEstado equals estado.IdEstado
                        select new
                        {
                            detalleEstado.IdDetalleestado,
                            detalleEstado.ValorDestado,
                            estado.Nombre
                        };
            List<Object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }
            return Json(resultado);
        }

        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //MANTENIMIENTO USUARIOS
        [HttpPost]
        public IActionResult mostrarUsuarios()
        {
            EvolvProContext contexto = new EvolvProContext();
            var query = from usu in contexto.Usuarios
                        join tusu in contexto.TipoUsuarios on usu.FkTipousu equals tusu.IdTipousu
                        join preUsu in contexto.PreguntaSeguridads on usu.FkPregunta equals preUsu.IdPregunta
                        select new
                        {
                            idUsuario = usu.IdUsuario,
                            nombreUsu = usu.NombreUsu,
                            telefonoUsu = usu.TelefonoUsu,
                            correoUsu = usu.CorreoUsu,
                            preguntaUsu = preUsu.DescPregunta,
                            respuestaUsu = usu.RespuestaUsu,
                            fkEstadousu = usu.FkEstadousu,
                            fkTipousu = tusu.NombreTipo
                        };
            List<Object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }
            return Json(resultado);
        }
        [HttpPost]
        public ActionResult guardarUsuario(Usuario usu)
        {
            EvolvProContext contexto = new EvolvProContext();
            Usuario obj = new Usuario();
            obj.IdUsuario = usu.IdUsuario;
            obj.NombreUsu = usu.NombreUsu;
            obj.TelefonoUsu = usu.TelefonoUsu;
            obj.CorreoUsu = usu.CorreoUsu;
            obj.ContrasenaUsu = usu.ContrasenaUsu;
            obj.RespuestaUsu = usu.RespuestaUsu;

            obj.FkEstadousu = usu.FkEstadousu;

            obj.FkTipousu = usu.FkTipousu;
            obj.FkPregunta = usu.FkPregunta;
            //CAPTURAMOS ALGUN POSIBLE ERROR
            try
            {
                if (obj.IdUsuario == 0 || obj.IdUsuario == null)
                {
                    contexto.Usuarios.Add(obj);
                    
                }
                else
                {
                    contexto.Entry(usu).State = EntityState.Modified;
                    
                }
                contexto.SaveChanges();
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
        [HttpPost]
        public IActionResult EditarUsuario(int id)
        {
            EvolvProContext contexto = new EvolvProContext();
            Usuario res = new Usuario();
            var objEdit = contexto.Usuarios.FirstOrDefault(x => x.IdUsuario == id);


            res.IdUsuario = objEdit.IdUsuario;
            res.NombreUsu = objEdit.NombreUsu;
            res.TelefonoUsu = objEdit.TelefonoUsu;
            res.CorreoUsu = objEdit.CorreoUsu;
            res.FkPregunta = objEdit.FkPregunta;
            res.RespuestaUsu = objEdit.RespuestaUsu;
            res.FkEstadousu = objEdit.FkEstadousu;
            res.FkTipousu = objEdit.FkTipousu;

            return Json(res);
        }

        [HttpPost]
        public IActionResult EliminarUsuario(int id)
        {
            try
            {
                EvolvProContext contexto = new EvolvProContext();
                var objDel = contexto.Usuarios.FirstOrDefault(x => x.IdUsuario == id);
                contexto.Usuarios.Remove(objDel);
                contexto.SaveChanges();
                return Json(true);
            }

            catch (Exception ex)
            {
                return Json(false);
            }

        }
        [HttpPost]
        public IActionResult EliminarDetEstado(int id)
        {
            try
            {
                EvolvProContext contexto = new EvolvProContext();
                var objDel = contexto.DetalleEstados.FirstOrDefault(x => x.IdDetalleestado == id);
                contexto.DetalleEstados.Remove(objDel);
                contexto.SaveChanges();
                return Json(true);
            }

            catch (Exception ex)
            {
                return Json(false);
            }

        }

        public IActionResult listPreguntas()
        {
            EvolvProContext contexto = new EvolvProContext();
            List<PreguntaSeguridad> preguntasSeguridad = contexto.PreguntaSeguridads.ToList();
            return Json(preguntasSeguridad);
        }
        public IActionResult listTUsuario()
        {
            EvolvProContext contexto = new EvolvProContext();
            List<TipoUsuario> tipoUsuario = contexto.TipoUsuarios.ToList();
            return Json(tipoUsuario);
        }
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

        public IActionResult listEstado()
        {
            EvolvProContext contexto = new EvolvProContext();
            List<Estado> estados = contexto.Estados.ToList();
            return Json(estados);
        }
        public IActionResult cargarFormEstado(int IdDetalleestado)
        {
            EvolvProContext contexto = new EvolvProContext();
            var query = from detalleEstado in contexto.DetalleEstados
                        join estado in contexto.Estados on detalleEstado.FkEstado equals estado.IdEstado
                        where detalleEstado.IdDetalleestado == IdDetalleestado
                        select new
                        {
                            detalleEstado.IdDetalleestado,
                            detalleEstado.ValorDestado,
                            estado.Nombre
                        };
            return Json(query);
        }

        [HttpPost]
        public ActionResult guardarDetEstado(DetalleEstado det)
        {
            EvolvProContext contexto = new EvolvProContext();
            DetalleEstado obj = new DetalleEstado();
            obj.ValorDestado = det.ValorDestado;

            obj.FkEstado = det.FkEstado;
            //CAPTURAMOS ALGUN POSIBLE ERROR
            try
            {
                
                    contexto.DetalleEstados.Add(obj);
                    contexto.SaveChanges();


                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        //MANTENIMIENTO CLIENTES:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        public IActionResult Clientes()
        {
            ViewBag.sesion = HttpContext.Session.GetString("VarSesion1");
            return View();
        }

        [HttpPost]
        public ActionResult guardarClientes(Cliente cli)
        {
            EvolvProContext contexto = new EvolvProContext();
            Cliente obj = new Cliente();
            obj.IdCliente = cli.IdCliente;
            obj.NombreCliente = cli.NombreCliente;
            obj.DireccionCliente = cli.DireccionCliente;

            //CAPTURAMOS ALGUN POSIBLE ERROR
            try
            {
                if (obj.IdCliente == 0 || obj.IdCliente == null)
                {
                    contexto.Clientes.Add(obj);
                }
                else
                {
                    contexto.Entry(cli).State = EntityState.Modified;
                }
                contexto.SaveChanges();
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public IActionResult mostrarClientes()
        {
            EvolvProContext contexto = new EvolvProContext();
            var query = from usu in contexto.Clientes
                            //join tusu in contexto.TipoUsuarios on usu.FkTipousu equals tusu.IdTipousu
                            //join preUsu in contexto.PreguntaSeguridads on usu.FkPregunta equals preUsu.IdPregunta
                        select new
                        {
                            idCliente = usu.IdCliente,
                            nombreCli = usu.NombreCliente,
                            direccionCli = usu.DireccionCliente
                        };
            List<Object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }
            return Json(resultado);
        }

        [HttpPost]
        public IActionResult EditarClientes(int id)
        {
            EvolvProContext contexto = new EvolvProContext();
            Cliente res = new Cliente();
            var objEdit = contexto.Clientes.FirstOrDefault(x => x.IdCliente == id);

            res.IdCliente = objEdit.IdCliente;
            res.NombreCliente = objEdit.NombreCliente;
            res.DireccionCliente = objEdit.DireccionCliente;

            return Json(res);
        }

        [HttpPost]
        public IActionResult EliminarClientes(int id)
        {
            try
            {
                EvolvProContext contexto = new EvolvProContext();
                var objDel = contexto.Clientes.FirstOrDefault(x => x.IdCliente == id);
                contexto.Clientes.Remove(objDel);
                contexto.SaveChanges();
                return Json(true);
            }

            catch (Exception ex)
            {
                return Json(false);
            }

        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}