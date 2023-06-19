using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace e_commerce.Pages
{
    public class contactModel : PageModel
    {
        protected string security(string a)
        {
            string[] dangerousExpressions = { "--", ";",
                "@@", 
                "char", "nchar",
                "varchar", "nvarchar",
                "alter", "begin",
                "cast", "create",
                "cursor", "declare",
                "delete", "drop",
                "end", "exec",
                "execute", "fetch",
                "insert", "kill",
                "open", "select",
                "sys", "sysobjects",
                "syscolumns", "table",
                "update","<",">",
                "<>","/","==","!=" };

            foreach (string expression in dangerousExpressions)

            {
                a = Regex.Replace(a, expression, "x");
            }
            return a;

        }
        public List<ShopCartCountInfo> ShopCartCountInfoList = new List<ShopCartCountInfo>();

        public List<CategoryInfo> listCategories = new List<CategoryInfo>();
        public List<CategoryInfo2> listCategories2 = new List<CategoryInfo2>();

        [BindProperty]
        public string? ContactName { get; set; }
        [BindProperty]
        public string? ContactEmail { get; set; }
        [BindProperty]
        public string? ContactSubject { get; set; }
        [BindProperty]
        public string? ContactMessage { get; set; }


        public void OnGet()
        {
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
                                ass.number3 = reader["numberofshop"].ToString();
                                ShopCartCountInfoList.Add(ass);
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
        public void OnPost()
        {
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string sorgu = "Insert into messages (message_name, message_email, message_subject,message_desc) Values (@message_name, @message_email, @message_subject,@message_desc)";

                    SqlCommand cmd = new SqlCommand(sorgu, connection);

                    connection.Open();
                    try
                    {
                        cmd.Parameters.AddWithValue("@message_name", security(ContactName));
                        cmd.Parameters.AddWithValue("@message_email", security(ContactEmail));
                        cmd.Parameters.AddWithValue("@message_subject", security(ContactSubject));
                        cmd.Parameters.AddWithValue("@message_desc", security(ContactMessage));
                        cmd.ExecuteNonQuery();

                        Response.Redirect("contact?SendingSuccessfully");
                        connection.Close();
                    }
                    catch
                    {
                        Response.Redirect("Index?invalid");

                    }


                    connection.Close();

                }
            
        }
        public class ShopCartCountInfo
        {
            public string number3;


        }
    }

}
