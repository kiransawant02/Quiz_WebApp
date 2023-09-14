using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;

namespace Quiz_Assignment
{
    public partial class QuestionPage : System.Web.UI.Page
    {
        string conn = "Data Source=SAWANT;Initial Catalog=Assignment;Integrated Security=True";
        private DataTable questionsTable = new DataTable();
        private int currentQuestionIndex = 0;
        private string selectedResponse;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                currentQuestionIndex = 0; // Initialize to the first question
                LoadQuestionsFromDatabase();
                DisplayCurrentQuestion();
            }
            else
            {
                // Check if questionsTable exists in ViewState
                if (ViewState["questionsTable"] != null)
                {
                    questionsTable = (DataTable)ViewState["questionsTable"];
                }
                else
                {
                    // Handle the case where questionsTable in ViewState is null or missing
                    // You can log an error or display a message to the user.
                }

                // Check if currentQuestionIndex exists in ViewState
                if (ViewState["currentQuestionIndex"] != null)
                {
                    currentQuestionIndex = (int)ViewState["currentQuestionIndex"];
                }
                else
                {
                    // Handle the case where currentQuestionIndex in ViewState is null or missing
                    // You can log an error or display a message to the user.
                }
            }
        }

        private void LoadQuestionsFromDatabase()
        {
            try
            {
                // Load questions from the database into a DataTable
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Questions", connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(questionsTable);
                }

                // Store questionsTable in ViewState
                ViewState["questionsTable"] = questionsTable;
            }
            catch (Exception ex)
            {
                // Handle database connection error, log the exception, and possibly display an error message to the user.
                // For example:
                // Response.Write("An error occurred while loading questions from the database: " + ex.Message);
                // Log the exception using a logging framework or write it to a log file.
            }
        }

        private void DisplayCurrentQuestion()
        {
            if (currentQuestionIndex < questionsTable.Rows.Count)
            {
                DataRow questionRow = questionsTable.Rows[currentQuestionIndex];
                QuestionLabel.Text = questionRow["QuestionText"].ToString();

                // Bind answer choices to RadioButtonList
                AnswerChoices.Items.Clear();
                for (int i = 1; i <= 4; i++)
                {
                    string answerChoice = questionRow["Answer" + i].ToString();
                    if (!string.IsNullOrEmpty(answerChoice))
                    {
                        AnswerChoices.Items.Add(new ListItem(answerChoice));
                    }
                }
            }
            else
            {
                // No more questions, quiz completed
                QuestionLabel.Text = "Quiz Completed!";
                AnswerChoices.Visible = false;
                NextButton.Visible = false;
            }
        }

        protected void NextButton_Click(object sender, EventArgs e)
        {
            // Get the user's email address from the TextBox
            string userEmail = GetUserEmail();

            // Validate that an email address is provided (you can add more robust validation)
            if (string.IsNullOrEmpty(userEmail))
            {
                Response.Write("Please enter your email address.");
                return;
            }

            // Get the selected response from the RadioButtonList
             selectedResponse = AnswerChoices.SelectedValue;

            // Save the user's response to the database
            SaveResponseToDatabase(userEmail, selectedResponse);

            // Increment the question index only if there are more questions
            if (currentQuestionIndex < questionsTable.Rows.Count - 1)
            {
                currentQuestionIndex++;
                ViewState["currentQuestionIndex"] = currentQuestionIndex; // Save the updated index
            }
            else
            {
                Response.Redirect("ExamResult.aspx");
                //Response.Write("No more questions to display."); 
            }

            // Display the next question or quiz completion message
            DisplayCurrentQuestion();
        }


        private string GetUserEmail()
        {
            HttpCookie userCookie = Request.Cookies["user_email"];
            if (userCookie != null)
            {
                return userCookie.Value;
            }
            else
            {
                // If the cookie doesn't exist or is empty, you can handle it here
                // For example, you can redirect the user to the login page or display an error message
                Response.Redirect("LoginPage.aspx"); // Redirect to the login page
                return null; // Or return an appropriate default value
            }
        }

        private void SaveResponseToDatabase(string email, string response)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO UserAnswers (QuestionID, UserEmail, SelectedAnswer) VALUES (@QuestionID, @UserEmail, @SelectedAnswer)", connection);
                    cmd.Parameters.AddWithValue("@QuestionID", currentQuestionIndex+1);
                    cmd.Parameters.AddWithValue("@UserEmail", email);
                    cmd.Parameters.AddWithValue("@SelectedAnswer", selectedResponse);
                    int K = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Handle database connection error, log the exception, and possibly display an error message to the user.
                // For example:
                // Response.Write("An error occurred while saving the response to the database: " + ex.Message);
                // Log the exception using a logging framework or write it to a log file.
            }
        }


    }
}
