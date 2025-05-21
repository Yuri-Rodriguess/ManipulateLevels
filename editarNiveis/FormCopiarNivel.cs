using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Eletric.editarNiveis
{
    public class FormCopiarNivel : System.Windows.Forms.Form
    {
        private List<Level> levels;
        private System.Windows.Forms.ComboBox comboBoxLevels;
        private System.Windows.Forms.TextBox numberOfCopiesTextBox;
        private System.Windows.Forms.TextBox distanceBetweenCopiesTextBox;
        private Button okButton;

        public int NumberOfCopies { get; private set; }
        public double DistanceBetweenCopies { get; private set; }
        public Level SelectedLevel { get; private set; }

        public FormCopiarNivel(List<Level> levels)
        {
            this.levels = levels;
            InitializeComponent();
            FillComboBox();
        }

        private void FillComboBox()
        {
            comboBoxLevels.DataSource = levels;
            comboBoxLevels.DisplayMember = "Name";
        }

        private void InitializeComponent()
        {
            {
                Label numberOfCopiesLabel = new Label
                {
                    Text = "Número de Cópias:",
                    AutoSize = true,
                    Location = new System.Drawing.Point(60, 41)
                };
                this.Controls.Add(numberOfCopiesLabel);

                numberOfCopiesTextBox = new System.Windows.Forms.TextBox
                {
                    Location = new System.Drawing.Point(250, 38),
                    Size = new System.Drawing.Size(100, 20)
                };
                this.Controls.Add(numberOfCopiesTextBox);

                Label distanceBetweenCopiesLabel = new Label
                {
                    Text = "Distância entre Cópias em Metros:",
                    AutoSize = true,
                    Location = new System.Drawing.Point(60, 67)
                };
                this.Controls.Add(distanceBetweenCopiesLabel);

                comboBoxLevels = new System.Windows.Forms.ComboBox
                {
                    Location = new System.Drawing.Point(250, 10),
                    Size = new System.Drawing.Size(100, 20)
                };
                this.Controls.Add(comboBoxLevels);

                Label Combo = new Label
                {
                    Text = "Todos os niveis:",
                    AutoSize = true,
                    Location = new System.Drawing.Point(60, 17)
                };
                this.Controls.Add(Combo);

                distanceBetweenCopiesTextBox = new System.Windows.Forms.TextBox
                {
                    Location = new System.Drawing.Point(250, 64),
                    Size = new System.Drawing.Size(100, 20)
                };
                this.Controls.Add(distanceBetweenCopiesTextBox);

                okButton = new Button
                {
                    Location = new System.Drawing.Point(12, 90),
                    Size = new System.Drawing.Size(75, 23),
                    Text = "OK",
                    DialogResult = DialogResult.OK
                };
                okButton.Click += OkButton_Click;
                this.Controls.Add(okButton);

                this.Text = "Informe a altura do primeiro nível, o número de cópias e a distância entre elas";
                this.ClientSize = new System.Drawing.Size(400, 130);
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.StartPosition = FormStartPosition.CenterScreen;
            }

            void OkButton_Click(object sender, EventArgs e)
            {
                if (int.TryParse(numberOfCopiesTextBox.Text, out int numberOfCopies) &&
                    double.TryParse(distanceBetweenCopiesTextBox.Text, out double distanceBetweenCopies))
                {
                    NumberOfCopies = numberOfCopies;
                    DistanceBetweenCopies = distanceBetweenCopies;
                    SelectedLevel = comboBoxLevels.SelectedItem as Level;

                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    TaskDialog.Show("Erro", "Certifique-se de inserir valores válidos para a altura do primeiro nível, número de cópias e distância entre elas.");
                }
            }
        }
    }
}