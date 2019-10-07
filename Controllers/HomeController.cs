using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;

namespace CRUDelicious.Controllers
{
    public class HomeController : Controller
    {
        private DishesContext dbContext;

        public HomeController(DishesContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            List<Dish> AllDishes = dbContext.Dishes.ToList();
            AllDishes.Reverse();
            return View(AllDishes);
        }

        [HttpGet]
        [Route("/new")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        [Route("/edit/{dishId}")]
        public IActionResult Edit(int dishId)
        {
            Dish selectedDish = dbContext.Dishes.FirstOrDefault(user => user.DishId == dishId);
            return View(selectedDish);
        }

        [HttpPost("/create")]
        public IActionResult CreateDish(Dish newDish)
        {
            if(ModelState.IsValid){
                dbContext.Add(newDish);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Add");
        }  

        [HttpPost("update/{dishId}")]
        public IActionResult UpdateDish(int dishId, Dish changedDish)
        {
            if(ModelState.IsValid){
                Dish selectedDish = dbContext.Dishes.FirstOrDefault(dish => dish.DishId == dishId);
                selectedDish.Name = changedDish.Name;
                selectedDish.Chef = changedDish.Chef;
                selectedDish.Tastiness = changedDish.Tastiness;
                selectedDish.Calories = changedDish.Calories;
                selectedDish.Description = changedDish.Description;
                selectedDish.UpdatedAt = DateTime.Now;
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit");
        }

        [HttpGet("delete/{dishId}")]
        public IActionResult DeleteDish(int dishId)
        {
            Dish selectedDish = dbContext.Dishes.FirstOrDefault(dish => dish.DishId == dishId);
            dbContext.Dishes.Remove(selectedDish);
            dbContext.SaveChanges();
            // Other code
            return RedirectToAction("Index");
        }

        [HttpGet("/{dishId}")]
        public IActionResult GetOneDish(int dishId)
        {
            Dish selectedDish = dbContext.Dishes.FirstOrDefault(user => user.DishId == dishId);
            return View(selectedDish);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
