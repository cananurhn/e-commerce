using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace e_commerce.Pages
{
    public class searchModel : PageModel
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
        public List<SearchProductInfo> searchProductInfo = new List<SearchProductInfo>();
        public string? Msg { get; set; }
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
                                category.category_name = reader["category_name"].ToString();


                                listCategories.Add(category);
                            }
                        }
                    }
                    connection.Close();


                    string sql2 = "select category_id,category_subcategory,category_name from categories where category_id !=25";
                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        connection.Open();
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


                    //içerisinde arama kelimesinden gelen deðerin ürünler tablosunda olup olmadýgýný kontrol eden sql sorgusu
                    string sql3 = "select * from products WHERE product_name LIKE '%'+@search+'%'";
                    using (SqlCommand command = new SqlCommand(sql3, connection))
                    {
                        command.Parameters.AddWithValue("@search", security(Request.Query["SearchText"].ToString()));
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                SearchProductInfo sproduct = new SearchProductInfo();
                                sproduct.product_id = reader["product_id"].ToString();
                                    sproduct.product_name = reader["product_name"].ToString();
                                    sproduct.product_price = reader["product_price"].ToString();
                                sproduct.product_photo = reader["product_photo"].ToString();

                                searchProductInfo.Add(sproduct);
                            }
                        }
                    
                    connection.Close();
                    }





                }
            }
            catch (Exception ex)
            {

            }
        }


    }
    public class SearchProductInfo
    {
        public string? product_id { get; set; }
        public string? product_name { get; set; }
        public string? product_price { get; set; }
        public string? product_photo { get; set; }


    }

}
