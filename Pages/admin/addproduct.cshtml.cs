using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

namespace e_commerce.Pages.admin
{
    public class addproductModel : PageModel
    {
        public List<listcategory> listCategory = new List<listcategory>();

        private IWebHostEnvironment Environment;

        public addproductModel(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }
        [BindProperty]
        public string? selectmainCategory { get; set; }
        [BindProperty]
        public string? fileName { get; set; }
        [BindProperty]
        public string? productName { get; set; }
        [BindProperty]
        public string? productStock { get; set; }
        [BindProperty]
        public string? productPrice { get; set; }
        [BindProperty]
        public string? productVat { get; set; }
        [BindProperty]
        public string? productDesc { get; set; }
        public void OnGet()
        {
            if (HttpContext.Session.GetString("admin_name") == null)
            {
                Response.Redirect("login?Warning");
            }

            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "select * from categories";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            listcategory category = new listcategory();
                            category.category_id = reader["category_id"].ToString();
                            category.category_subcategory = reader["category_subcategory"].ToString();


                            listCategory.Add(category);
                        }
                    }
                }
                connection.Close();

            }
            }
        public void OnPost(List<IFormFile> postedFiles)
        {
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "imgUploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in postedFiles)
            {
                 fileName = Path.GetFileName(postedFile.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                }







                    string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {

                    


                    string sorgu = "Insert into products (product_name,product_photo,product_price,product_stock,product_vat,product_desc,product_category_id) Values (@product_name,@product_photo,@product_price,@product_stock,@product_vat,@product_desc,@product_category_id)";

                        SqlCommand cmd = new SqlCommand(sorgu, connection);

                        connection.Open();
                        try
                        {
                            cmd.Parameters.AddWithValue("@product_name", productName);
                            cmd.Parameters.AddWithValue("@product_photo", fileName); 
                            cmd.Parameters.AddWithValue("@product_price", productPrice);
                            cmd.Parameters.AddWithValue("@product_stock", productStock);
                            cmd.Parameters.AddWithValue("@product_vat", productVat);
                            cmd.Parameters.AddWithValue("@product_desc", productDesc);
                            cmd.Parameters.AddWithValue("@product_category_id", selectmainCategory);

                        cmd.ExecuteNonQuery();

                            Response.Redirect("/admin");
                            connection.Close();
                        }
                        catch
                        {
                            Response.Redirect("/admin?invalid");

                        }


                        connection.Close();

                    }
                










            }
        }

    }
    public class listcategory
    {

        public string category_id { get; set; }
        public string category_subcategory { get; set; }

    }
}
