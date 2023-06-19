using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace e_commerce.Pages
{
    public class homeTechModel : PageModel
    {
        protected string security(string a)
        {
            string[] dangerousExpressions = { "--", ";",
                "@@", "@",
                "char", "nchar",
                "varchar", "nvarchar",
                "alter", "begin",
                "cast", "create",
                "cursor", "declare",
                "delete", "drop",
                "end", "exec",
                "execute", "fetch",
                "insert", "kill",
                "open", "select",
                "sys", "sysobjects",
                "syscolumns", "table",
                "update","<",">",
                "<>","/","!=","//" };

            foreach (string expression in dangerousExpressions)

            {
                a = Regex.Replace(a, expression, "");
            }
            return a;

        }

        public List<ShopCartCountInfo> ShopCartCountInfoList = new List<ShopCartCountInfo>();

        public List<CategoryInfo4> listCategories4 = new List<CategoryInfo4>();
        public List<CategoryInfo> listCategories = new List<CategoryInfo>();
        public List<CategoryInfo2> listCategories2 = new List<CategoryInfo2>();
        public List<ProductInfo> listProduct = new List<ProductInfo>();

        public void OnGet()
        {
            //ana kategorilerin oluþturulmasý
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {


                connection.Open();
                string sql4 = "select category_id,category_img,category_subcategory,category_name from categories WHERE category_name = @category_name and category_id != 25";
                using (SqlCommand command = new SqlCommand(sql4, connection))
                {
                    command.Parameters.AddWithValue("@category_name", security(Request.Query["p"].ToString()));
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CategoryInfo4 subcategoryD = new CategoryInfo4();
                            subcategoryD.category_id = reader["category_id"].ToString(); ;
                            subcategoryD.category_subcategory = reader["category_subcategory"].ToString(); ;
                            subcategoryD.category_name = reader["category_name"].ToString(); ;
                            subcategoryD.category_img = reader["category_img"].ToString(); ;


                            listCategories4.Add(subcategoryD);
                        }
                    }
                }
                connection.Close();
                connection.Open();
                string sql = "select category_name from categories where category_name!=' ' GROUP BY category_name";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CategoryInfo category = new CategoryInfo();
                            category.category_name = reader["category_name"].ToString();


                            listCategories.Add(category);
                        }
                    }
                }
                connection.Close();

                //alt kategorilerin oluþturulmasý
                connection.Open();
                string sql2 = "select category_id,category_subcategory,category_name from categories where category_id !=25";
                using (SqlCommand command = new SqlCommand(sql2, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CategoryInfo2 subcategory = new CategoryInfo2();
                            subcategory.category_id = reader["category_id"].ToString();
                            subcategory.category_subcategory = reader["category_subcategory"].ToString();
                            subcategory.category_name = reader["category_name"].ToString();


                            listCategories2.Add(subcategory);
                        }
                    }
                }

                connection.Close();


                //týklanan alt kategoriye göre ürünlerin getirilmesi
                connection.Open();
                 

                string sql3 = "select * from products WHERE product_category_id = @category_id";
                using (SqlCommand command = new SqlCommand(sql3, connection))
                {
                    command.Parameters.AddWithValue("@category_id", (Request.Query["id"].ToString()));
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ProductInfo product = new ProductInfo();
                            product.product_category_id = reader["product_category_id"].ToString();
                            product.product_id = reader["product_id"].ToString();
                            product.product_name = reader["product_name"].ToString();
                            product.product_price = reader["product_price"].ToString();
                            product.product_photo = reader["product_photo"].ToString();

                            listProduct.Add(product);
                        }
                    }
                }

                connection.Close();
                if (HttpContext.Session.GetString("user_id") != null)
                {

               
                string sql5 = "SELECT COUNT(*) AS numberofshop FROM shoppingcart WHERE sc_user_id=@sc_user_id and sc_approve=0";
                using (SqlCommand command = new SqlCommand(sql5, connection))
                {
                    command.Parameters.AddWithValue("@sc_user_id", HttpContext.Session.GetString("user_id"));

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ShopCartCountInfo ass = new ShopCartCountInfo();
                            ass.number5 = reader["numberofshop"].ToString();
                            ShopCartCountInfoList.Add(ass);
                        }
                    }
                }

                connection.Close();
                }


            }

        }
        public class ShopCartCountInfo
        {
            public string number5;


        }
    }
    public class CategoryInfo4
    {
        public string? category_id { get; set; } 
        public string? category_name { get; set; }
        public string? category_subcategory { get; set; }
        public string? category_img { get; set; }


    }
    public class ProductInfo
    {
        public string? product_category_id { get; set; }
        public string? product_id { get; set; }
        public string? product_name { get; set; }
        public string? product_price { get; set; }
        public string? product_photo { get; set; }


    }

}






