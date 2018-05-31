using EntityLesson.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EntityLesson.Controllers
{
    public class HomeController : Controller
    {
		
        public ActionResult Index()
        {
            //build our ORM Object Relational Mapping
            CoffeeEntities ORM = new CoffeeEntities();

            //these lines of code grab the data from our DB by using the ORM
            //I use the ToList() to make the data a list we can later index through
            ViewBag.Items = ORM.Items.ToList();
            ViewBag.Users = ORM.Users.ToList();


            return View();
        }

        //we pass in the item data so that we can add the new item to our DB
        public ActionResult About(Item data)
        {
            CoffeeEntities ORM = new CoffeeEntities();
            //we check to make sure the Item Model that we passed in is Valid
            if (ModelState.IsValid)
            {
                //if the model is valid then we add to our DB
                ORM.Items.Add(data);
                //we have to save our changes or they won't stay in our DB
                ORM.SaveChanges();
                ViewBag.message = $"{data.Description} has been added";
            }
            else
            {
                ViewBag.message = "Item is not valid, cannot add to DB.";
            }

            return View();
        }
        //we pass in the ItemID so that we can remove a certain object
        public ActionResult Contact(int ID)
        {
            CoffeeEntities ORM = new CoffeeEntities();
            //we build this object so that we can make a transaction
            DbContextTransaction DeleteCustomerTransaction = ORM.Database.BeginTransaction();
            Item temp = new Item();
            try
            {
                //we first find the specific item by the items id
                temp = ORM.Items.Find(ID);
                ORM.Items.Remove(temp);
                ORM.SaveChanges();
                DeleteCustomerTransaction.Commit(); //if the remove was successful we commit the transaction
                ViewBag.Message = $"{temp.Description} was removed";
            }
            catch (Exception ex)
            {
                //if the remove was unsuccessful then we 
                //roll back the transaction so no data is lost
                DeleteCustomerTransaction.Rollback();                
                ViewBag.Message = "Item could not be removed";
            }

            return View();

        }

        public ActionResult ItemView(string Name)
        {
            CoffeeEntities ORM = new CoffeeEntities();
            //here I used a .Where method, and then I used a lambda function to find items for a specific customer
            ViewBag.Items = ORM.Items.Where(x => x.Name == Name).ToList();

            return View();

            //This code is for looking up an item be description instead of customer email

            //way to find item by description
            //grab all items from the DB
            //List<Item> itemList = ORM.Items.ToList();
            ////make blank list to add specific items to
            //List<Item> newItems = new List<Item>();
            ////this loop will compare the items to the the description
            //foreach (Item I in itemList)
            //{
            //    if (I.Description == descrip)
            //    {
            //        newItems.Add(I);
            //    }
            //}
        }
    }
}