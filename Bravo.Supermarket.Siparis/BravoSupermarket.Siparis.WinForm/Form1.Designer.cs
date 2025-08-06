using BravoSupermarket.Siparis.WinForm.Dtos;
using BravoSupermarket.Siparis.WinForm.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;

namespace BravoSupermarket.Siparis.WinForm
{
    partial class Form1 : Form
    {
        private readonly HttpClient _httpClient;

        private System.ComponentModel.IContainer components = null;
        private Button button1;
        private Button button2;
        private TextBox textBox1;
        private ListBox listBox1;
        private ListBox listBox2;
        private OpenFileDialog openFileDialog1;
        private Label labelLoading;
        List<string> files = new List<string>();

        public Form1()
        {
            InitializeComponent();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7164/") 
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            openFileDialog1.Filter = "XML files (*.xml)|*.xml";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            listBox1.Items.Clear();

            foreach (var filePath in openFileDialog1.FileNames)
            {
                files.Add(filePath);
                listBox1.Items.Add(Path.GetFileName(filePath));
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {

            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("Xaiş edirik XML faylı seçin.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            button2.Enabled = false;
            labelLoading.Visible = true;
            Refresh();

            try
            {
                string mesmerCode = textBox1.Text;


                foreach (var item in files)
                {

                    string filePath = item.ToString();
                    var orderDto = XmlParserHelper.ParseXml(filePath);


                    var jsonContent = JsonConvert.SerializeObject(orderDto);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    string url = $"api/Operation/ReceiveOrder?mesmer_code={Uri.EscapeDataString(mesmerCode)}";

                    var response = await _httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();

                        listBox2.Items.Add(Path.GetFileName(filePath));

                        MessageBox.Show("Sifariş uğurla göndərildi!\n" + responseBody,
                                        "Uğurlu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Xəta baş verdi: {response.StatusCode}\n{errorContent}",
                                        "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show($"Xəta baş verdi: {ex.Message}", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                labelLoading.Visible = false;
                button2.Enabled = true;
            }

        }



        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region WinForm Designer generated

        private Label label1;
        private Label label2;

        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            listBox1 = new ListBox();
            listBox2 = new ListBox();
            label1 = new Label();
            label2 = new Label();
            openFileDialog1 = new OpenFileDialog();
            SuspendLayout();

            // button1
            button1.Location = new Point(400, 30);
            button1.Size = new Size(200, 50);
            button1.Name = "button1";
            button1.TabIndex = 0;
            button1.Text = "Faylları seçin.";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;

            // button2
            button2.Location = new Point(650, 30);
            button2.Size = new Size(200, 50);
            button2.Name = "button2";
            button2.TabIndex = 1; 
            button2.Text = "İcra edin.";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;

            Label label3 = new Label();
            label3.Text = "Mesmer kodu:"; 
            label3.Location = new Point(50, 10);
            label3.Size = new Size(200, 20);
            label3.Font = new Font(label3.Font, FontStyle.Bold);
            Controls.Add(label3);

            // textBox1
            textBox1 = new TextBox();
            textBox1.Location = new Point(50, 30);
            textBox1.Size = new Size(300, 30);
            textBox1.Name = "textBox1";
            textBox1.Font = new Font("Microsoft Sans Serif", 12);
            Controls.Add(textBox1);


            // label1
            label1.Location = new Point(50, 95);
            label1.Size = new Size(200, 20);
            label1.Text = "Seçilmiş fayllar:";
            label1.Font = new Font(label1.Font, FontStyle.Bold);

            // listBox1
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(50, 120);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(500, 500);
            listBox1.TabIndex = 2;

            // label2
            label2.Location = new Point(600, 95);
            label2.Size = new Size(200, 20);
            label2.Text = "İcra edilmiş fayllar:";
            label2.Font = new Font(label2.Font, FontStyle.Bold);

            // listBox2 
            listBox2.FormattingEnabled = true;
            listBox2.Location = new Point(600, 120);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(500, 500);
            listBox2.TabIndex = 3;

            // openFileDialog1
            openFileDialog1.Filter = "XML files (*.xml)|*.xml";
            openFileDialog1.Multiselect = true;

            // Form1
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 650);
            Controls.Add(button1);
            Controls.Add(button2);
            Controls.Add(label1);
            Controls.Add(listBox1);
            Controls.Add(label2);
            Controls.Add(listBox2);
            Name = "Form1";
            Text = "XML to Excel Parser";
            ResumeLayout(false);

            // labelLoading
            labelLoading = new Label();
            labelLoading.Text = "Göndərilir...";
            labelLoading.Font = new Font("Microsoft Sans Serif", 16, FontStyle.Bold);
            labelLoading.ForeColor = Color.DarkBlue;
            labelLoading.AutoSize = true;
            labelLoading.Visible = false;
            labelLoading.Location = new Point((ClientSize.Width - 200) / 2, (ClientSize.Height - 30) / 2);
            Controls.Add(labelLoading);
            labelLoading.BringToFront();

        }


        #endregion
    }
}

