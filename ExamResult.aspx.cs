using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;

namespace Quiz_Assignment
{
    public partial class ExamResult : System.Web.UI.Page
    {
        string conn = "Data Source=SAWANT;Initial Catalog=Assignment;Integrated Security=True";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Retrieve the user's email saved in the cookie
                string userEmail = GetUserEmail();

                // Display the user's email
                EmailLabel.Text = userEmail;

                // Calculate and display the exam result (you can customize this logic)
                int totalQuestions = GetTotalQuestions();
                int correctAnswers = GetCorrectAnswers(userEmail);

                ResultLabel.Text = $"You answered {correctAnswers} out of {totalQuestions} questions correctly.";
                
                DisplayAllQuestionsAndAnswers(userEmail);
            }
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

        private int GetTotalQuestions()
        {
            int totalQuestions = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Questions";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        totalQuestions = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("An error occurred while retrieving the total number of questions: " + ex.Message);
            }

            return totalQuestions;
        }
        private int GetCorrectAnswers(string userEmail)
        {
            int correctAnswers = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();

                    // Define a SQL query to count the correct answers for the user
                    string query = @"
                SELECT COUNT(*) 
                FROM UserAnswers AS UA
                INNER JOIN Questions AS Q ON UA.QuestionID = Q.QuestionID
                WHERE UA.UserEmail = @UserEmail AND UA.SelectedAnswer = Q.CorrectAnswer";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserEmail", userEmail);
                        correctAnswers = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("An error occurred while counting correct answers: " + ex.Message);
            }

            return correctAnswers;
        }
        private void DisplayAllQuestionsAndAnswers(string userEmail)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();

                    // Define a SQL query to retrieve all questions and their answers for the user
                    string query = @"
                SELECT Q.QuestionText, Q.CorrectAnswer, UA.SelectedAnswer
                FROM Questions AS Q
                LEFT JOIN UserAnswers AS UA ON Q.QuestionID = UA.QuestionID AND UA.UserEmail = @UserEmail
                ORDER BY Q.QuestionID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserEmail", userEmail);

                        // Execute the query and retrieve the result
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            int questionNumber = 1;

                            while (reader.Read())
                            {
                                string questionText = reader["QuestionText"].ToString();
                                string correctAnswer = reader["CorrectAnswer"].ToString();
                                string userAnswer = reader["SelectedAnswer"].ToString();

                                // Display the question number, question text, correct answer, and user's answer
                                DisplayQuestionAndAnswers(questionNumber, questionText, correctAnswer, userAnswer);

                                questionNumber++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("An error occurred while retrieving and displaying questions and answers: " + ex.Message);
            }
        }

        private void DisplayQuestionAndAnswers(int questionNumber, string questionText, string correctAnswer, string userAnswer)
        {
            // Create labels to display the question and answers
            Label questionLabel = new Label();
            questionLabel.Text = $"Question {questionNumber}: {questionText}<br/>";
             questionLabel.Font.Bold = true;

            Label correctAnswerLabel = new Label();
            correctAnswerLabel.Text = $"Correct Answer: {correctAnswer}<br/>";

            Label userAnswerLabel = new Label();
            userAnswerLabel.Text = $"Your Answer: {userAnswer}<br/><br/>";

            // Add the labels to your page or a panel where you want to display them
            // For example, you can add them to a Panel called QuestionPanel:
            QuestionPanel.Controls.Add(questionLabel);
            QuestionPanel.Controls.Add(correctAnswerLabel);
            QuestionPanel.Controls.Add(userAnswerLabel);
        }

    }
}
