using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace e_commerce.Pages.admin
{
    public class ProductsModel : PageModel
    {
        public List<ProductsInfo> listProduct = new List<ProductsInfo>();

        public void OnGet()
        {
            if (HttpContext.Session.GetString("admin_name") == null)
            {
                Response.Redirect("login?Warning");
            }
            //ana kategorilerin oluþturulmasý
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string sql = "select * from products";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ProductsInfo product = new ProductsInfo();
                            product.product_id = reader["product_id"].ToString();
                            product.product_name = reader["product_name"].ToString();
                            product.product_price = reader["product_price"].ToString();
                            product.product_views = reader["product_views"].ToString();
                            product.product_photo = reader["product_photo"].ToString();
                            product.product_stock = reader["product_stock"].ToString();
                            product.product_vat = reader["product_vat"].ToString();
                            product.product_category_id = reader["product_category_id"].ToString();


                            listProduct.Add(product);
                        }
                    }
                }

                connection.Close();







                if (Request.Query["delete_product"].ToString() != "")
                {

                

                string sql2 = "delete from products where product_id=@product_id";
                using (SqlCommand command = new SqlCommand(sql2, connection))
                {
                    command.Parameters.AddWithValue("@product_id", Request.Query["delete_product"].ToString());
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    connection.Close();
                    Response.Redirect("Products?DeleteSuccessfully");


                }

                }






            }
        }
    }
    public class ProductsInfo
    {
        public string? product_id { get; set; }
        public string? product_name { get; set; }
        public string? product_price { get; set; }
        public string? product_views { get; set; }
        public string? product_photo { get; set; }
        public string? product_stock { get; set; }
        public string? product_vat { get; set; }
        public string? product_category_id { get; set; }
    }
}
