using Demo.BLL.Models;
using Demo.Entity.Entity;
using ESSAM_ACCOUNTANT.BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.BLL.Helpers
{
    public class OrderItemHelper
    {
        public string SaveOrder(OrderVM orderdata)
        {
            string message = string.Empty;
            try
            {
                DemoEntities entities = new DemoEntities();
                ItemMasterTable obj = new ItemMasterTable();
                obj.Code = orderdata.Code;
                obj.MDate = orderdata.MDate;
                obj.DueDays = orderdata.DueDays;
                obj.Duedate = orderdata.DueDate;
                obj.Party = orderdata.Party;
                obj.CreatedOn = DateTime.Now;
                obj.UpdatedOn = (DateTime?)null;

                entities.ItemMasterTables.Add(obj);
                entities.SaveChanges();

                if (orderdata.OrderDetails.Count > 0)
                {
                    for (int i = 0; i < orderdata.OrderDetails.Count; i++)
                    {
                        ItemDetail objdetails = new ItemDetail();
                        objdetails.Id = obj.Id;
                        objdetails.SerialNo = orderdata.OrderDetails[i].SerialNo;
                        objdetails.ItemName = orderdata.OrderDetails[i].ItemName;
                        objdetails.Qty = orderdata.OrderDetails[i].Qty;
                        objdetails.Rate = orderdata.OrderDetails[i].Rate;
                        objdetails.Amount = orderdata.OrderDetails[i].Amount;
                        objdetails.CreatedOn = DateTime.Now;
                        objdetails.UpdatedOn = (DateTime?)null;

                        entities.ItemDetails.Add(objdetails);
                        entities.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.ErrorMsg("OrderItemHelper", "SaveOrder", ex.Message.ToString());
                throw;
            }
            message = "Data Saved Successfully";
            return message;
        }

        public List<OrderVM> GetData()
        {
            List<OrderVM> orderList = new List<OrderVM>();
            try
            {
                DemoEntities entities = new DemoEntities();
                orderList = entities.ItemMasterTables.ToList().Select(x => new OrderVM()
                {
                    Code = x.Code.Value,
                    MDate = x.MDate.Value,
                    DueDate = x.Duedate.Value,
                    Party = x.Party
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return orderList;
        }

        public OrderVM GetItemData(int OrderId)
        {
            OrderVM orderList = new OrderVM();
            try
            {
                DemoEntities entities = new DemoEntities();
                ItemMasterTable dbMaster = entities.ItemMasterTables.Where(x => x.Id == OrderId).FirstOrDefault();

                if (dbMaster != null)
                {
                    orderList.Id = dbMaster.Id;
                    orderList.Code = dbMaster.Code.Value;
                    orderList.MDate = dbMaster.MDate.Value;
                    orderList.DueDate = dbMaster.Duedate.Value;
                    orderList.DueDays = (int)dbMaster.DueDays;

                    List<ItemDetail> dbItems = entities.ItemDetails.Where(x => x.Id == dbMaster.Id).ToList();
                    orderList.OrderDetails = new List<ItemDetail>();
                    foreach (var item in dbItems)
                    {
                        ItemDetail itemdetail = new ItemDetail();
                        itemdetail.SerialNo = item.SerialNo;
                        itemdetail.ItemName = item.ItemName;
                        itemdetail.ItemId = item.ItemId;
                        itemdetail.Id = item.Id;
                        itemdetail.Qty = item.Qty;
                        itemdetail.Rate = item.Rate;
                        itemdetail.Amount = item.Amount;

                        orderList.OrderDetails.Add(itemdetail);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return orderList;
        }
    }
}
