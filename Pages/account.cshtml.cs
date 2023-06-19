using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace e_commerce.Pages
{
    public class accountModel : PageModel
    {
        // Bu fonksiyon g�venlik fonksiyonudur. �stenmeyen verilerin girilmesini/g�nderilmesini engeller.
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
                "<>","/","==","!="};

            foreach (string expression in dangerousExpressions)

            {
                a = Regex.Replace(a, expression, "x");
            }
            return a;

        }

        // bu listeler bizim de�i�kenlerimizin tan�ml� oldu�u listelerdir(bu sayfan�n en alt�na bak�n s�n�flar� g�receksiniz.)
        //i�lemler i�in s�n�f ve liste yap�s�n� kullanmak bize kolayl�k ve dinamiklik sa�lar.
        public List<ShopCartCountInfo> ShopCartCountInfoList = new List<ShopCartCountInfo>();

        //bu iki listenin class yap�s� index.cshtml.cs dosyas�nda tan�mlanm��t�r bilgileri oradaki sayfadan  (en alt�na) �eker oraya bakabilirsiniz
        public List<CategoryInfo> listCategories = new List<CategoryInfo>();
        public List<CategoryInfo2> listCategories2 = new List<CategoryInfo2>();




        // Bind property ile tan�mlanan string de�i�kenler, bunlar bu sayfan�n (account.cshtml) form tag�n�n alt�ndaki asp-for attrib�tlerinden gelen datalar� tutar.
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }


        // �stteki a��klaman�n ayn�d�r sadece login formundan gelen datalar�n asp-for attrib�tlerini tutar.
        [BindProperty]
        public string registerUsername { get; set; }
        [BindProperty]
        public string registerEmail { get; set; }
        [BindProperty]
        public string registerPassword{ get; set; }

        // normal bir de�i�kendir cshtml dosyas�nda ekrana mesaj yazd�rmam�z� sa�lar.
        public string? Msg { get; set; }

        public void OnGet()
        {
            
            try
            {   //kategori isimlerini �eken sqp sorgusu
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


                    //alt kategorileri �eken sql sorgusu
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
                    // sepetteki �r�n say�s�n� �eken sql sorgusu
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
                                ass.number2 = reader["numberofshop"].ToString();
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

                //giri� yap�lmak istendi�inde g�nderilen verileri do�rulay�p �yle bir kullan�c�n�n sistemde olup olmad�g�n� kontrol eden sorgu.
                if (Email != null && Password != null)
                {
                    string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {

                        string sorgu = "select * from users where user_email = @user_email AND user_password = @user_password";

                        SqlCommand cmd = new SqlCommand(sorgu, connection);
                        cmd.Parameters.AddWithValue("@user_email", security(Email));
                        cmd.Parameters.AddWithValue("@user_password", security(Password));

                        connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                        // giri� yap�ld��� anda o kullan�c�ya ait id ile bir session olu�turur.
                            HttpContext.Session.SetString("user_id", reader["user_id"].ToString());
                        //ve response redirect ile sepet sayfas�na g�nderir default olarak.
                            Response.Redirect("shopping-cart");
                        }
                        else
                        {
                            Response.Redirect("account?invalid");

                        }
                        connection.Close();
                    }
                }

            //giri�/kay�t yap�lmak istendi�inde g�nderilen verileri do�rulay�p kullan�c�lar tablosuna ekleme i�lemi.

            if (registerEmail != null && registerPassword != null && registerUsername != null)
            {
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string sorgu = "Insert into users (user_name, user_email, user_password) Values (@user_name, @user_email, @user_password)";

                    SqlCommand cmd = new SqlCommand(sorgu, connection);

                    connection.Open();
                    try
                    {
                        cmd.Parameters.AddWithValue("@user_name", security(registerUsername));
                        cmd.Parameters.AddWithValue("@user_email", security(registerEmail));
                        cmd.Parameters.AddWithValue("@user_password", security(registerPassword));
                        cmd.ExecuteNonQuery();
                        
                        Response.Redirect("shopping-cart?RegisterSuccessfully");
                        connection.Close();
                    }
                    catch
                    {
                        Msg = "Invalid.";
                        Response.Redirect("account?invalid");

                    }


                    connection.Close();

                }
            }
        }

    }
    // en �stte bahsetti�im s�n�f yap�s�
    public class ShopCartCountInfo
    {
        public string number2;


    }

}
