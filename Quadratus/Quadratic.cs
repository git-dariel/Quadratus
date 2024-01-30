using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Newtonsoft.Json;

namespace Quadratus
{
    public partial class Quadratic : Form
    {
        public Quadratic()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private async void btnGenerate_Click_1(object sender, EventArgs e)
        {
            double a, b, c;

            // Parse coefficients from textboxes
            if (!double.TryParse(txtA.Text, out a) || !double.TryParse(txtB.Text, out b) || !double.TryParse(txtC.Text, out c))
            {
                MessageBox.Show("Invalid coefficients. Please enter valid numbers.");
                return;
            }

            // Formulate the quadratic equation as a natural language query
            string query = $"Solve the equation {a}x^2 + {b}x + {c} = 0";

            // Create a JSON object to represent the payload
            var payload = new
            {
                prompt = query,
                model = "gpt-3.5-turbo" // Specify the GPT-3 model
            };

            // Serialize the payload to JSON
            string jsonPayload = JsonConvert.SerializeObject(payload);

            // Call OpenAI API to solve the equation
            string apiKey = "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";
            string apiUrl = "https://api.openai.com/v1/completions";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    // Display the result in a label or textbox
                    rtOutput.Text = result;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Failed to call OpenAI API. Status code: {response.StatusCode}\nError: {errorMessage}");
                }
            }

        }
    }
}
