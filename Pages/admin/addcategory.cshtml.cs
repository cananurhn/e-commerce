using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace e_commerce.Pages.admin
{
    public class addcategoryModel : PageModel
    {

        // sisteme resim dosyasý yükleyebilmek için gerekli eklenmesi gereken asp nin sýnýf ve deðiþken yapýsý
        private IWebHostEnvironment Environment;
        public addcategoryModel(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }


        // account.cshtml alanýnda deðinildi. BindProperty nin ne olduguna.
        [BindProperty]
        public string? categorySubcategoryName { get; set; }
        [BindProperty]
        public string? categoryName { get; set; }
        [BindProperty]
        public string? fileName { get; set; }


        public void OnGet()
        {// kullanýcý giriþi yoksa sayfa açýlmaz, direkt admin giriþ ekranýna atar(güvenlik için.)
            if (HttpContext.Session.GetString("admin_name") == null)
            {
                Response.Redirect("login?Warning");
            }
        }
        public void OnPost(List<IFormFile> postedFiles)
        {
            // form bilgileri doldurulup(fotoðraf seçme iþlemi kategori adý,alt kategori adý vs.) gönderildiðinde veritabanýna gerekli kayýt iþlemlerini yapan alan.
            //Seçilen fotoðrafý wwwroot/imgUploads klasörüne yükler.
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "imgUploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            //Buradan alttaki yorum satýrýna kadar olan alan seçilen resim dosyasýný belirttiðimiz imgUploads klasörüne yüklemeyi saðlar.
            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in postedFiles)
            {
                fileName = Path.GetFileName(postedFile.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                }
                //Bundan sonra alýnan bilgileri veritabanýna ekler
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string sorgu = "Insert into categories (category_name,category_img,category_subcategory) Values (@category_name,@category_img,@category_subcategory)";

                    SqlCommand cmd = new SqlCommand(sorgu, connection);

                    connection.Open();
                    try
                    {
                        cmd.Parameters.AddWithValue("@category_name", categoryName);
                        cmd.Parameters.AddWithValue("@category_img", fileName); 
                            cmd.Parameters.AddWithValue("@category_subcategory", categorySubcategoryName);
                        cmd.ExecuteNonQuery();

                        Response.Redirect("Categories");
                        connection.Close();
                    }
                    catch
                    {
                        Response.Redirect("Categories?invalid");

                    }
                    connection.Close();

                }

            }
        }
    }
}
