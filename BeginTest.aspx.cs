using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Quiz_Assignment
{
    public partial class BeginTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void beginexambtnclick(object sender, EventArgs e)
        {
                string userEmail = txtemail.Text;

                if (IsValidEmail(userEmail))
                {
                    Response.Cookies["user_email"].Value = userEmail;
                    Response.Redirect("QuestionPage.aspx");
                }
            else
            {
                string errorMessage = "Invalid email format. Please enter a valid email address.";
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + errorMessage + "');", true);
            }
        }

        private bool IsValidEmail(string email)
        {
            string emailRegex = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailRegex);
        }
    }
}