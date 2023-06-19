using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

using static e_commerce.Pages.admin.UsersModel;

namespace e_commerce.Pages.admin
{
    public class MessagesModel : PageModel
    {
        public List<MessageInfo> listMessages = new List<MessageInfo>();

        public void OnGet()
        {
            if (HttpContext.Session.GetString("admin_name") == null)
            {
                Response.Redirect("login?Warning");
            }
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=e-commerce;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string sql = "select * from messages";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            MessageInfo message = new MessageInfo();
                            message.message_id = reader["message_id"].ToString();
                            message.message_name = reader["message_name"].ToString();
                            message.message_email = reader["message_email"].ToString();
                            message.message_subject = reader["message_subject"].ToString();
                            message.message_desc = reader["message_desc"].ToString();


                            listMessages.Add(message);
                        }
                    }
                }

                connection.Close();







                if (Request.Query["delete_message"].ToString() != "")
                {



                    string sql2 = "delete from messages where message_id=@message_id";
                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        command.Parameters.AddWithValue("@message_id", Request.Query["delete_message"].ToString());
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        connection.Close();
                        Response.Redirect("Messages?DeleteSuccessfully");


                    }

                }

            }
        }
    }
    public class MessageInfo
    {
        public string? message_id { get; set; }
        public string? message_name { get; set; }
        public string? message_email { get; set; }
        public string? message_subject { get; set; }
        public string? message_desc { get; set; }

    }
}
