using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace e_commerce.Pages
{
    public class productDetailModel : PageModel
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
        [BindProperty]
        public string product_id { get; set; }
        [BindProperty]
        public string product_name { get; set; }
        [BindProperty]
        public string product_price { get; set; }
        [BindProperty]
        public string product_vat { get; set; }
        public List<ShopCartCountInfo> ShopCartCountInfoList = new List<ShopCartCountInfo>();

        public List<ProductINFO> ProductInf = new List<ProductINFO>();

        public List<CategoryInfo> listCategories = new List<CategoryInfo>();
        public List<CategoryInfo2> listCategories2 = new List<CategoryInfo2>();

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
                    connection.Close();

                    connection.Open();
                   string sql2 = "select category_id,category_subcategory,category_name from categories WHERE category_name!=' '";
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


                    connection.Open();
                    string sql5 = "select * from products where product_id=@product_id";
                    using (SqlCommand command = new SqlCommand(sql5, connection))
                    {
                        command.Parameters.AddWithValue("@product_id", (Request.Query["pd"].ToString()));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductINFO pd = new ProductINFO();
                                pd.product_id = reader["product_id"].ToString();
                                pd.product_name = reader["product_name"].ToString();
                                pd.product_price = reader["product_price"].ToString();
                                pd.product_desc = reader["product_desc"].ToString();
                                pd.product_category_id = reader["product_category_id"].ToString();
                                pd.product_views = reader["product_views"].ToString();
                                pd.product_vat = reader["product_vat"].ToString();
                                pd.product_photo = reader["product_photo"].ToString();


                                // pd.product_views = reader.IsDBNull(3) ? null : reader.GetString(3);


                                ProductInf.Add(pd);
                            }
                        }
                    }

                    connection.Close();






                    connection.Open();
                    string sql6 = "update products set product_views = product_views + 1 where product_id=@product_id";
                    using (SqlCommand command = new SqlCommand(sql6, connection))
                    {
                        command.Parameters.AddWithValue("@product_id", security(Request.Query["pd"].ToString()));

                        SqlDataReader reader = command.ExecuteReader();

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
                                ass.number4 = reader["numberofshop"].ToString();
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

                string sorgu = "Insert into shoppingcart (sc_product_name, sc_user_id, sc_product_price, sc_product_vat, sc_total_price) Values (@sc_product_name, @sc_user_id, @sc_product_price, @sc_product_vat, @sc_total_price)";

                SqlCommand cmd = new SqlCommand(sorgu, connection);

                connection.Open();

                cmd.Parameters.AddWithValue("@sc_product_name", security(product_name));
                cmd.Parameters.AddWithValue("@sc_user_id", HttpContext.Session.GetString("user_id"));
                cmd.Parameters.AddWithValue("@sc_product_price", security(product_price));
                cmd.Parameters.AddWithValue("@sc_product_vat", security(product_vat));
                cmd.Parameters.AddWithValue("@sc_total_price", security(product_price));
                cmd.ExecuteNonQuery();

                connection.Close();
                Response.Redirect("shopping-cart");



                connection.Close();


            }

        }



        public class ShopCartCountInfo
        {
            public string number4;


        }
    }
    public class ProductINFO
    {
        public string? product_id { get; set; }
        public string? product_name { get; set; }
        public string? product_price { get; set; }
        public string? product_views { get; set; }
        public string? product_stock { get; set; }
        public string? product_desc { get; set; }
        public string? product_photo { get; set; }
        public string? product_category_id { get; set; }
        public string? product_vat { get; set; }


    }




}
