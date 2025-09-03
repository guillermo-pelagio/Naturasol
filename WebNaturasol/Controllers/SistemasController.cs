using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class SistemasController : Controller
    {
        /*
        //[Authorize]
        public ActionResult tickets()
        {
            if (((Session["usuarioId"]) != null))
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "inicio");
            }
        }
        */
        public PartialViewResult tickets()
        {
            if (((Session["usuarioId"]) != null))
            {
                return PartialView();
            }
            else
            {
                return PartialView("login", "inicio");
            }
        }

        // GET: Sistemas
        public ActionResult Index()
        {
            return View();
        }

        // GET: Sistemas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Sistemas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sistemas/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sistemas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sistemas/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sistemas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sistemas/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
