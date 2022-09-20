using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public IngredientController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {
            String query = @"select ingredientName,Availability, ID_Ingredient from dbo.Ingredients order by Availability";
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
        [HttpPatch]
        public JsonResult Patch(Ingredient ig)
        {

            Console.WriteLine(ig);
            String query = @" update dbo.Ingredients set Availability='" + ig.Availability + @"' where ingredientName='" + ig.IngredientName + @"'";

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
            return new JsonResult("Updated '" + ig.IngredientName + @"' an availability of '" + ig.Availability + @"'");


        }
        public JsonResult Post(Ingredient ig)
        {
            String query = @"insert into dbo.Ingredients(ingredientName,Availability,ID_Ingredient) values
('" + ig.IngredientName + @"'
,'" + ig.Availability + @"'
,'" + ig.ID_Ingredient + @"')";
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
                return new JsonResult("Posted Succesfully");
            }
        }
    }
}

