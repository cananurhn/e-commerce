using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace e_commerce.Pages.admin
{


    public class loginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }
        public void OnPost()
        {
            if (Email != null && Password != null)
            {
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string sorgu = "select * from admin where admin_email = @admin_email AND admin_password = @admin_password";

                    SqlCommand cmd = new SqlCommand(sorgu, connection);
                    cmd.Parameters.AddWithValue("@admin_email", Email);
                    cmd.Parameters.AddWithValue("@admin_password", Password);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        HttpContext.Session.SetString("admin_name", reader["admin_name"].ToString());
                        Response.Redirect("/admin");
                    }
                    else
                    {
                        Response.Redirect("login?invalid");

                    }
                    connection.Close();
                }
            }
        }
    }
}
