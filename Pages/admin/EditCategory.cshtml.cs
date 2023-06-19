using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

using static e_commerce.Pages.admin.EditProductModel;

namespace e_commerce.Pages.admin
{
    public class EditCategoryModel : PageModel
    {
        [BindProperty]
        public string category_id { get; set; }
        [BindProperty]
        public string category_name { get; set; }
        [BindProperty]
        public string category_subcategory { get; set; }
        public List<CategoryDetail> categoryDetail = new List<CategoryDetail>();

        public void OnGet()
        {
            if (HttpContext.Session.GetString("admin_name") == null)
            {
                Response.Redirect("login?Warning");
            }

            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string sql2 = "SELECT * FROM categories where category_id=@category_id";
                using (SqlCommand command = new SqlCommand(sql2, connection))
                {
                    command.Parameters.AddWithValue("@category_id", Request.Query["category_id"].ToString());

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            CategoryDetail product = new CategoryDetail();
                            product.category_id = reader["category_id"].ToString();
                            product.category_name = reader["category_name"].ToString();
                            product.category_subcategory = reader["category_subcategory"].ToString();

                            categoryDetail.Add(product);
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

                string sorgu = "update categories set category_name=@category_name,category_subcategory=@category_subcategory where category_id=@category_id";

                SqlCommand cmd = new SqlCommand(sorgu, connection);

                connection.Open();
                //try
                ///  {
                cmd.Parameters.AddWithValue("@category_name", category_name);
                cmd.Parameters.AddWithValue("@category_subcategory", category_subcategory);
                cmd.Parameters.AddWithValue("@category_id", category_id);


                cmd.ExecuteNonQuery();
                connection.Close();
                Response.Redirect("Categories");

                // }
                // catch
                // {
                // Response.Redirect("Products?invalid");

                //}
                connection.Close();

            }
        }
    }
    public class CategoryDetail
    {

        public string? category_id { get; set; }
        public string? category_name { get; set; }
        public string? category_subcategory { get; set; }
    }
}
