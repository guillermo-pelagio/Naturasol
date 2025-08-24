using System.Web.Mvc;

namespace WebArribosPlanta.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(int error = 0)
        {

            ViewBag.Title = "Error desconocido";
            ViewBag.Description = "Algo salio muy mal, comunicate a sistemas " + error; 
            return View("~/views/error/error.cshtml");
        }

        public ActionResult error403()
        {
            ViewBag.Title = "¡Error de acceso!";
            ViewBag.Description = "No tienes permiso para ver esta página"; 
            return View();
        }
        public ActionResult error404()
        {

            ViewBag.Title = "Página no encontrada";
            ViewBag.Description = "La dirección que está intentando ingresar no existe"; 
            return View();
        }

        public ActionResult error505()
        {

            ViewBag.Title = "Ocurrio un error inesperado";
            ViewBag.Description = "Comunicarse a sistemas"; 
            return View();
        }
    }
}
