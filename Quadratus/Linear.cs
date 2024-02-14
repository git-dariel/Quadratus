using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Net.Http;

namespace Quadratus
{
    public partial class Linear : Form
    {
        DateTime currentDateTime;
        string formattedDateTime;
        string equationText = "Linear Equation";
        //int userIdCounter = 1;

        public Linear()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            currentDateTime = DateTime.Now;
            formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=quadratus;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            //int userID = userIdCounter++;

            //Show the loading message
            rtOutput.Text = "Generating, please wait...\n\n";

            double a, b;

            // Parse coefficients from textboxes
            if (!double.TryParse(txtA.Text, out a) || !double.TryParse(txtB.Text, out b))
            {
                MessageBox.Show("Invalid coefficients. Please enter valid numbers.");
                //Clear the loading message
                rtOutput.Clear();
                return;
            }

            // Formulate the quadratic equation as a natural language query
            string query = $"Solve the equation {a}x + {b} = 0. And then give me the answer make it professional." +
                $"After that provide a step by step on how you solve that problem. Indicate the Answer in the last output.";

            // Create a JSON object to represent the payload
            var payload = new
            {
                model = "gpt-3.5-turbo-1106", // Specify the GPT-3 model
                messages = new[] { new { role = "system", content = "" }, new { role = "user", content = query } }
            };

            // Serialize the payload to JSON
            string jsonPayload = JsonConvert.SerializeObject(payload);

            // Call OpenAI API to solve the equation
            string apiKey = "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";
            string apiUrl = "https://api.openai.com/v1/chat/completions";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    // Deserialize the JSON response
                    var responseObj = JsonConvert.DeserializeObject<dynamic>(result);

                    // Assuming the last message in the "choices" array contains the answer
                    string answer = responseObj.choices[0].message.content.ToString();

                    foreach (char character in answer)
                    {
                        rtOutput.AppendText(character.ToString());
                        await Task.Delay(10);
                    }

                    addQuadraticEquation(equationText, answer, formattedDateTime);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Failed to call OpenAI API. Status code: {response.StatusCode}\nError: {errorMessage}");
                    rtOutput.Clear();
                }
            }
        }

        public void addQuadraticEquation(string equation, string output, string datetime)
        {
            try
            {
                con.Open();
                string query = "INSERT INTO QuadratusEquations (EquationText, SolutionText, SolveDateTime)" +
                    "VALUES (@EquationText, @SolutionText, @SolveDateTime)";
                SqlCommand com = new SqlCommand(query, con);
                //com.Parameters.AddWithValue("@userId", userId);
                com.Parameters.AddWithValue("@EquationText", equation);
                com.Parameters.AddWithValue("@SolutionText", output);
                com.Parameters.AddWithValue("@SolveDateTime", datetime);
                com.ExecuteNonQuery();
                MessageBox.Show("Generated Successfully!", "Complete!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Ask for confirmation before clearing the text
            DialogResult result = MessageBox.Show("Are you sure you want to clear the text?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // If the user confirms, clear the text
            if (result == DialogResult.Yes)
            {
                rtOutput.Clear();
                txtA.Clear();
                txtB.Clear();
            }
        }

        private void pbHistory_Click(object sender, EventArgs e)
        {
            this.Hide();
            History history = new History();
            history.Show();
        }

        private void pbProfile_Click(object sender, EventArgs e)
        {
            // Ask for confirmation before clearing the text
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // If the user confirms, clear the text
            if (result == DialogResult.Yes)
            {
                this.Hide();
                Form1 form = new Form1();
                form.Show();
            }
        }
    }
}
