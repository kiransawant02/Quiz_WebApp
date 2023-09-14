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
                if (ViewState["questionsTable"] != null)
                {
                    questionsTable = (DataTable)ViewState["questionsTable"];
                }
                else
                {
                }

                if (ViewState["currentQuestionIndex"] != null)
                {
                    currentQuestionIndex = (int)ViewState["currentQuestionIndex"];
                }
                else
                {
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
                Response.Write("An error occurred while loading questions from the database: " + ex.Message);
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

            string userEmail = GetUserEmail();

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
            }
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
                Response.Redirect("LoginPage.aspx");
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
                    cmd.Parameters.AddWithValue("@QuestionID", currentQuestionIndex + 1);
                    cmd.Parameters.AddWithValue("@UserEmail", email);
                    cmd.Parameters.AddWithValue("@SelectedAnswer", selectedResponse);
                    int K = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Response.Write("An error occurred while saving the response to the database: " + ex.Message);
            }
        }


    }
}
