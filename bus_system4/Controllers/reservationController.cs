using bus_system3.Areas.Identity.Data;
using bus_system4.Areas.Identity.Data;
using bus_system4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace bus_system4.Controllers
{
    public class reservationController : Controller
    {
        private readonly bus_context dbcontext2;
       

        public reservationController(bus_context dbcontext2 )
        {
            this.dbcontext2 = dbcontext2;
            
        }
        // GET: reservationController
        public ActionResult result()
        {
            //var book = _booksService.GetById(id);
           // ViewBag.purchase = book.Price;

            string userid = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            IQueryable<reservation> items = from i in dbcontext2.reservation.Include(x=> x.bus_user).
                                            Where(x => x.UserId ==userid)
                                        orderby i.Id
                                        select i;

            IQueryable<int> myitems = (from i in dbcontext2.reservation.Include(x => x.bus_user).
                                            Where(x => x.UserId == userid)
                                     select i.price);

            int x = 0;
            foreach (var item in myitems)
            {
                x += item;
            }
            ViewBag.purchase = x;
            List<reservation> todoList = items.ToList();
            
          
            return View(todoList);
          
        }

        
        public ActionResult reserve()
        {

            return View();
        }

        // POST: reservationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult reserve(reservation reserv )
        {
            if (ModelState.IsValid)
            {
               reserv.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (reserv.departure == "cairo" && reserv.arrival == "sharm")
                    reserv.price = 1000;
                else if (reserv.departure == "cairo" && reserv.arrival == "hurghada")
                    reserv.price = 1250;
                else if (reserv.departure == "tanta" && reserv.arrival == "sharm")
                    reserv.price = 1300;
                else if (reserv.departure == "tanta" && reserv.arrival == "hurghada")
                    reserv.price = 1350;

                dbcontext2.Add(reserv);
                dbcontext2.SaveChanges();

                return RedirectToAction("result");
            }
            return View(reserv);
        }

        // GET: reservationController/Edit/5
        public ActionResult Edit(int id)
        {
           // string userid = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            reservation reserv_item = dbcontext2.reservation.Find(id);
            if (reserv_item == null)
                return NotFound();

            return View(reserv_item);
            
        }

        // POST: reservationController/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(  reservation reserv)
        {
            if(ModelState.IsValid)
            {
                reserv.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (reserv.departure == "cairo" && reserv.arrival == "sharm")
                    reserv.price = 1000;
                else if (reserv.departure == "cairo" && reserv.arrival == "hurghada")
                    reserv.price = 1250;
                else if (reserv.departure == "tanta" && reserv.arrival == "sharm")
                    reserv.price = 1300;
                else if (reserv.departure == "tanta" && reserv.arrival == "hurghada")
                    reserv.price = 1350;
                dbcontext2.Update(reserv);
                dbcontext2.SaveChanges();



                return RedirectToAction("result");
            }
            return View(reserv);
        }

        // GET: reservationController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            reservation reserv_item = await dbcontext2.reservation.FindAsync(id);


            dbcontext2.reservation.Remove(reserv_item);
            await dbcontext2.SaveChangesAsync();
            return RedirectToAction("result");
        }
        [HttpPost]
        public IActionResult Create(string stripeToken)
        {
            string userid = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            IQueryable<int> items =  (from i in dbcontext2.reservation.Include(x => x.bus_user).
                                            Where(x => x.UserId == userid)
                                   select i.price);

            int x= 0;
            foreach (var item in items)
            {
                x+=item;
            }

            // Console.WriteLine(reserv_item.price);
            var chargeoptions = new ChargeCreateOptions()
            {
                Amount = x * 100,
                Currency = "usd",
                Source = stripeToken,

            };
            var service = new ChargeService();

            Charge charge = service.Create(chargeoptions);
            if (charge.Status == "succeeded")
            {
                return View("success");
            }
            else
            {
                return View("fail");
            }

        }
        /*
        public IActionResult Charge (string stripeEmail , string stripeToken)
        {
            var customerservice = new CustomerService();
            var chargeservice = new ChargeService();
            var customer = customerservice.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken,
                
            }
            );
            
            var Card = new TokenCardOptions
            {
                Number = 
                ExpMonth = payModel.Month,
                ExpYear = payModel.Year,
                Cvc = payModel.CVC
            };
            
            
            var charge = chargeservice.Create(new ChargeCreateOptions
            {
                Amount = 500,
               Description =    "airline reservation system",
                Currency = "usd",
                Customer = customer.Id
            // Customer = customer.Id

        }
            ) ;
            

            
            return View();    
        }

    */

    }
}
