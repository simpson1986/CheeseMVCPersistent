﻿using System.Collections.Generic;
using static CheeseMVC.ViewModels.ViewMenuViewModel;

namespace CheeseMVC.Models
{
    public class Cheese
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public CheeseCategory Category { get; set; }
        public int CategoryID { get; set; }

        public IList<CheeseMenu> CheeseMenu { get; set; }


    }
}

   