using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace e_commerce.Pages
{
    public class PaymentModel : PageModel
    {
        public List<CategoryInfo> listCategories = new List<CategoryInfo>();
        public List<CategoryInfo2> listCategories2 = new List<CategoryInfo2>();
        public void OnGet()
        {//Username = HttpContext.Session.GetString("user_id");

            if(HttpContext.Session.GetString("user_id") == null)
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
                                category.category_name = reader["category_name"].ToString();


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





                }
            }
            catch (Exception ex)
            {

            }
        }


        public void OnGetApprove()
        {
             //anlýk zamaný veren asp fonksiyonu

            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string sql5 = "update  shoppingcart set sc_approve=1, sc_date=GETDATE() where sc_user_id=@sc_user_id";

                SqlCommand cmd = new SqlCommand(sql5, connection);
                connection.Open();



                //sql5 sorgusunda @scuser id yerine sessiondan gelen kullanýcý idsini ekle demek.
                cmd.Parameters.AddWithValue("@sc_user_id", HttpContext.Session.GetString("user_id"));

                

                cmd.ExecuteNonQuery();

                connection.Close();
                Response.Redirect("shopping-cart?ApproveSuccessfully");
                connection.Close();

            }
        }

    }
}
