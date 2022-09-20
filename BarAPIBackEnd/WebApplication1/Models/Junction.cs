using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_II.Models
{
    public class Junction
    {

        public int ProductID { get; set; }
        public int IngredientID { get; set; }

        public Junction (int ProductID, int IngredientID)
        {
            this.ProductID = ProductID;
            this.IngredientID = IngredientID;

        }
    }
}
