using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace e_commerce.Pages.admin
{
    public class EditUserModel : PageModel
    {
        [BindProperty]
        public string user_email { get; set; }
        [BindProperty]
        public string user_id { get; set; }
        [BindProperty]
        public string user_name { get; set; }
        [BindProperty]
        public string user_password { get; set; }
        public List<UserDetail> userDetail = new List<UserDetail>();

        public void OnGet()
        {
            if (HttpContext.Session.GetString("admin_name") == null)
            {
                Response.Redirect("login?Warning");
            }

            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string sql2 = "SELECT * FROM users where user_id=@user_id";
                using (SqlCommand command = new SqlCommand(sql2, connection))
                {
                    command.Parameters.AddWithValue("@user_id", Request.Query["user_id"].ToString());

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            UserDetail user = new UserDetail();
                            user.user_id = reader["user_id"].ToString();
                            user.user_name = reader["user_name"].ToString();
                            user.user_email = reader["user_email"].ToString();
                            user.user_password = reader["user_password"].ToString();

                            userDetail.Add(user);
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

                string sorgu = "update users set user_name=@user_name,user_email=@user_email,user_password=@user_password where user_id=@user_id";

                SqlCommand cmd = new SqlCommand(sorgu, connection);

                connection.Open();
              cmd.Parameters.AddWithValue("@user_id", user_id);

                cmd.Parameters.AddWithValue("@user_name", user_name);
                cmd.Parameters.AddWithValue("@user_email", user_email);
                cmd.Parameters.AddWithValue("@user_password", user_password);
                cmd.ExecuteNonQuery();
                connection.Close();
                Response.Redirect("Users");
                connection.Close();

            }
        }
    }
    public class UserDetail
    {

        public string? user_id { get; set; }
        public string? user_name { get; set; }
        public string? user_password { get; set; }
        public string? user_email { get; set; }
    }

}

