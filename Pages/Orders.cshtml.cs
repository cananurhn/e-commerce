using e_commerce.Pages.admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace e_commerce.Pages
{
    public class OrdersModel : PageModel
    {
        public List<ShopCartCountInfo> ShopCartCountInfoList = new List<ShopCartCountInfo>();

        public List<CategoryInfo> listCategories = new List<CategoryInfo>();
        public List<CategoryInfo2> listCategories2 = new List<CategoryInfo2>(); 
                    public List<OrderInfoUser> listuserorder = new List<OrderInfoUser>();

        public void OnGet()
        {
            if (HttpContext.Session.GetString("user_id") == null)
            {
                Response.Redirect("Index?Invalid");
            }
            try
            {
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();





                    string sql = "select category_name from categories where category_name!=' ' GROUP BY category_name";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CategoryInfo category = new CategoryInfo();
                                category.category_name = reader.GetString(0);


                                listCategories.Add(category);
                            }
                        }
                    }





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


                    string sql3 = "SELECT COUNT(*) AS numberofshop FROM shoppingcart WHERE sc_user_id=@sc_user_id and sc_approve=0";
                    using (SqlCommand command = new SqlCommand(sql3, connection))
                    {
                        command.Parameters.AddWithValue("@sc_user_id", HttpContext.Session.GetString("user_id"));

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                ShopCartCountInfo ass = new ShopCartCountInfo();
                                ass.number9 = reader["numberofshop"].ToString();
                                ShopCartCountInfoList.Add(ass);
                            }
                        }
                    }

                    connection.Close();



                    string sql7 = "SELECT * FROM shoppingcart where sc_user_id=@sc_user_id and sc_approve=1";
                    using (SqlCommand command = new SqlCommand(sql7, connection))
                    {
                        command.Parameters.AddWithValue("@sc_user_id", HttpContext.Session.GetString("user_id"));

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                OrderInfoUser order = new OrderInfoUser();
                                order.sc_product_name = reader["sc_product_name"].ToString();
                                order.sc_numberofproducts = reader["sc_numberofproducts"].ToString();
                                order.sc_product_price = reader["sc_product_price"].ToString();
                                order.sc_total_price = reader["sc_total_price"].ToString();

                                listuserorder.Add(order);
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public class ShopCartCountInfo
        {
            public string number9;


        }
    }
    public class OrderInfoUser
    {
        public string? sc_product_price { get; set; }
        public string? sc_total_price { get; set; }
        public string? sc_product_name { get; set; }
        public string? sc_numberofproducts { get; set; }
    }
}
