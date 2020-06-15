using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Calendario.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            using (BancoDeDadosEntities bd = new BancoDeDadosEntities())
            {
                var events = bd.Events.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SalveEvents(Events e)
        {
            var status = false;
            using (BancoDeDadosEntities bd = new BancoDeDadosEntities())
            {
                if (e.EventID > 0)
                {
                    //Aquele update no bd
                    var v = bd.Events.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if(v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    bd.Events.Add(e);
                }
                bd.SaveChanges();
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
        }
        
        [HttpPost]
        public JsonResult DeleteEvents(int eventID)
        {
            var status = false;
            using (BancoDeDadosEntities bd = new BancoDeDadosEntities())
            {
                var v = bd.Events.Where(a => a.EventID == eventID).FirstOrDefault();
                if(v != null)
                {
                    bd.Events.Remove(v);
                    bd.SaveChanges();
                    status = true;
                }
            }

                return new JsonResult { Data = new { status = status } };
        }
    }
}