using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace e_commerce.Pages.admin
{
    public class addcategoryModel : PageModel
    {

        // sisteme resim dosyas� y�kleyebilmek i�in gerekli eklenmesi gereken asp nin s�n�f ve de�i�ken yap�s�
        private IWebHostEnvironment Environment;
        public addcategoryModel(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }


        // account.cshtml alan�nda de�inildi. BindProperty nin ne olduguna.
        [BindProperty]
        public string? categorySubcategoryName { get; set; }
        [BindProperty]
        public string? categoryName { get; set; }
        [BindProperty]
        public string? fileName { get; set; }


        public void OnGet()
        {// kullan�c� giri�i yoksa sayfa a��lmaz, direkt admin giri� ekran�na atar(g�venlik i�in.)
            if (HttpContext.Session.GetString("admin_name") == null)
            {
                Response.Redirect("login?Warning");
            }
        }
        public void OnPost(List<IFormFile> postedFiles)
        {
            // form bilgileri doldurulup(foto�raf se�me i�lemi kategori ad�,alt kategori ad� vs.) g�nderildi�inde veritaban�na gerekli kay�t i�lemlerini yapan alan.
            //Se�ilen foto�raf� wwwroot/imgUploads klas�r�ne y�kler.
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "imgUploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            //Buradan alttaki yorum sat�r�na kadar olan alan se�ilen resim dosyas�n� belirtti�imiz imgUploads klas�r�ne y�klemeyi sa�lar.
            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in postedFiles)
            {
                fileName = Path.GetFileName(postedFile.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                }
                //Bundan sonra al�nan bilgileri veritaban�na ekler
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
