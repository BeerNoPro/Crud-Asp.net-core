using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCoreApp.Pages.Common;
using System.Data.SqlClient;

namespace MyCoreApp.Pages.Employee
{
    public class CreateModel : PageModel
    {
        public EmployeeInfo employeeInfo = new EmployeeInfo();
        public String errorMessage = String.Empty;
        public String successMessage = String.Empty;

        private readonly IConfiguration _configuration;
        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            // get info employee
            this.employeeInfo.name = Request.Form["name"];
            this.employeeInfo.email = Request.Form["email"];
            this.employeeInfo.phone = Request.Form["phone"];
            this.employeeInfo.address = Request.Form["address"];

            // check not null
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

            // save new employee into the database
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    String sql = "INSERT INTO clients (name, email, phone, address) VALUES (@name, @email, @phone, @address)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", this.employeeInfo.name);
                        command.Parameters.AddWithValue("@email", this.employeeInfo.email);
                        command.Parameters.AddWithValue("@phone", this.employeeInfo.phone);
                        command.Parameters.AddWithValue("@address", this.employeeInfo.address);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }

                // Clear value
                this.employeeInfo.name = "";
                this.employeeInfo.email = "";
                this.employeeInfo.phone = "";
                this.employeeInfo.address = "";
                this.successMessage = "New employee added success";

                // Rollback page view
                Response.Redirect("/Employee/Index");
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.Message;
            }

        }
    }
}
