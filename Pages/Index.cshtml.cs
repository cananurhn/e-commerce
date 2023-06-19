using e_commerce.Pages.Shared.admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace e_commerce.Pages
{
    
    public class IndexModel : PageModel
    {
        protected string security(string a)
        {
            string[] dangerousExpressions = { "--", ";",
                "@@", "@",
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
        public List<CategoryInfo> listCategories = new List<CategoryInfo>();
        public List<CategoryInfo2> listCategories2 = new List<CategoryInfo2>();
        public List<ShopCartCountInfo> ShopCartCountInfoList = new List<ShopCartCountInfo>();

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        //public object? Username { get; set; }

        public void OnGet()
        {
            //Username = HttpContext.Session.GetString("user_id");

            


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





                    string sql2 = "select category_id,category_img,category_subcategory,category_name from categories where category_id !=25";
                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CategoryInfo2 subcategory = new CategoryInfo2();
                                subcategory.category_id= reader["category_id"].ToString();
                                subcategory.category_subcategory = reader["category_subcategory"].ToString();
                                subcategory.category_name = reader["category_name"].ToString();
                                subcategory.category_img = reader["category_img"].ToString();


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
                                ass.number = reader["numberofshop"].ToString();
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
        public class ShopCartCountInfo
        {
            public string number;


        }
        //çıkış yap denildiğinde çalışan fonksiyon.
        public void OnGetLogout()
        {
            HttpContext.Session.Remove("user_id");
            Response.Redirect("Index?LogoutSuccessfully");
        }
    }
    public class CategoryInfo
    {
        public string? category_name;
        public string? category_subcategory;


    }
    public class CategoryInfo2
    {
        public string category_id;
        public string? category_name;
        public string? category_subcategory;
        public string? category_img;


    }


}