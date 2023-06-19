using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace e_commerce.Pages.admin
{
    public class UsersModel : PageModel
    {
        public List<UserInfo> listUser = new List<UserInfo>();

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

                string sql = "select * from users";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            UserInfo user = new UserInfo();
                            user.user_id = reader["user_id"].ToString();
                            user.user_name = reader["user_name"].ToString();
                            user.user_email = reader["user_email"].ToString();
                            user.user_password = reader["user_password"].ToString();


                            listUser.Add(user);
                        }
                    }
                }

                connection.Close();







                if (Request.Query["delete_user"].ToString() != "")
                {



                    string sql2 = "delete from users where user_id=@user_id";
                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        command.Parameters.AddWithValue("@user_id", Request.Query["delete_user"].ToString());
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        connection.Close();
                        Response.Redirect("Users?DeleteSuccessfully");


                    }

                }

            }
        }
        public class UserInfo
        {
            public string? user_id { get; set; }
            public string? user_name { get; set; }
            public string? user_email { get; set; }
            public string? user_password { get; set; }

        }
    }
}
