using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace e_commerce.Pages.admin
{
    public class OrdersModel : PageModel
    {
        public List<OrderInfo> listOrders = new List<OrderInfo>();

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

                string sql = "SELECT * FROM users INNER JOIN shoppingcart ON users.user_id = shoppingcart.sc_user_id WHERE sc_approve=1";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            OrderInfo order = new OrderInfo();
                            order.sc_id = reader["sc_id"].ToString();
                            order.user_name = reader["user_name"].ToString();
                            order.user_email = reader["user_email"].ToString();
                            order.sc_product_name = reader["sc_product_name"].ToString();
                            order.sc_numberofproducts = reader["sc_numberofproducts"].ToString();
                            order.sc_product_price = reader["sc_product_price"].ToString();
                            order.sc_total_price = reader["sc_total_price"].ToString();

                            listOrders.Add(order);
                        }
                    }
                }

                connection.Close();







                if (Request.Query["delete_order"].ToString() != "")
                {



                    string sql2 = "delete from shoppingcart where sc_id=@sc_id";
                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        command.Parameters.AddWithValue("@sc_id", Request.Query["delete_order"].ToString());
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        connection.Close();
                        Response.Redirect("Orders?DeleteSuccessfully");


                    }

                }

            }
        }
    }
    public class OrderInfo
    {
        public string? sc_id { get; set; }
        public string? user_name { get; set; }
        public string? user_email { get; set; }
        public string? sc_product_name { get; set; }
        public string? sc_numberofproducts { get; set; }
        public string? sc_product_price { get; set; }
        public string? sc_total_price { get; set; }
    }
}
