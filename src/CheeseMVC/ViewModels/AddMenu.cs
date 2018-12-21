using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.ViewModels
{
    public class AddMenuItemViewModel
    {
        [Required]
        [Display(Name = "Cheese")]
        public int CheeseID { get; set; }


        [Required]
        [Display(Name = "Menu")]
        public int MenuID { get; set; }
        public Menu Menu { get; set; }

        public List<SelectListItem> Cheeses { get; set; }

        public AddMenuItemViewModel() { }


        public AddMenuItemViewModel(Menu menu, IEnumerable<Cheese> cheeses)
        {
            this.Menu = menu;
            this.MenuID = menu.ID;

            Cheeses = new List<SelectListItem>();

            foreach (var cheese in cheeses)
            {
                if (CheeseIsAvailable(cheese, menu.ID))
                {
                    Cheeses.Add(new SelectListItem
                    {
                        Value = cheese.ID.ToString(),
                        Text = cheese.Name
                    });
                }
            }
        }

        private bool CheeseIsAvailable(Cheese cheese, int id)
        {
            bool unique = true;

            foreach (var cheeseMenu in cheese.CheeseMenu)
            {
                if (cheeseMenu.MenuID == id)
                {
                    unique = false;
                    break;
                }
            }

            return unique;
        }


       

    }

}