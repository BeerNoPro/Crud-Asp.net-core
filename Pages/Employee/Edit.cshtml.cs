using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCoreApp.Pages.Common;
using System.Data.SqlClient;

namespace MyCoreApp.Pages.Employee
{
    public class EditModel : PageModel
    {
        public EmployeeInfo employeeInfo = new EmployeeInfo();
        public String errorMessage = String.Empty;
        public String successMessage = String.Empty;

        private readonly IConfiguration _iconfiguration;
        public EditModel(IConfiguration configuration)
        {
            _iconfiguration = configuration;
        }

        public void OnGet()
        {
            // Get id in url 
            String id = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(_iconfiguration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clients WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                this.employeeInfo.id = "" + reader.GetInt32(0);
                                this.employeeInfo.name = reader.GetString(1);
                                this.employeeInfo.email = reader.GetString(2);
                                this.employeeInfo.phone = reader.GetString(3);
                                this.employeeInfo.address = reader.GetString(4);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            // Get info employee
            this.employeeInfo.id = Request.Form["id"];
            this.employeeInfo.name = Request.Form["name"];
            this.employeeInfo.email = Request.Form["email"];
            this.employeeInfo.phone = Request.Form["phone"];
            this.employeeInfo.address = Request.Form["address"];

            // Check not null
            if (string.IsNullOrEmpty(this.employeeInfo.name))
            {
                this.errorMessage = "Please enter your full name!";
                return;
            }

            if (string.IsNullOrEmpty(this.employeeInfo.email))
            {
                this.errorMessage = "Please enter your email!";
                return;
            }

            if (string.IsNullOrEmpty(this.employeeInfo.phone))
            {
                this.errorMessage = "Please enter your phone number!";
                return;
            }

            if (string.IsNullOrEmpty(this.employeeInfo.address))
            {
                this.errorMessage = "Please enter your address!";
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_iconfiguration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    String sql = "UPDATE clients SET name = @name, email = @email, phone = @phone, address = @address WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("name", this.employeeInfo.name);
                        command.Parameters.AddWithValue("email", this.employeeInfo.email);
                        command.Parameters.AddWithValue("phone", this.employeeInfo.phone);
                        command.Parameters.AddWithValue("address", this.employeeInfo.address);
                        command.Parameters.AddWithValue("id", this.employeeInfo.id);

                        command.ExecuteNonQuery();
                    }
                    connection.Close();

                    // Rollback view data
                    Response.Redirect("/Employee/Index");
                }
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.Message;
                return;
            }
        }
    }
}
