using e_commerce.Pages.Shared.admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace e_commerce.Pages
{

    public class basketModel : PageModel
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
        public List<ShopCartCountInfo> ShopCartCountInfoList = new List<ShopCartCountInfo>();
        public List<numberproducts> numberproducts = new List<numberproducts>();

        public List<CategoryInfo> listCategories = new List<CategoryInfo>(); 
        public List<CategoryInfo2> listCategories2 = new List<CategoryInfo2>();
        public List<ShopCartInfo> listShopCart = new List<ShopCartInfo>(); 
                    public List<Total> numberoftotal = new List<Total>();

        [BindProperty]
        public string product_id { get; set; }
        [BindProperty]
        public string product_name { get; set; }
        [BindProperty]
        public string product_price { get; set; }
        [BindProperty]
        public string product_vat { get; set; }

        public void OnGet()
        {
            //Username = HttpContext.Session.GetString("user_id");
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string sql = "select category_name from categories where category_name!=' ' GROUP BY category_name";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CategoryInfo category = new CategoryInfo();
                            category.category_name = reader.GetString(0);


                            listCategories.Add(category);
                        }
                    }
                    connection.Close();
                }





                connection.Open();
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

                if(HttpContext.Session.GetString("user_id") != null)
                {
                
                        connection.Open();
                        string sql3 = "select * from shoppingcart where sc_user_id=@sc_user_id and sc_approve=0";
                        using (SqlCommand command = new SqlCommand(sql3, connection))
                        {
                        command.Parameters.AddWithValue("@sc_user_id", HttpContext.Session.GetString("user_id"));
                        using (SqlDataReader reader = command.ExecuteReader())
                            {

                                while (reader.Read()) 
                                {
                                    ShopCartInfo shopcart = new ShopCartInfo();
                                    shopcart.sc_name = reader["sc_product_name"].ToString();
                                    shopcart.sc_numberofproducts = reader["sc_numberofproducts"].ToString();
                                    shopcart.sc_price = reader["sc_product_price"].ToString();
                                    shopcart.sc_vat = reader["sc_product_vat"].ToString();
                                    shopcart.sc_total_price = reader["sc_total_price"].ToString();


                                    listShopCart.Add(shopcart);
                                }
                            }
                        }

                        connection.Close();

                    string sql7 = "SELECT COUNT(*)AS a  FROM shoppingcart where sc_user_id=@sc_user_id and sc_approve=0";
                    using (SqlCommand command = new SqlCommand(sql7, connection))
                    {
                        command.Parameters.AddWithValue("@sc_user_id", HttpContext.Session.GetString("user_id"));

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                numberproducts ps = new numberproducts();
                                ps.numberofproducts = reader["a"].ToString();
                                numberproducts.Add(ps);
                            }
                        }
                    }

                    connection.Close();




                    string sql8 = "SELECT SUM(sc_total_price) AS a FROM shoppingcart where sc_user_id=@sc_user_id and sc_approve=0";
                    using (SqlCommand command = new SqlCommand(sql8, connection))
                    {
                        command.Parameters.AddWithValue("@sc_user_id", HttpContext.Session.GetString("user_id"));

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                Total ps = new Total();
                                ps.numberoftotal = reader["a"].ToString();
                                numberoftotal.Add(ps);
                            }
                        }
                    }

                    connection.Close();
                }
            }



        }
        public void OnGetEmpty()
        {
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string sql5 = "delete from shoppingcart where sc_user_id=@sc_user_id and sc_approve!=1";

                SqlCommand cmd = new SqlCommand(sql5, connection);

                connection.Open();

                cmd.Parameters.AddWithValue("@sc_user_id", HttpContext.Session.GetString("user_id"));

                cmd.ExecuteNonQuery();

                connection.Close();
                Response.Redirect("shopping-cart?DeleteSuccessfully");
                connection.Close();

            }
        }
        public void OnPost()
        {

            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string sorgu = "Insert into shoppingcart (sc_product_name, sc_user_id, sc_product_price, sc_product_vat, sc_total_price) Values (@sc_product_name, @sc_user_id, @sc_product_price, @sc_product_vat, @sc_total_price)";

                SqlCommand cmd = new SqlCommand(sorgu, connection);

                connection.Open();
                
                    cmd.Parameters.AddWithValue("@sc_product_name", (product_name));
                    cmd.Parameters.AddWithValue("@sc_user_id", HttpContext.Session.GetString("user_id"));
                    cmd.Parameters.AddWithValue("@sc_product_price",(product_price));
                    cmd.Parameters.AddWithValue("@sc_product_vat", (product_vat));
                    cmd.Parameters.AddWithValue("@sc_total_price", (product_price));
                    cmd.ExecuteNonQuery();

                    connection.Close();
                Response.Redirect("shopping-cart");






                connection.Close();


                string sql5 = "SELECT COUNT(*) AS numberofshop FROM shoppingcart WHERE sc_user_id=@sc_user_id and sc_approve=0";
                using (SqlCommand command = new SqlCommand(sql5, connection))
                {
                    command.Parameters.AddWithValue("@sc_user_id", HttpContext.Session.GetString("user_id"));

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ShopCartCountInfo ass = new ShopCartCountInfo();
                            ass.number7 = reader["numberofshop"].ToString();
                            ShopCartCountInfoList.Add(ass);
                        }
                    }
                }

                connection.Close();

            }
           
        }
        public class ShopCartCountInfo
        {
            public string number7;


        }
    }
    public class ShopCartInfo
    {
        public string sc_name { get; set; }
        public string sc_numberofproducts { get; set; }
        public string sc_price { get; set; }
        public string sc_vat { get; set; }
        public string sc_total_price { get; set; }

    }
    public class numberproduct
    {
        public string? numberofproducts { get; set; }

    }
    public class Total
    {
        public string? numberoftotal { get; set; }

    }
}
