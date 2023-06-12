using EvolvPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using EFCore.BulkExtensions;
using System.Diagnostics.Contracts;

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

            return View();
        }

        public IActionResult Privacy()
        {

            return View();
        }
        public IActionResult Reuniones()
        {
            ViewBag.sesion = HttpContext.Session.GetString("VarSesion1");
            return View();
        }
        public IActionResult Recu_Contra()
        {
            ViewBag.sesion = HttpContext.Session.GetString("VarSesion1");
            return View();
        }
        public IActionResult DetEstado()
        {
            ViewBag.sesion = HttpContext.Session.GetString("VarSesion1");
            return View();
        }

        public IActionResult Proyectos()
        {
            ViewBag.sesion = HttpContext.Session.GetString("VarSesion1");
            ViewBag.idusu = HttpContext.Session.GetInt32("VarSesionIdUsu");
            return View();
        }
        public IActionResult Cronograma()
        {
            ViewBag.sesion = HttpContext.Session.GetString("VarSesion1");
            return View();
        }
        public IActionResult Estado()
        {
            ViewBag.sesion = HttpContext.Session.GetString("VarSesion1");
            return View();
        }
        public IActionResult CategoriaIssue()
        {
            ViewBag.sesion = HttpContext.Session.GetString("VarSesion1");
            return View();
        }
        public IActionResult PreguntaSeguridad()
        {
            ViewBag.sesion = HttpContext.Session.GetString("VarSesion1");
            return View();
        }
        public IActionResult Recurso()
        {
            ViewBag.sesion = HttpContext.Session.GetString("VarSesion1");
            return View();
        }
        public IActionResult Rolhora()
        {
            ViewBag.sesion = HttpContext.Session.GetString("VarSesion1");
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
                    int idusu = Convert.ToInt32(buscarNombre.IdUsuario.ToString());
                    //TempData["NivelUsu"] = nivel; 
                    HttpContext.Session.SetString("VarSesion1", nivel);
                    HttpContext.Session.SetInt32("VarSesionIdUsu", idusu);
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
        public IActionResult RecupContra(Usuario usu)
        {
            EvolvProContext _DbContext = new EvolvProContext();
            var obj = _DbContext.Usuarios.FirstOrDefault(x => x.CorreoUsu == usu.CorreoUsu);
            if (obj != null)
            {


                if (usu.FkPregunta == obj.FkPregunta && usu.RespuestaUsu == obj.RespuestaUsu && usu.CorreoUsu == obj.CorreoUsu)
                {
                    obj.ContrasenaUsu = usu.ContrasenaUsu;

                    _DbContext.Entry(obj).State = EntityState.Modified;

                    _DbContext.SaveChanges();

                    var msj = "Cambio de contraseña exitoso!!!";
                    return Json(msj);
                }
                else
                {
                    var msj = "Error de validación, intenta de nuevo!!!";
                    return Json(msj);
                }
                return Json(true);
            }
            else
            {
                var msj = "No se encontrarón registros para el usuario: " + usu.CorreoUsu;
                return Json(msj);
            }
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

        [HttpPost]
        public IActionResult EditarDetEstado(int id)
        {
            EvolvProContext contexto = new EvolvProContext();
            DetalleEstado res = new DetalleEstado();
            var objEdit = contexto.DetalleEstados.FirstOrDefault(x => x.IdDetalleestado == id);


            res.IdDetalleestado = objEdit.IdDetalleestado;
            res.ValorDestado = objEdit.ValorDestado;
            res.FkEstado = objEdit.FkEstado;

            return Json(res);
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
        public IActionResult listDetEstado()
        {
            EvolvProContext contexto = new EvolvProContext();
            List<DetalleEstado> detestados = contexto.DetalleEstados.Where(d => d.FkEstado == 2).ToList();
            return Json(detestados);
        }
        public IActionResult listCliente()
        {
            EvolvProContext contexto = new EvolvProContext();
            List<Cliente> clientes = contexto.Clientes.ToList();
            return Json(clientes);
        }
        public IActionResult listUsuario()
        {
            EvolvProContext contexto = new EvolvProContext();
            List<Usuario> usu = contexto.Usuarios.Where(d => d.FkTipousu == 2).ToList();
            return Json(usu);
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
            obj.IdDetalleestado = det.IdDetalleestado;
            obj.ValorDestado = det.ValorDestado;

            obj.FkEstado = det.FkEstado;
            //CAPTURAMOS ALGUN POSIBLE ERROR
            try
            {
                if(obj.IdDetalleestado == 0 || obj.IdDetalleestado == null)
                {
                    contexto.DetalleEstados.Add(obj);
                }
                else
                {
                    contexto.Entry(det).State = EntityState.Modified;
                }
                    
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


        //::::::::::::::PROYECTOS:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

        [HttpPost]
        public IActionResult mostrarProyectos()
        {
            EvolvProContext contexto = new EvolvProContext();
            var query = from proyecto in contexto.Proyectos
                        join cliente in contexto.Clientes on proyecto.FkCliente equals cliente.IdCliente
                        join estado in contexto.DetalleEstados on proyecto.FkEstado equals estado.IdDetalleestado
                        join usuario in contexto.Usuarios on proyecto.FkUsuario equals usuario.IdUsuario
                        orderby proyecto.IdProyecto descending
                        select new
                        {
                            proyecto.IdProyecto,
                            proyecto.NombrePry,
                            proyecto.CasoNegocio,
                            proyecto.HorasTotales,
                            proyecto.HorasTotalesreal,
                            proyecto.Interesados,
                            proyecto.FechaInicio,
                            proyecto.FechaFinalProp,
                            proyecto.FechaFinalReal,
                            ClienteNombre = cliente.NombreCliente,
                            EstadoNombre = estado.ValorDestado,
                            UsuarioNombre = usuario.NombreUsu
                        };

            List<Object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }
            return Json(resultado);
        }
        [HttpPost]
        public IActionResult mostrarProyectosPM(int id)
        {
            EvolvProContext contexto = new EvolvProContext();
            var query = from proyecto in contexto.Proyectos
                        join cliente in contexto.Clientes on proyecto.FkCliente equals cliente.IdCliente
                        join estado in contexto.DetalleEstados on proyecto.FkEstado equals estado.IdDetalleestado
                        join usuario in contexto.Usuarios on proyecto.FkUsuario equals usuario.IdUsuario
                        where proyecto.FkUsuario == id
                        orderby proyecto.IdProyecto descending
                        select new
                        {
                            proyecto.IdProyecto,
                            proyecto.NombrePry,
                            proyecto.CasoNegocio,
                            proyecto.HorasTotales,
                            proyecto.HorasTotalesreal,
                            proyecto.Interesados,
                            proyecto.FechaInicio,
                            proyecto.FechaFinalProp,
                            proyecto.FechaFinalReal,
                            ClienteNombre = cliente.NombreCliente,
                            EstadoNombre = estado.ValorDestado,
                            UsuarioNombre = usuario.NombreUsu
                        };

            List<Object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }
            return Json(resultado);
        }
        [HttpPost]
        public IActionResult mostrarProyectosBusca(string cadena)
        {
            EvolvProContext contexto = new EvolvProContext();
            var query = from proyecto in contexto.Proyectos
                        join cliente in contexto.Clientes on proyecto.FkCliente equals cliente.IdCliente
                        join estado in contexto.DetalleEstados on proyecto.FkEstado equals estado.IdDetalleestado
                        join usuario in contexto.Usuarios on proyecto.FkUsuario equals usuario.IdUsuario
                        where proyecto.NombrePry.Contains(cadena)
                        orderby proyecto.IdProyecto descending
                        select new
                        {
                            proyecto.IdProyecto,
                            proyecto.NombrePry,
                            proyecto.CasoNegocio,
                            proyecto.HorasTotales,
                            proyecto.HorasTotalesreal,
                            proyecto.Interesados,
                            proyecto.FechaInicio,
                            proyecto.FechaFinalProp,
                            proyecto.FechaFinalReal,
                            ClienteNombre = cliente.NombreCliente,
                            EstadoNombre = estado.ValorDestado,
                            UsuarioNombre = usuario.NombreUsu
                        };


            List<Object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }
            return Json(resultado);
        }
        [HttpPost]
        public IActionResult mostrarProyectosBuscaPM(string cadena, int id)
        {
            EvolvProContext contexto = new EvolvProContext();
            var query = from proyecto in contexto.Proyectos
                        join cliente in contexto.Clientes on proyecto.FkCliente equals cliente.IdCliente
                        join estado in contexto.DetalleEstados on proyecto.FkEstado equals estado.IdDetalleestado
                        join usuario in contexto.Usuarios on proyecto.FkUsuario equals usuario.IdUsuario
                        where proyecto.NombrePry.Contains(cadena) && proyecto.FkUsuario == id
                        orderby proyecto.IdProyecto descending
                        select new
                        {
                            proyecto.IdProyecto,
                            proyecto.NombrePry,
                            proyecto.CasoNegocio,
                            proyecto.HorasTotales,
                            proyecto.HorasTotalesreal,
                            proyecto.Interesados,
                            proyecto.FechaInicio,
                            proyecto.FechaFinalProp,
                            proyecto.FechaFinalReal,
                            ClienteNombre = cliente.NombreCliente,
                            EstadoNombre = estado.ValorDestado,
                            UsuarioNombre = usuario.NombreUsu
                        };


            List<Object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }
            return Json(resultado);
        }

        [HttpPost]
        public ActionResult guardarProyecto(Proyecto pry)
        {
            EvolvProContext contexto = new EvolvProContext();
            Proyecto obj = new Proyecto();
            obj.IdProyecto = pry.IdProyecto;
            obj.NombrePry = pry.NombrePry;
            obj.CasoNegocio = pry.CasoNegocio;
            obj.HorasTotales = pry.HorasTotales;
            obj.HorasTotalesreal = null;
            obj.Interesados = pry.Interesados;
            obj.FechaInicio = pry.FechaInicio;
            obj.FechaFinalProp = pry.FechaFinalProp;
            obj.FechaFinalReal = null;
            obj.FkCliente = pry.FkCliente;
            obj.FkUsuario = pry.FkUsuario;
            obj.FkEstado = pry.FkEstado;
            //CAPTURAMOS ALGUN POSIBLE ERROR
            try
            {
                if (obj.IdProyecto == 0 || obj.IdProyecto == null)
                {
                    contexto.Proyectos.Add(obj);
                }
                else
                {
                    contexto.Entry(pry).State = EntityState.Modified;
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
        public IActionResult EditarProyectos(int id)
        {
            EvolvProContext contexto = new EvolvProContext();
            Proyecto res = new Proyecto();
            var objEdit = contexto.Proyectos.FirstOrDefault(x => x.IdProyecto == id);

            res.IdProyecto = objEdit.IdProyecto;
            res.NombrePry = objEdit.NombrePry;
            res.CasoNegocio = objEdit.CasoNegocio;
            res.HorasTotales = objEdit.HorasTotales;
            res.Interesados = objEdit.Interesados;
            res.FechaInicio = objEdit.FechaInicio;
            res.FechaFinalProp = objEdit.FechaFinalProp;
            res.FkCliente = objEdit.FkCliente;
            res.FkEstado = objEdit.FkEstado;
            res.FkUsuario = objEdit.FkUsuario;

            return Json(res);
        }

        [HttpPost]
        public IActionResult DetallesProyectos(int id)
        {
            EvolvProContext contexto = new EvolvProContext();
            var query = from proyecto in contexto.Proyectos
                        join cliente in contexto.Clientes on proyecto.FkCliente equals cliente.IdCliente
                        join estado in contexto.DetalleEstados on proyecto.FkEstado equals estado.IdDetalleestado
                        join usuario in contexto.Usuarios on proyecto.FkUsuario equals usuario.IdUsuario
                        where proyecto.IdProyecto == id // Filtrar por IdProyecto
                        select new
                        {
                            proyecto.IdProyecto,
                            proyecto.NombrePry,
                            proyecto.CasoNegocio,
                            proyecto.HorasTotales,
                            proyecto.HorasTotalesreal,
                            proyecto.Interesados,
                            proyecto.FechaInicio,
                            proyecto.FechaFinalProp,
                            proyecto.FechaFinalReal,
                            ClienteNombre = cliente.NombreCliente,
                            EstadoNombre = estado.ValorDestado,
                            UsuarioNombre = usuario.NombreUsu
                        };

            List<Object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }
            return Json(resultado);
        }



        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

        //::::::::::::::::::::::::Cronograma::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        public IActionResult listProyectos()
        {
            EvolvProContext contexto = new EvolvProContext();
            List<Proyecto> prys = contexto.Proyectos.ToList();
            return Json(prys);
        }
        public IActionResult listRolHora()
        {
            EvolvProContext contexto = new EvolvProContext();
            List<RolHora> prys = contexto.RolHoras.ToList();
            return Json(prys);
        }
        public IActionResult listDetEstadoact()
        {
            EvolvProContext contexto = new EvolvProContext();
            List<DetalleEstado> detestados = contexto.DetalleEstados.Where(d => d.FkEstado == 3).ToList();
            return Json(detestados);
        }
        public IActionResult listDetEstadoIssue()
        {
            EvolvProContext contexto = new EvolvProContext();
            List<DetalleEstado> detestados = contexto.DetalleEstados.Where(d => d.FkEstado == 4).ToList();
            return Json(detestados);
        }
        public IActionResult listRecurso()
        {
            EvolvProContext contexto = new EvolvProContext();
            List<Recurso> detestados = contexto.Recursos.ToList();
            return Json(detestados);
        }

        public IActionResult listCategoria()
        {
            EvolvProContext contexto = new EvolvProContext();
            List<CategoriaIssue> cat = contexto.CategoriaIssues.ToList();
            return Json(cat);
        }

        [HttpPost]
        public IActionResult mostrarActividadesBusca(int cadena)
        {
            EvolvProContext contexto = new EvolvProContext();
            var query = from crg in contexto.Cronogramas
                        join prj in contexto.Proyectos on crg.FkProyecto equals prj.IdProyecto
                        join dh in contexto.DetalleEstados on crg.FkEstado equals dh.IdDetalleestado
                        join rh in contexto.RolHoras on crg.FkRolhora equals rh.IdRolhora
                        join rc in contexto.Recursos on crg.FkRecurso equals rc.IdRecurso
                        where prj.IdProyecto == cadena
                        select new
                        {
                            crg.IdCronograma,
                            crg.NombreCrgm,
                            crg.DescripcionCrgm,
                            crg.HorasCrgm,
                            crg.Jerarquia,
                            Proyecto = prj.NombrePry,
                            Estado = dh.ValorDestado,
                            RolHora = rh.NombreRol,
                            Recurso = rc.NombreRec
                        };


            List<Object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }
            return Json(resultado);
        }

        [HttpPost]
        public IActionResult mostrarActividadesBusca2(int cadena, int id)
        {
            EvolvProContext contexto = new EvolvProContext();
            var query = from crg in contexto.Cronogramas
                        join prj in contexto.Proyectos on crg.FkProyecto equals prj.IdProyecto
                        join dh in contexto.DetalleEstados on crg.FkEstado equals dh.IdDetalleestado
                        join rh in contexto.RolHoras on crg.FkRolhora equals rh.IdRolhora
                        join rc in contexto.Recursos on crg.FkRecurso equals rc.IdRecurso
                        where prj.IdProyecto == cadena && crg.IdCronograma == id
                        select new
                        {
                            crg.IdCronograma,
                            crg.NombreCrgm,
                            crg.DescripcionCrgm,
                            crg.HorasCrgm,
                            crg.Jerarquia,
                            Proyecto = prj.NombrePry,
                            Estado = dh.ValorDestado,
                            RolHora = rh.NombreRol,
                            Recurso = rc.NombreRec
                        };


            List<Object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }
            return Json(resultado);
        }

        [HttpPost]
        public IActionResult mostrarActividadesBusca3(int cadena, string acti)
        {
            EvolvProContext contexto = new EvolvProContext();
            var query = from crg in contexto.Cronogramas
                        join prj in contexto.Proyectos on crg.FkProyecto equals prj.IdProyecto
                        join dh in contexto.DetalleEstados on crg.FkEstado equals dh.IdDetalleestado
                        join rh in contexto.RolHoras on crg.FkRolhora equals rh.IdRolhora
                        join rc in contexto.Recursos on crg.FkRecurso equals rc.IdRecurso
                        where prj.IdProyecto == cadena && crg.NombreCrgm.Contains(acti)
                        select new
                        {
                            crg.IdCronograma,
                            crg.NombreCrgm,
                            crg.DescripcionCrgm,
                            crg.HorasCrgm,
                            crg.Jerarquia,
                            Proyecto = prj.NombrePry,
                            Estado = dh.ValorDestado,
                            RolHora = rh.NombreRol,
                            Recurso = rc.NombreRec
                        };


            List<Object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }
            return Json(resultado);
        }

        [HttpPost]
        public IActionResult EditarActividad(int id)
        {
            EvolvProContext contexto = new EvolvProContext();
            Cronograma res = new Cronograma();
            var objEdit = contexto.Cronogramas.FirstOrDefault(x => x.IdCronograma == id);

            res.IdCronograma = objEdit.IdCronograma;
            res.NombreCrgm = objEdit.NombreCrgm;
            res.DescripcionCrgm = objEdit.DescripcionCrgm;
            res.HorasCrgm = objEdit.HorasCrgm;
            res.Jerarquia = objEdit.Jerarquia;
            res.FkProyecto = objEdit.FkProyecto;
            res.FkEstado = objEdit.FkEstado;
            res.FkRolhora = objEdit.FkRolhora;
            res.FkRecurso = objEdit.FkRecurso;

            //var comprobacion = contexto.Cronogramas.Where(x => x.FkProyecto == id);

            return Json(res);
        }

        [HttpPost]
        public ActionResult guardarActividad(Cronograma crono)
        {
            double horasupdate = 0;
            

            EvolvProContext contexto = new EvolvProContext();
            Cronograma obj = new Cronograma();
            obj.IdCronograma = crono.IdCronograma;
            obj.NombreCrgm = crono.NombreCrgm;
            obj.DescripcionCrgm= crono.DescripcionCrgm;
            obj.HorasCrgm = crono.HorasCrgm;
            obj.Jerarquia = crono.Jerarquia;
            obj.FkProyecto = crono.FkProyecto;
            obj.FkEstado = crono.FkEstado;
            obj.FkRolhora= crono.FkRolhora;
            obj.FkRecurso= crono.FkRecurso;
            //CAPTURAMOS ALGUN POSIBLE ERROR
            try
            {
                contexto.Entry(crono).State = EntityState.Modified;
                contexto.SaveChanges();


                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public ActionResult sumarHoras(int pry, double? horasBase, double horasNuevo)
        {
            if (horasBase.HasValue && !double.IsNaN(horasBase.Value))
            {
                if (horasBase < horasNuevo)
                {
                    decimal horasRest = (decimal)(horasNuevo - horasBase.Value);

                    EvolvProContext contexto = new EvolvProContext();
                    var crono = contexto.Proyectos.Where(x => x.IdProyecto == pry).FirstOrDefault();
                    decimal horasTotal = horasRest + (crono.HorasTotalesreal ?? 0);

                    // Actualizar el campo HorasTotalesReal
                    crono.HorasTotalesreal = horasTotal;

                    // Guardar los cambios en la base de datos
                    contexto.SaveChanges();

                    // Retornar un resultado exitoso (opcional)
                    return Json(true);
                }
                else if (horasBase > horasNuevo)
                {
                    decimal horasRest = (decimal)(horasBase.Value - horasNuevo);

                    EvolvProContext contexto = new EvolvProContext();
                    var crono = contexto.Proyectos.Where(x => x.IdProyecto == pry).FirstOrDefault();
                    decimal horasTotal = (crono.HorasTotalesreal ?? 0) - horasRest;

                    // Actualizar el campo HorasTotalesReal
                    crono.HorasTotalesreal = horasTotal;

                    // Guardar los cambios en la base de datos
                    contexto.SaveChanges();

                    // Retornar un resultado exitoso (opcional)
                    return Json(true);
                }
                else if (horasBase == horasNuevo)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            else
            {
                if (horasNuevo != null)
                {
                    decimal horasRest = (decimal)(horasNuevo);
                    EvolvProContext contexto = new EvolvProContext();
                    var crono = contexto.Proyectos.Where(x => x.IdProyecto == pry).FirstOrDefault();
                    decimal horasTotal = horasRest + (crono.HorasTotalesreal ?? 0);

                    // Actualizar el campo HorasTotalesReal
                    crono.HorasTotalesreal = horasTotal;

                    // Guardar los cambios en la base de datos
                    contexto.SaveChanges();

                    // Retornar un resultado exitoso (opcional)
                    return Json(true);
                }
                else
                {
                    decimal horasRest = 0;
                    EvolvProContext contexto = new EvolvProContext();
                    var crono = contexto.Proyectos.Where(x => x.IdProyecto == pry).FirstOrDefault();
                    decimal horasTotal = horasRest + (crono.HorasTotalesreal ?? 0);

                    // Actualizar el campo HorasTotalesReal
                    crono.HorasTotalesreal = horasTotal;

                    // Guardar los cambios en la base de datos
                    contexto.SaveChanges();

                    // Retornar un resultado exitoso (opcional)
                    return Json(true);
                }
                

                
            }

            // Retornar un resultado de error
            return Json(false);
        }


        [HttpPost]
        public IActionResult EnviarDatos([FromForm] IFormFile ArchivoExcel, int pry)
        {
            EvolvProContext contexto = new EvolvProContext();
            Stream stream = ArchivoExcel.OpenReadStream();

            IWorkbook MiExcel = null;

            if (Path.GetExtension(ArchivoExcel.FileName) == ".xlsx")
            {
                MiExcel = new XSSFWorkbook(stream);
            }
            else
            {
                MiExcel = new HSSFWorkbook(stream);
            }

            ISheet HojaExcel = MiExcel.GetSheetAt(0);

            int cantidadFilas = HojaExcel.LastRowNum;
            List<Cronograma> lista = new List<Cronograma>();
            var ids = contexto.Cronogramas
                    .Where(c => c.FkProyecto == pry).ToList();
            for (int i = 1; i <= cantidadFilas; i++)
            {

                IRow fila = HojaExcel.GetRow(i);

                string nombreCrgm = fila.GetCell(0).ToString();
                string descripcionCrgm = fila.GetCell(1).ToString();
                int jerarquia = Convert.ToInt32(fila.GetCell(2).ToString());
                int fkRolhora = Convert.ToInt32(fila.GetCell(3).ToString());
                int fkRecurso = Convert.ToInt32(fila.GetCell(4).ToString());

                Cronograma actividad = new Cronograma
                {
                    NombreCrgm = nombreCrgm,
                    DescripcionCrgm = descripcionCrgm,
                    Jerarquia = jerarquia,
                    FkProyecto = pry,
                    FkRolhora = fkRolhora,
                    FkRecurso = fkRecurso
                };

                // Verificar si la actividad ya existe en la base de datos
                var actividadExistente = contexto.Cronogramas
                    .FirstOrDefault(c => c.FkProyecto == pry && c.NombreCrgm == nombreCrgm);

                if (actividadExistente != null)
                {
                    actividad.IdCronograma = actividadExistente.IdCronograma;
                    actividad.HorasCrgm = actividadExistente.HorasCrgm;
                    actividad.FkEstado = actividadExistente.FkEstado;
                }
                else
                {
                    actividad.HorasCrgm = null; // Asignar el valor por defecto para nuevas actividades
                    actividad.FkEstado = 5; // Asignar el valor por defecto para nuevas actividades
                }

                lista.Add(actividad);
            }

            // Obtener los nombres de las actividades existentes para el proyecto actual
            var actividadesExistentes = contexto.Cronogramas
                .Where(c => c.FkProyecto == pry && lista.Select(a => a.NombreCrgm).Contains(c.NombreCrgm))
                .ToList();

            // Asignar el IdCronograma correspondiente a cada objeto Cronograma en la lista actividadesActualizar
            foreach (var actividadActualizar in lista.Where(c => actividadesExistentes.Select(a => a.NombreCrgm).Contains(c.NombreCrgm)))
            {
                var actividadExistente = actividadesExistentes.FirstOrDefault(a => a.NombreCrgm == actividadActualizar.NombreCrgm);
                if (actividadExistente != null)
                {
                    actividadActualizar.IdCronograma = actividadExistente.IdCronograma;
                }
            }

            // Filtrar las actividades de la lista que ya existen y actualizarlas
            var actividadesActualizar = lista.Where(c => c.IdCronograma != 0).ToList();

            // Filtrar las actividades de la lista que no existen y agregarlas
            var actividadesAgregar = lista.Where(c => c.IdCronograma == 0).ToList();

            // Actualizar las actividades existentes
            if (actividadesActualizar.Any())
            {
                contexto.BulkUpdate(actividadesActualizar);
            }

            // Agregar las nuevas actividades
            if (actividadesAgregar.Any())
            {
                contexto.BulkInsert(actividadesAgregar);
            }

            return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
        }
        [HttpPost]
        public IActionResult EliminarCronograma(int id)
        {
            try
            {

                EvolvProContext contexto = new EvolvProContext();
                // Paso 1: Calcular la suma de las horas_crgm para el proyecto
                var sumaHorasCrgm = contexto.Cronogramas.Where(x => x.FkProyecto == id).Sum(x => x.HorasCrgm);

                // Paso 2: Obtener el proyecto correspondiente
                var proyecto = contexto.Proyectos.Find(id);

                // Paso 3: Restar la suma al campo horas_totalesreal del proyecto
                proyecto.HorasTotalesreal -= sumaHorasCrgm;

                // Paso 4: Guardar los cambios en la base de datos
                contexto.SaveChanges();
                var cronogramas = contexto.Cronogramas.Where(x => x.FkProyecto == id).ToList();
                contexto.Cronogramas.RemoveRange(cronogramas);
                contexto.SaveChanges();
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }





        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::::::::::::::::::::::::::PROBLEMAS O ISSUES::::::::::::::::::::::::::::::::::::::::::::::::::::::
        [HttpPost]
        public IActionResult mostrarIssues(int id)
        {
            EvolvProContext contexto = new EvolvProContext();

            var query = from issue in contexto.Issues
                        join recurso in contexto.Recursos on issue.FkRecurso equals recurso.IdRecurso
                        join detalleEstado in contexto.DetalleEstados on issue.FkEstado equals detalleEstado.IdDetalleestado
                        where issue.FkCronograma == id
                        orderby issue.IdIssue descending
                        select new
                        {
                            issue.IdIssue,
                            issue.TituloIssue,
                            issue.DescripcionIssue,
                            issue.FechaIssue,
                            issue.FechaCierre,
                            issue.ResolucionIssue,
                            issue.HorasIssue,
                            recurso.NombreRec,
                            detalleEstado.ValorDestado
                        };

            List<object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }

            return Json(resultado);
        }
        [HttpPost]
        public IActionResult mostrarDetCategoria(int id)
        {
            EvolvProContext contexto = new EvolvProContext();

            var query = from detalleIssue in contexto.DetalleIssues
                        join issue in contexto.Issues on detalleIssue.FkIssue equals issue.IdIssue
                        join categoria in contexto.CategoriaIssues on detalleIssue.FkCategoria equals categoria.IdCatissue
                        where detalleIssue.FkIssue == id
                        select new
                        {
                            DetalleIssueId = detalleIssue.IdDetalleissue,
                            CategoriaId = categoria.IdCatissue,
                            CategoriaNombre = categoria.Nombre,
                            IssueId = issue.IdIssue,
                            IssueTitulo = issue.TituloIssue,
                            IssueDescripcion = issue.DescripcionIssue
                        };

            List<object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }

            return Json(resultado);
        }


        [HttpPost]
        public ActionResult guardarIssues(int id, string titulo, string desc, DateTime fechaIssue, DateTime? fechaFinal, string? resolu, decimal? horas, int estado, int crono, int recurso, int pry)
        {
            EvolvProContext contexto = new EvolvProContext();
            

            //CAPTURAMOS ALGUN POSIBLE ERROR
            try
            {
                Issue obj = new Issue();
                obj.IdIssue = id;
                obj.TituloIssue = titulo;
                obj.DescripcionIssue = desc;
                obj.FechaIssue = fechaIssue;
                obj.FechaCierre = fechaFinal;
                obj.ResolucionIssue = resolu;
                obj.HorasIssue = horas;
                obj.FkEstado = estado;
                obj.FkCronograma = crono;
                obj.FkRecurso = recurso;

                if (obj.IdIssue == 0 || obj.IdIssue == null)
                {
                    if (horas != null)
                    {
                        contexto.Issues.Add(obj);
                        contexto.SaveChanges();
                        var proyecto = contexto.Proyectos.Find(pry);
                        proyecto.HorasTotalesreal += horas;
                        contexto.SaveChanges();
                        return Json(true);
                    }
                    else
                    {
                        contexto.Issues.Add(obj);
                        contexto.SaveChanges();
                        return Json(true);
                    }
                }
                else
                {
                    var valorIssue = contexto.Issues.Find(id);
                    var horasAnterior = valorIssue.HorasIssue;
                    valorIssue.IdIssue = id;
                    valorIssue.TituloIssue = titulo;
                    valorIssue.DescripcionIssue = desc;
                    valorIssue.FechaIssue = fechaIssue;
                    valorIssue.FechaCierre = fechaFinal;
                    valorIssue.ResolucionIssue = resolu;
                    valorIssue.HorasIssue = horas;
                    valorIssue.FkEstado = estado;
                    valorIssue.FkCronograma = crono;
                    valorIssue.FkRecurso = recurso;

                    
                    if (horasAnterior == null)
                    {
                        horasAnterior = 0;
                    }

                    if (horas != null)
                    {
                        if (horas < horasAnterior)
                        {
                            
                            contexto.Entry(valorIssue).State = EntityState.Modified;
                            contexto.SaveChanges();

                            var restante = horasAnterior - horas;
                            var proyecto = contexto.Proyectos.Find(pry);
                            proyecto.HorasTotalesreal -= restante;
                            contexto.SaveChanges();

                            return Json(true);
                        }
                        else if (horas > horasAnterior)
                        {
                            
                            contexto.Entry(valorIssue).State = EntityState.Modified;
                            contexto.SaveChanges();

                            var restante = horas - horasAnterior;
                            var proyecto = contexto.Proyectos.Find(pry);
                            proyecto.HorasTotalesreal += restante;
                            contexto.SaveChanges();

                            return Json(true);
                        }
                        else
                        {
                            contexto.Entry(valorIssue).State = EntityState.Modified;
                            contexto.SaveChanges();
                            return Json(true);
                        }
                    }
                    else
                    {
                        contexto.Entry(valorIssue).State = EntityState.Modified;
                        contexto.SaveChanges();
                        return Json(true);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public ActionResult guardarDetCategoria(int idDetIssue, int idIssue, int idCate)
        {
            EvolvProContext contexto = new EvolvProContext();
            DetalleIssue obj = new DetalleIssue();
            obj.IdDetalleissue = idDetIssue;
            obj.FkIssue = idIssue;
            obj.FkCategoria = idCate;
            //CAPTURAMOS ALGUN POSIBLE ERROR
            try
            {
                    contexto.DetalleIssues.Add(obj);


                contexto.SaveChanges();
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public IActionResult EditarIssues(int id)
        {
            EvolvProContext contexto = new EvolvProContext();
            Issue res = new Issue();
            var objEdit = contexto.Issues.FirstOrDefault(x => x.IdIssue == id);


            res.IdIssue = objEdit.IdIssue;
            res.TituloIssue = objEdit.TituloIssue;
            res.DescripcionIssue = objEdit.DescripcionIssue;
            res.FechaIssue = objEdit.FechaIssue;
            res.FechaCierre = objEdit.FechaCierre;
            res.ResolucionIssue = objEdit.ResolucionIssue;
            res.HorasIssue = objEdit.HorasIssue;
            res.FkEstado = objEdit.FkEstado;
            res.FkRecurso = objEdit.FkRecurso;

            return Json(res);
        }

        [HttpPost]
        public IActionResult EliminarIssue(int id, int pry)
        {
            try
            {

                EvolvProContext contexto = new EvolvProContext();
                
                var valorIssue = contexto.Issues.Where(x => x.IdIssue == id).FirstOrDefault();

                var horasIssue = valorIssue.HorasIssue;
                if (horasIssue != null)
                {
                    var proyecto = contexto.Proyectos.Find(pry);


                    proyecto.HorasTotalesreal -= horasIssue;

                    // Paso 4: Guardar los cambios en la base de datos
                    contexto.SaveChanges();
                    var objDel = contexto.Issues.FirstOrDefault(x => x.IdIssue == id);
                    contexto.Issues.Remove(objDel);
                    contexto.SaveChanges();
                    return Json(true);
                }
                else
                {
                     var objDel = contexto.Issues.FirstOrDefault(x => x.IdIssue == id);
                    contexto.Issues.Remove(objDel);
                    contexto.SaveChanges();
                    return Json(true);
                }
                
                
            }


            catch (Exception ex)
            {
                return Json(false);
            }

        }
        [HttpPost]
        public IActionResult EliminarDetIssue(int id)
        {
            try
            {
                EvolvProContext contexto = new EvolvProContext();
                var objDel = contexto.DetalleIssues.FirstOrDefault(x => x.IdDetalleissue == id);
                contexto.DetalleIssues.Remove(objDel);
                contexto.SaveChanges();
                return Json(true);
            }

            catch (Exception ex)
            {
                return Json(false);
            }

        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //:::::::::::::::::: RECURSOS ::::::::::::::::::::::::::::
        [HttpPost]
        public ActionResult guardarRecurso(Recurso rec)
        {
            EvolvProContext contexto = new EvolvProContext();
            Recurso obj = new Recurso();
            obj.IdRecurso = rec.IdRecurso;
            obj.NombreRec = rec.NombreRec;
            obj.TelefonoRec = rec.TelefonoRec;
            obj.CorreoRec = rec.CorreoRec;

            //CAPTURAMOS ALGUN POSIBLE ERROR
            try
            {
                if (obj.IdRecurso == 0 || obj.IdRecurso == null)
                {
                    contexto.Recursos.Add(obj);
                }
                else
                {
                    contexto.Entry(rec).State = EntityState.Modified;
                }
                contexto.SaveChanges();
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        //editar recurso
        [HttpPost]
        public IActionResult EditarRecurso(int id)
        {
            EvolvProContext contexto = new EvolvProContext();
            Recurso res = new Recurso();
            var objEdit = contexto.Recursos.FirstOrDefault(x => x.IdRecurso == id);


            res.IdRecurso = objEdit.IdRecurso;
            res.NombreRec = objEdit.NombreRec;
            res.TelefonoRec = objEdit.TelefonoRec;
            res.CorreoRec = objEdit.CorreoRec;

            return Json(res);
        }

        // mostrar

        [HttpPost]
        public IActionResult mostrarRecurso()
        {
            EvolvProContext contexto = new EvolvProContext();
            var query = from recurso in contexto.Recursos
                        select new
                        {
                            recurso.IdRecurso,
                            recurso.NombreRec,
                            recurso.TelefonoRec,
                            recurso.CorreoRec,

                        };

            List<object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }

            return Json(resultado);
        }

        [HttpPost]
        public IActionResult EliminarRecurso(int id)
        {
            try
            {
                EvolvProContext contexto = new EvolvProContext();
                var objRecurso = contexto.Recursos.FirstOrDefault(x => x.IdRecurso == id);
                contexto.Recursos.Remove(objRecurso);
                contexto.SaveChanges();
                return Json(true);
            }

            catch (Exception ex)
            {
                return Json(false);
            }

        }






        //:::::::::::::::::: CATEGORIA ::::::::::::::::::::::::
        [HttpPost]
        public ActionResult guardarCategoria(CategoriaIssue cat)
        {
            EvolvProContext contexto = new EvolvProContext();
            CategoriaIssue obj = new CategoriaIssue();
            obj.IdCatissue = cat.IdCatissue;
            obj.Nombre = cat.Nombre;

            try
            {
                if (obj.IdCatissue == 0 || obj.IdCatissue == null)
                {
                    contexto.CategoriaIssues.Add(obj);
                }
                else
                {
                    CategoriaIssue categoriaExistiente = contexto.CategoriaIssues.FirstOrDefault(e => e.IdCatissue == obj.IdCatissue);
                    if (categoriaExistiente != null)
                    {
                        categoriaExistiente.Nombre = obj.Nombre;
                        contexto.Entry(categoriaExistiente).State = EntityState.Modified;
                    }

                    else
                    {
                        return Json(false); // Estado no encontrado, retorna falso
                    }

                }

                contexto.SaveChanges();


                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        // editar categoria
        [HttpPost]
        public IActionResult EditarCategoria(int id)
        {
            EvolvProContext contexto = new EvolvProContext();
            CategoriaIssue res = new CategoriaIssue();
            var objEdit = contexto.CategoriaIssues.FirstOrDefault(x => x.IdCatissue == id);


            res.IdCatissue = objEdit.IdCatissue;
            res.Nombre = objEdit.Nombre;

            return Json(res);
        }



        // mostrar

        [HttpPost]
        public IActionResult mostrarCategoria()
        {
            EvolvProContext contexto = new EvolvProContext();
            var query = from categoria in contexto.CategoriaIssues
                        select new
                        {
                            categoria.IdCatissue,
                            categoria.Nombre
                        };

            List<object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }

            return Json(resultado);
        }



        [HttpPost]
        public IActionResult EliminarCategoria(int id)
        {
            try
            {
                EvolvProContext contexto = new EvolvProContext();
                var objCategoria = contexto.CategoriaIssues.FirstOrDefault(x => x.IdCatissue == id);
                contexto.CategoriaIssues.Remove(objCategoria);
                contexto.SaveChanges();
                return Json(true);
            }

            catch (Exception ex)
            {
                return Json(false);
            }

        }




        //:::::::::::::::::: ESTADOO :::::::::::::::::::::::


        [HttpPost]
        public ActionResult guardarEstado(Estado est)
        {
            EvolvProContext contexto = new EvolvProContext();
            Estado obj = new Estado();
            obj.IdEstado = est.IdEstado;
            obj.Nombre = est.Nombre;


            // obj.FkEstado = det.FkEstado;
            //CAPTURAMOS ALGUN POSIBLE ERROR
            try
            {
                if (obj.IdEstado == 0 || obj.IdEstado == null)
                {
                    contexto.Estados.Add(obj);
                }
                else
                {
                    Estado estadoExistiente = contexto.Estados.FirstOrDefault(e => e.IdEstado == obj.IdEstado);
                    if (estadoExistiente != null)
                    {
                        estadoExistiente.Nombre = obj.Nombre;
                        contexto.Entry(estadoExistiente).State = EntityState.Modified;
                    }

                    else
                    {
                        return Json(false); // Estado no encontrado, retorna falso
                    }

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
        public IActionResult EditarEstado(int id)
        {
            EvolvProContext contexto = new EvolvProContext();
            Estado res = new Estado();
            var objEdit = contexto.Estados.FirstOrDefault(x => x.IdEstado == id);


            res.IdEstado = objEdit.IdEstado;
            res.Nombre = objEdit.Nombre;

            return Json(res);
        }



        [HttpPost]
        public IActionResult mostrarEstado()
        {
            EvolvProContext contexto = new EvolvProContext();
            var query = from estado in contexto.Estados
                        select new
                        {
                            estado.IdEstado,
                            estado.Nombre,

                        };

            List<object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }

            return Json(resultado);
        }



        [HttpPost]
        public IActionResult EliminarEstado(int id)
        {
            try
            {
                EvolvProContext contexto = new EvolvProContext();
                var objEstado = contexto.Estados.FirstOrDefault(x => x.IdEstado == id);
                contexto.Estados.Remove(objEstado);
                contexto.SaveChanges();
                return Json(true);
            }

            catch (Exception ex)
            {
                return Json(false);
            }

        }
        
        [HttpPost]
        public IActionResult mostrarRolhora()
        {
            EvolvProContext contexto = new EvolvProContext();
            var query = from rolh in contexto.RolHoras
                        join proy in contexto.Proyectos on rolh.FkProyecto equals proy.IdProyecto

                        select new
                        {
                            idRolhora = rolh.IdRolhora,
                            nombreRol = rolh.NombreRol,
                            valorHora = rolh.ValorHora,
                            horaTotal = rolh.HoraTotal,
                            fkProyecto = rolh.FkProyecto

                        };
            List<Object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }
            return Json(resultado);
        }
        [HttpPost]
        public ActionResult guardarRolhora(RolHora rolh)
        {
            EvolvProContext contexto = new EvolvProContext();
            RolHora obj = new RolHora();
            obj.IdRolhora = rolh.IdRolhora;
            obj.NombreRol = rolh.NombreRol;
            obj.ValorHora = rolh.ValorHora;
            obj.HoraTotal = rolh.HoraTotal;

            obj.FkProyecto = rolh.FkProyecto;
            //CAPTURAMOS ALGUN POSIBLE ERROR
            try
            {
                if (obj.IdRolhora == 0 || obj.IdRolhora == null)
                {
                    contexto.RolHoras.Add(obj);

                }
                else
                {
                    contexto.Entry(rolh).State = EntityState.Modified;

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
        public IActionResult EditarRolhora(int id)
        {
            EvolvProContext contexto = new EvolvProContext();
            RolHora res = new RolHora();
            var objEdit = contexto.RolHoras.FirstOrDefault(x => x.IdRolhora == id);

            res.IdRolhora = objEdit.IdRolhora;
            res.NombreRol = objEdit.NombreRol;
            res.ValorHora = objEdit.ValorHora;
            res.HoraTotal = objEdit.HoraTotal;

            res.FkProyecto = objEdit.FkProyecto;

            return Json(res);
        }

        [HttpPost]
        public IActionResult EliminarRolhora(int id)
        {
            try
            {
                EvolvProContext contexto = new EvolvProContext();
                var objDel = contexto.RolHoras.FirstOrDefault(x => x.IdRolhora == id);
                contexto.RolHoras.Remove(objDel);
                contexto.SaveChanges();
                return Json(true);
            }

            catch (Exception ex)
            {
                return Json(false);
            }

        }

        //:::::::::::::::::::::::::::GRAFICOS:::::::::::::::::::::::::::::::::::::
        public IActionResult resumenHorasPry()
        {
            EvolvProContext _dbcontext = new EvolvProContext();
            List<Proyecto> Lista = _dbcontext.Proyectos
                .OrderByDescending(p => p.IdProyecto) // Ordenar descendentemente por las horas totales reales
                .Select(p => new Proyecto
                {
                    NombrePry = p.NombrePry,
                    HorasTotalesreal = p.HorasTotalesreal
                })
                .ToList();

            return StatusCode(StatusCodes.Status200OK, Lista);
        }

        public IActionResult resumenPryTerPro()
        {
            EvolvProContext _dbcontext = new EvolvProContext();

            int proyectosEstado3 = _dbcontext.Proyectos.Count(p => p.FkEstado == 3);
            int proyectosEstado4 = _dbcontext.Proyectos.Count(p => p.FkEstado == 4);

            var resumen = new
            {
                ProyectosEstado3 = proyectosEstado3,
                ProyectosEstado4 = proyectosEstado4
            };

            return StatusCode(StatusCodes.Status200OK, resumen);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

        //MANTENIMIENTO REUNIONES

        [HttpPost]
        public ActionResult guardarReuniones(Reunione reu)
        {
            EvolvProContext contexto = new EvolvProContext();
            Reunione obj = new Reunione();
            obj.IdReunion = reu.IdReunion;
            obj.TituloReu = reu.TituloReu;
            obj.PuntosTratar = reu.PuntosTratar;
            obj.DesarrolloPunto = reu.DesarrolloPunto;
            obj.Asistentes = reu.Asistentes;
            obj.TiempoReu = reu.TiempoReu;
            obj.FkProyecto = reu.FkProyecto;

            //CAPTURAMOS ALGUN POSIBLE ERROR
            try
            {
                if (obj.IdReunion == 0 || obj.IdReunion == null)
                {
                    contexto.Reuniones.Add(obj);
                }
                else
                {
                    contexto.Entry(reu).State = EntityState.Modified;
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
        public IActionResult mostrarReuniones()
        {
            EvolvProContext contexto = new EvolvProContext();
            var query = from reu in contexto.Reuniones
                        join pro in contexto.Proyectos on reu.FkProyecto equals pro.IdProyecto
                        //join preUsu in contexto.PreguntaSeguridads on usu.FkPregunta equals preUsu.IdPregunta
                        select new
                        {
                            idReunion = reu.IdReunion,
                            tituloReu = reu.TituloReu,
                            puntosTratar = reu.PuntosTratar,
                            desarrolloPunto = reu.DesarrolloPunto,
                            asistentes = reu.Asistentes,
                            tiempoReu = reu.TiempoReu,
                            fkProyecto = pro.NombrePry
                        };
            List<Object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }
            return Json(resultado);
        }

        [HttpPost]
        public IActionResult EditarReuniones(int id)
        {
            EvolvProContext contexto = new EvolvProContext();
            Reunione res = new Reunione();
            var objEdit = contexto.Reuniones.FirstOrDefault(x => x.IdReunion == id);

            res.IdReunion = objEdit.IdReunion;
            res.TituloReu = objEdit.TituloReu;
            res.PuntosTratar = objEdit.PuntosTratar;

            res.DesarrolloPunto = objEdit.DesarrolloPunto;
            res.Asistentes = objEdit.Asistentes;
            res.TiempoReu = objEdit.TiempoReu;
            res.FkProyecto = objEdit.FkProyecto;

            return Json(res);
        }

        [HttpPost]
        public IActionResult EliminarReuniones(int id)
        {
            EvolvProContext contexto = new EvolvProContext();
            try
            {
                var objDel = contexto.Reuniones.FirstOrDefault(x => x.IdReunion == id);
                contexto.Reuniones.Remove(objDel);
                contexto.SaveChanges();
                return Json(true);
            }

            catch (Exception ex)
            {
                return Json(false);
            }

        }

        public IActionResult listaProyectos()
        {
            EvolvProContext contexto = new EvolvProContext();
            List<Proyecto> proyectos = contexto.Proyectos.ToList();
            return Json(proyectos);
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //:::::::::::::::::: PREGUNTA SEGURIDAD ::::::::::::::::::::
        [HttpPost]
        public ActionResult guardarPregunta(PreguntaSeguridad pre)
        {
            EvolvProContext contexto = new EvolvProContext();
            PreguntaSeguridad obj = new PreguntaSeguridad();
            obj.IdPregunta = pre.IdPregunta;
            obj.DescPregunta = pre.DescPregunta;

            //CAPTURAMOS ALGUN POSIBLE ERROR
            try
            {
                if (obj.IdPregunta == 0 || obj.IdPregunta == null)
                {
                    contexto.PreguntaSeguridads.Add(obj);
                }
                else
                {
                    contexto.Entry(pre).State = EntityState.Modified;
                }
                contexto.SaveChanges();
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        //editar recurso
        [HttpPost]
        public IActionResult EditarPregunta(int id)
        {
            EvolvProContext contexto = new EvolvProContext();
            PreguntaSeguridad res = new PreguntaSeguridad();
            var objEdit = contexto.PreguntaSeguridads.FirstOrDefault(x => x.IdPregunta == id);


            res.IdPregunta = objEdit.IdPregunta;
            res.DescPregunta = objEdit.DescPregunta;

            return Json(res);
        }


        [HttpPost]
        public IActionResult mostrarPreguntaSeguridad()
        {
            EvolvProContext contexto = new EvolvProContext();
            var query = from pregunta in contexto.PreguntaSeguridads
                        select new
                        {
                            pregunta.IdPregunta,
                            pregunta.DescPregunta
                        };

            List<object> resultado = new List<object>();
            foreach (var item in query)
            {
                resultado.Add(item);
            }

            return Json(resultado);
        }

        [HttpPost]
        public IActionResult EliminarPregunta(int id)
        {
            try
            {
                EvolvProContext contexto = new EvolvProContext();
                var objPregunta = contexto.PreguntaSeguridads.FirstOrDefault(x => x.IdPregunta == id);
                contexto.PreguntaSeguridads.Remove(objPregunta);
                contexto.SaveChanges();
                return Json(true);
            }

            catch (Exception ex)
            {
                return Json(false);
            }

        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}