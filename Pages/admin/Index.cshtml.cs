using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
namespace e_commerce.Pages.Shared.admin
{
    public class IndexModel : PageModel
    {
        public List<numberproducts> numberproducts = new List<numberproducts>();
        public List<numbermessages> numbermessages = new List<numbermessages>();
        public List<numberusers> numberusers = new List<numberusers>();
        public List<numbercategories> numbercategories = new List<numbercategories>();

        public void OnGet()
        {
            if(HttpContext.Session.GetString("admin_name") == null)
            {
            Response.Redirect("admin/login?Warning");
             }

            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string sql = "SELECT COUNT(*) AS numberofp FROM products";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            numberproducts ps = new numberproducts();
                            ps.numberofproducts = reader["numberofp"].ToString();
                            numberproducts.Add(ps);
                        }
                    }
                }

                connection.Close();



                string sql2 = "SELECT COUNT(*) AS numberofm FROM messages";
                using (SqlCommand command = new SqlCommand(sql2, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            numbermessages ps = new numbermessages();
                            ps.numberofmessages = reader["numberofm"].ToString();
                            numbermessages.Add(ps);
                        }
                    }
                }

                connection.Close();

                string sql3 = "SELECT COUNT(*) AS numberofusers FROM users";
                using (SqlCommand command = new SqlCommand(sql3, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            numberusers ps = new numberusers();
                            ps.numberofusers = reader["numberofusers"].ToString();
                            numberusers.Add(ps);
                        }
                    }
                }

                connection.Close();



                string sql4 = "SELECT COUNT(*) AS numberofcategories FROM categories";
                using (SqlCommand command = new SqlCommand(sql4, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            numbercategories ps = new numbercategories();
                            ps.numberofcategories = reader["numberofcategories"].ToString();
                            numbercategories.Add(ps);
                        }
                    }
                }

                connection.Close();

            }


        }
        public void OnGetLogout()
        {
            HttpContext.Session.Remove("admin_name");
            Response.Redirect("/admin?LogoutSuccessfully");
        }
    }
    public class numberproducts
    {
        public string? numberofproducts { get; set; }

    }
    public class numbermessages
    {
        public string? numberofmessages { get; set; }

    }
    public class numberusers
    {
        public string? numberofusers { get; set; }

    }
    public class numbercategories
    {
        public string? numberofcategories { get; set; }

    }
}
