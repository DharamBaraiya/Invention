using Demo.BLL.Helpers;
using Demo.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Demo.Controllers
{
    public class OrderMasterController : Controller
    {
        // GET: OrderMaster
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveOrder(OrderVM Orderdata)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                OrderItemHelper orderHelper = new OrderItemHelper();
                message = orderHelper.SaveOrder(Orderdata);
                status = true;
            }
            else
            {
                status = false;
            }
            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetData()
        {
            bool status = false;
            List<OrderVM> data = new List<OrderVM>();
            try
            {
                OrderItemHelper orderHelper = new OrderItemHelper();
                data = orderHelper.GetData();
                status = true;
            }
            catch (Exception)
            {
                throw;
            }
            return Json(new { status = status, Data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemData(int OrderId)
        {
            bool status = false;
            OrderVM data = new OrderVM();
            try
            {
                OrderItemHelper orderHelper = new OrderItemHelper();
                data = orderHelper.GetItemData(OrderId);
                status = true;
            }
            catch (Exception)
            {
                throw;
            }
            return Json(new { status = status, Data = data }, JsonRequestBehavior.AllowGet);
        }
    }
}