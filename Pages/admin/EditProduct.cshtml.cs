using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace e_commerce.Pages.admin
{


    public class EditProductModel : PageModel
    {

        public string a {  get; set; }  
        public class ProductDetail
        {

            public string? product_id { get; set; }
            public string? product_name { get; set; }
            public string? product_price { get; set; }
            public string? product_stock { get; set; }
            public string? product_photo { get; set; }
            public string? product_vat { get; set; }
            public string? category_subcategory { get; set; }
            public string? product_desc { get; set; }
        }
        [BindProperty]
        public string? product_id { get; set; }
        [BindProperty]
        public string? product_name { get; set; }
        [BindProperty]
        public string? product_price { get; set; }
        [BindProperty]
        public string? product_stock { get; set; }
        [BindProperty]
        public string? product_vat { get; set; }
        [BindProperty]
        public string? product_category_id { get; set; }
        [BindProperty]
        public string? product_desc { get; set; }

        public List<mainCategoryInfo> listMainCategories = new List<mainCategoryInfo>();
        public List<ProductDetail> Detail = new List<ProductDetail>();


        public void OnGet()
        {
            if (HttpContext.Session.GetString("admin_name") == null)
            {
                Response.Redirect("login?Warning");
            }
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string sql = "select * from categories where category_id != 25";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            mainCategoryInfo category = new mainCategoryInfo();
                            category.category_id = reader["category_id"].ToString();
                            category.category_name = reader["category_name"].ToString();
                            category.category_subcategory = reader["category_subcategory"].ToString();


                            listMainCategories.Add(category);
                        }
                    }
                }

                connection.Close();





                string sql2 = "SELECT * FROM categories INNER JOIN products ON categories.category_id = products.product_category_id where product_id = @product_id";
                using (SqlCommand command = new SqlCommand(sql2, connection))
                {
                    command.Parameters.AddWithValue("@product_id", Request.Query["product_id"].ToString());
                    
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ProductDetail product = new ProductDetail();
                            product.product_id = reader["product_id"].ToString();
                            product.product_name = reader["product_name"].ToString();
                            product.product_desc = reader["product_desc"].ToString();
                            product.product_price = reader["product_price"].ToString();
                            product.product_stock = reader["product_stock"].ToString();
                            product.product_photo = reader["product_photo"].ToString();
                            product.product_vat = reader["product_vat"].ToString();
                            product.category_subcategory = reader["category_subcategory"].ToString();

                            Detail.Add(product);
                        }
                    }
                }

                connection.Close();

            }



        }







        public void OnPost()
        {


                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string sorgu = "update products set product_name=@product_name, product_price=@product_price, product_stock=@product_stock, product_vat=@product_vat, product_category_id=@product_category_id, product_desc=@product_desc where product_id=@product_id";

                    SqlCommand cmd = new SqlCommand(sorgu, connection);

                    connection.Open();
//try
                  ///  {
                        cmd.Parameters.AddWithValue("@product_name", product_name);
                        cmd.Parameters.AddWithValue("@product_price", product_price);
                        cmd.Parameters.AddWithValue("@product_stock", product_stock);
                        cmd.Parameters.AddWithValue("@product_vat", product_vat);
                        cmd.Parameters.AddWithValue("@product_category_id", product_category_id);
                        cmd.Parameters.AddWithValue("@product_desc", product_desc);
                        cmd.Parameters.AddWithValue("@product_id", product_id);

                        cmd.ExecuteNonQuery();
                    connection.Close();
                    Response.Redirect("Products");
                        
                   // }
                   // catch
                   // {
                       // Response.Redirect("Products?invalid");

                    //}
                    connection.Close();

                }

            }
        }






    public class mainCategoryInfo
    {
        public string? category_id { get; set; }
        public string? category_name { get; set; }
        public string? category_subcategory { get; set; }
        public string category_img { get; set; }

    }


}

    
