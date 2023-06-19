using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace e_commerce.Pages.admin
{
    public class CategoriesModel : PageModel
    {
        public List<CategoryInfo> listCategories = new List<CategoryInfo>();

        public void OnGet()
        {
            if (HttpContext.Session.GetString("admin_name") == null)
            {
                Response.Redirect("login?Warning");
            }
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string sql = "select * from categories where category_name !=' '";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            CategoryInfo category = new CategoryInfo();
                            category.category_id = reader["category_id"].ToString();
                            category.category_name = reader["category_name"].ToString();
                            category.category_img = reader["category_img"].ToString();
                            category.category_subcategory = reader["category_subcategory"].ToString();


                            listCategories.Add(category);
                        }
                    }
                }

                connection.Close();


                if (Request.Query["delete_category"].ToString() != "")
                {



                    string sql2 = "delete from categories where category_id=@category_id";
                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        command.Parameters.AddWithValue("@category_id", Request.Query["delete_category"].ToString());
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        connection.Close();
                        


                    }

                    string sql3 = "UPDATE products SET product_category_id=25 WHERE product_category_id=@product_category_id";
                    using (SqlCommand command = new SqlCommand(sql3, connection))
                    {
                        command.Parameters.AddWithValue("@product_category_id", Request.Query["delete_category"].ToString());
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        connection.Close();
                        Response.Redirect("Categories?DeleteSuccessfully");

                    }

                }
            }




        }
    }
    public class CategoryInfo
    {
        public string? category_id { get; set; }
        public string? category_name { get; set; }
        public string? category_img { get; set; }
        public string? category_subcategory { get; set; }

    }
}
