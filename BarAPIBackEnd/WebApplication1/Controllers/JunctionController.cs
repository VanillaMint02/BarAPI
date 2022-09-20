using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Proiect_II.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace Proiect_II.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JunctionController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public JunctionController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {
            String query = @"select ID_Ingredient,ID_Product from dbo.Junction";
            DataTable table = new DataTable();
            String path = this.configuration.GetConnectionString("BarAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(path))
            {
                myCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, myCon))
                {
                    myReader = sqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }

            }
            return new JsonResult(table);
        }
      
        [HttpPost]
        public JsonResult Post(String pr,String ig )
        {
            String queryProduct = @"select Product_Name,ID_Product,count(Product_Name) as Count from dbo.Products  where Product_Name='" + pr + @"' group by Product_Name,ID_Product";
            String queryIngredient = @"select ingredientName,ID_Ingredient,count(ingredientName) as Count from dbo.Ingredients where ingredientName='" + ig + @"'group by ingredientName,ID_Ingredient";
            
            //initializations
            
            DataTable tableProduct = new DataTable();
            DataTable tableIngredient = new DataTable();
            
            SqlDataReader myReader;
            String path = this.configuration.GetConnectionString("BarAppCon");
            //table creating
            using (SqlConnection myCon=new SqlConnection(path))
            {
                myCon.Open();
                using(SqlCommand sqlCommand=new SqlCommand(queryProduct, myCon))
                {
                    myReader = sqlCommand.ExecuteReader();
                    tableProduct.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            using (SqlConnection myCon = new SqlConnection(path))
            {
                myCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(queryIngredient, myCon))
                {
                    myReader = sqlCommand.ExecuteReader();
                    tableIngredient.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }


            using (SqlConnection myCon = new SqlConnection(path))
            {
                myCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(queryProduct, myCon))
                {
                    myReader = sqlCommand.ExecuteReader();
                    tableProduct.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            bool ok1 = false, ok2 = false;
            if (tableIngredient != null && tableProduct != null)
            {
                 ok1 = tableIngredient.Rows[0][0].ToString().Equals(ig.ToString());
                 ok2 = tableProduct.Rows[0][0].ToString().Equals(pr.ToString());
            }

            if (ok1 == true && ok2 == true) {


                //   return new JsonResult("The IDS which are going to be posted are " +tableProduct.Rows[0][1] + " " + tableIngredient.Rows[0][1]);

               Junction ju = new Junction((int)tableProduct.Rows[0][1], (int)tableIngredient.Rows[0][1]);
                String query = @"insert into dbo.Junction (ID_Product,ID_Ingredient) values ('"+ju.ProductID+@"','"+ju.IngredientID+@"')";
                DataTable table = new DataTable();

                using (SqlConnection myCon = new SqlConnection(path))
                {
                    myCon.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(query, myCon))
                    {
                        myReader = sqlCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }

                }
                return new JsonResult("Posted Succesfully");
                
            }
            else if (ok1 == true) return new JsonResult("the Ingredient does not belong to the database");
            else if (ok2 == true) return new JsonResult("The Product does not belong to the database");
            else return new JsonResult ("Neither the Ingredient or the Product belong to the database ");

            //verification + addition
              //and in the end, if everything is alright, it posts.
            
           
        }

    }
}
