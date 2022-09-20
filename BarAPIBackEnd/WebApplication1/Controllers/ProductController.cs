using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
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
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public ProductController(IConfiguration configuration) 
        {
            this.configuration = configuration;
        }

        [HttpGet("{Id}")]
        public JsonResult Get(int id)
        {
            int minValue=0, maxValue=0;
             switch (id)
             {
                 case 0: minValue = 0;maxValue = 1000;break; // Products which do not have a recipe
                 case 1: minValue = 1001;maxValue = 2000;break;// Coffe
                 case 2: minValue = 2001;maxValue = 3000;break;// Shots
                 case 3: minValue = 3001;maxValue = 4000;break;// Cocktails
              
             };
            if (id == 0)
            {
                String query = "select Product_Name,Price from Products where ID_Product between '" + minValue + @"'and '" + maxValue + @"'";
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
                    return new JsonResult(table);
                }
            }
            else
            {
                String query_available = "select  Product_Name,Price," +
                    "count (Product_Name) as Count " +
                    "from Products " +
                    "join Junction on Products.ID_Product =Junction.ID_Product join Ingredients on Ingredients.ID_Ingredient = Junction.ID_Ingredient " +
                    "where Ingredients.Availability = 1 and Products.ID_Product between '" + minValue + @"' and '" + maxValue + @"' group by Product_Name,Price";
                DataTable table_available = new DataTable();
                String path = this.configuration.GetConnectionString("BarAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(path))
                {
                    myCon.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(query_available, myCon))
                    {
                        myReader = sqlCommand.ExecuteReader();
                        table_available.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }

                }

                String query_all = " select Product_Name,Price, " +
                    "count(Product_Name) as Count " +
                    "from Products " +
                    "join Junction on Products.ID_Product = Junction.ID_Product join Ingredients on Ingredients.ID_Ingredient = Junction.ID_Ingredient " +
                    "where Products.ID_Product between '" + minValue + @"' and '" + maxValue + @"' 
                     group by Product_Name,Price";
                DataTable table_all = new DataTable();
                using (SqlConnection myCon = new SqlConnection(path))
                {
                    myCon.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(query_all, myCon))
                    {
                        myReader = sqlCommand.ExecuteReader();
                        table_all.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }
                DataTable table_final = new DataTable();
                table_final.Clear();
                table_final.Columns.Add("Product_Name");
                table_final.Columns.Add("Price");
                table_final.Columns.Add("Count");
                for (int i = 0; i < table_all.Rows.Count; i++)

                {
                    if (table_available.Rows[i][0].ToString().Equals(table_all.Rows[i][0].ToString())
                        && table_available.Rows[i][1].ToString().Equals(table_all.Rows[i][1].ToString())
                        && table_available.Rows[i][2].ToString().Equals(table_all.Rows[i][2].ToString()))
                        table_final.Rows.Add(table_available.Rows[i][0], table_available.Rows[i][1],table_available.Rows[i][2]);
                }

                table_final.Columns.Remove("Count");
                return new JsonResult(table_final);

            }  
        }

        [HttpPost]
        public JsonResult Post( Products pr)
        {
            String query = @"insert into dbo.Products(Product_Name,ID_Product,Price) values
('" + pr.productName + @"'
,'" + pr.ID_Product+ @"'
,'" + pr.price + @"')";
            String verifier = @"Select ID_Product from dbo.Products";

            DataTable table1 = new DataTable();
            String path = this.configuration.GetConnectionString("BarAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(path))
            {
                myCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(verifier, myCon))
                {
                    myReader = sqlCommand.ExecuteReader();
                    table1.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }

            }
           for(int i=0;i<table1.Rows.Count;i++)
            {
                if (table1.Rows[i][0].ToString().Equals(pr.ID_Product.ToString()))
                    return new JsonResult("Entry already exists");
            }
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
    }
}
