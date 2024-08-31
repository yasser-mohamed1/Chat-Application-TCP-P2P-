using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp_PeertoPeer_.Components
{
    public class InputDialog : Form
    {
        private TextBox textBox;
        private Button okButton;
        private Button cancelButton;
        private Label promptLabel;

        public string InputText { get; private set; }

        public InputDialog(string prompt)
        {
            // Set up form appearance
            Text = "Input Required";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(350, 150);
            Font = new Font("Segoe UI", 10);
            ControlBox = false;
            // Label
            promptLabel = new Label();
            promptLabel.Text = prompt;
            promptLabel.Location = new Point(15, 20);
            promptLabel.AutoSize = true;

            // TextBox
            textBox = new TextBox();
            textBox.Location = new Point(15, 50);
            textBox.Width = 300;
            textBox.Text = "";
            textBox.PlaceholderText = "Enter your name here...";

            // OK Button
            okButton = new Button();
            okButton.Text = "OK";
            okButton.DialogResult = DialogResult.OK;
            okButton.Location = new Point(145, 100);
            okButton.Click += OkButton_Click;

            // Cancel Button
            cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Location = new Point(245, 100);
            cancelButton.Click += CancelButton_Click;

            // Add controls to the form
            Controls.Add(promptLabel);
            Controls.Add(textBox);
            Controls.Add(okButton);
            Controls.Add(cancelButton);

            // Set default button
            AcceptButton = okButton;
            CancelButton = cancelButton;
        }


        private void CancelButton_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            InputText = textBox.Text.Trim();
            if (string.IsNullOrEmpty(InputText))
            {
                MessageBox.Show("Name cannot be empty. Please enter a name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
            }
        }
    }

}
