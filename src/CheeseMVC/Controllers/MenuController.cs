using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.EntityFrameworkCore;
using static CheeseMVC.ViewModels.ViewMenuViewModel;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly CheeseDbContext context;
        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            IList<Menu> allMenus = context.Menus.ToList();

            return View(allMenus);
        }

        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name
                };
                context.Menus.Add(newMenu);
                context.SaveChanges();

                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }

            return View(addMenuViewModel);
        }

        public IActionResult ViewMenu(int id)
        {
            Menu menuRequested = context.Menus.Single(c => c.ID == id);
            IList<CheeseMenu> items = context.CheeseMenus
                .Include(item => item.Cheese)
                .Where(c => c.MenuID == id)
                .ToList();
            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel
            {
                Menu = menuRequested,
                Items = items
            };
            return View(viewMenuViewModel);
        }

        public IActionResult AddItem(int id)
        {
            Menu menu = context.Menus
                .Single(c => c.ID == id);

            List<Cheese> cheeses = context.Cheeses
                .Include(c => c.CheeseMenu)
                .ToList();

            

            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(menu, cheeses);

            return View(addMenuItemViewModel);
        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (ModelState.IsValid)
            {
                if (context.CheeseMenus
                    .Where(cm => cm.CheeseID == addMenuItemViewModel.CheeseID)
                    .Where(cm => cm.MenuID == addMenuItemViewModel.MenuID)
                    .ToList().Count == 0)
                {
                    CheeseMenu cheeseMenu = new CheeseMenu
                    {
                        MenuID = addMenuItemViewModel.MenuID,
                        CheeseID = addMenuItemViewModel.CheeseID

                    };
                    context.CheeseMenus.Add(cheeseMenu);
                    context.SaveChanges();


                }

                return Redirect("/Menu/ViewMenu/" + addMenuItemViewModel.MenuID);
            }

            return View(addMenuItemViewModel);
        }
    }
}