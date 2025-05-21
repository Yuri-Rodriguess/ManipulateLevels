using System;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Eletric.editarNiveis
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ComandoCriarNiveis : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            if (!VerificarDocumentoEditavel(uiDoc))
            {
                TaskDialog.Show("Aviso", "O documento não é editável. Certifique-se de que você tem permissões para editar o projeto.");
                return Result.Failed;
            }

            // Criar e exibir um formulário de entrada de dados
            FormDataInput formDataInput = new FormDataInput();
            if (formDataInput.ShowDialog() == DialogResult.OK)
            {
                int quantidadeNiveis = formDataInput.QuantidadeNiveis;
                double alturaNiveis = formDataInput.AlturaNiveis;
                double alturaPrimeiroNivel = formDataInput.AlturaPrimeiroNivel;
                string nomeNivel = formDataInput.NomeNivel;
                string preFix = formDataInput.Prefix;
                string susFix = formDataInput.Suffix;

                LevelCreator.CriarNiveis(uiDoc, quantidadeNiveis, alturaNiveis, nomeNivel, alturaPrimeiroNivel, preFix, susFix);

                return Result.Succeeded;
            }

            return Result.Cancelled;
        }

        private bool VerificarDocumentoEditavel(UIDocument uiDoc)
        {
            Document doc = uiDoc.Document;
            return !doc.IsReadOnly;
        }
    }

    public class FormDataInput : System.Windows.Forms.Form
    {
        private Label labelQuantidade;
        private Label labelAlturaNiveis;
        private Label labelAlturaPrimeiroNivel;
        private Label labelNomeNivel;
        private Label labelPrefix;
        private Label labelSuffix;
        private System.Windows.Forms.TextBox textBoxQuantidade;
        private System.Windows.Forms.TextBox textBoxAlturaNiveis;
        private System.Windows.Forms.TextBox textBoxAlturaPrimeiroNivel;
        private System.Windows.Forms.TextBox textBoxNomeNivel;
        private System.Windows.Forms.TextBox textBoxPrefix;
        private System.Windows.Forms.TextBox textBoxSuffix;
        private Button buttonOK;

        public int QuantidadeNiveis => int.Parse(textBoxQuantidade.Text);
        public double AlturaNiveis => double.Parse(textBoxAlturaNiveis.Text);
        public double AlturaPrimeiroNivel => double.Parse(textBoxAlturaPrimeiroNivel.Text);
        public string NomeNivel => textBoxNomeNivel.Text;
        public string Prefix => textBoxPrefix.Text;
        public string Suffix => textBoxSuffix.Text;

        public FormDataInput()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.labelQuantidade = new System.Windows.Forms.Label();
            this.labelAlturaNiveis = new System.Windows.Forms.Label();
            this.labelAlturaPrimeiroNivel = new System.Windows.Forms.Label();
            this.labelNomeNivel = new System.Windows.Forms.Label();
            this.labelPrefix = new System.Windows.Forms.Label();
            this.labelSuffix = new System.Windows.Forms.Label();
            this.textBoxQuantidade = new System.Windows.Forms.TextBox();
            this.textBoxAlturaNiveis = new System.Windows.Forms.TextBox();
            this.textBoxAlturaPrimeiroNivel = new System.Windows.Forms.TextBox();
            this.textBoxNomeNivel = new System.Windows.Forms.TextBox();
            this.textBoxPrefix = new System.Windows.Forms.TextBox();
            this.textBoxSuffix = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // Configuração dos rótulos (labels)
            this.labelQuantidade.Location = new System.Drawing.Point(10, 10);
            this.labelQuantidade.Size = new System.Drawing.Size(180, 20);
            this.labelQuantidade.Text = "Quantidade de Níveis:";
            this.Controls.Add(this.labelQuantidade);

            this.textBoxQuantidade.Location = new System.Drawing.Point(10, 30);
            this.textBoxQuantidade.Size = new System.Drawing.Size(180, 20);
            this.Controls.Add(this.textBoxQuantidade);

            this.labelAlturaNiveis.Location = new System.Drawing.Point(10, 60);
            this.labelAlturaNiveis.Size = new System.Drawing.Size(180, 20);
            this.labelAlturaNiveis.Text = "Altura dos Níveis (metros):";
            this.Controls.Add(this.labelAlturaNiveis);

            this.textBoxAlturaNiveis.Location = new System.Drawing.Point(10, 80);
            this.textBoxAlturaNiveis.Size = new System.Drawing.Size(180, 20);
            this.Controls.Add(this.textBoxAlturaNiveis);

            this.labelAlturaPrimeiroNivel.Location = new System.Drawing.Point(10, 110);
            this.labelAlturaPrimeiroNivel.Size = new System.Drawing.Size(180, 20);
            this.labelAlturaPrimeiroNivel.Text = "Altura do Primeiro Nível (metros):";
            this.Controls.Add(this.labelAlturaPrimeiroNivel);

            this.textBoxAlturaPrimeiroNivel.Location = new System.Drawing.Point(10, 130);
            this.textBoxAlturaPrimeiroNivel.Size = new System.Drawing.Size(180, 20);
            this.Controls.Add(this.textBoxAlturaPrimeiroNivel);

            this.labelNomeNivel.Location = new System.Drawing.Point(10, 160);
            this.labelNomeNivel.Size = new System.Drawing.Size(180, 20);
            this.labelNomeNivel.Text = "Nome do Nível:";
            this.Controls.Add(this.labelNomeNivel);

            this.textBoxNomeNivel.Location = new System.Drawing.Point(10, 180);
            this.textBoxNomeNivel.Size = new System.Drawing.Size(180, 20);
            this.Controls.Add(this.textBoxNomeNivel);

            this.labelPrefix.Location = new System.Drawing.Point(10, 210);
            this.labelPrefix.Size = new System.Drawing.Size(180, 20);
            this.labelPrefix.Text = "Prefixo:";
            this.Controls.Add(this.labelPrefix);

            this.textBoxPrefix.Location = new System.Drawing.Point(10, 230);
            this.textBoxPrefix.Size = new System.Drawing.Size(180, 20);
            this.Controls.Add(this.textBoxPrefix);

            this.labelSuffix.Location = new System.Drawing.Point(10, 260);
            this.labelSuffix.Size = new System.Drawing.Size(180, 20);
            this.labelSuffix.Text = "Sufixo:";
            this.Controls.Add(this.labelSuffix);

            this.textBoxSuffix.Location = new System.Drawing.Point(10, 280);
            this.textBoxSuffix.Size = new System.Drawing.Size(180, 20);
            this.Controls.Add(this.textBoxSuffix);

            // Configuração do botão OK
            this.buttonOK.Location = new System.Drawing.Point(10, 310);
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.Controls.Add(this.buttonOK);

            // Configuração de outros controles e propriedades...

            // Configurações gerais do formulário
            this.AcceptButton = buttonOK;
            this.CancelButton = buttonOK;
            this.ClientSize = new System.Drawing.Size(200, 350);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Criar Niveis";

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }

    public class LevelCreator
    {
        public static void CriarNiveis(UIDocument uiDoc, int quantidade, double alturaMetros, string nomeNivel, double alturaPrimeiroNivel, string preFix, string susFix)
        {
            Document doc = uiDoc.Document;

            if (doc.IsReadOnly)
            {
                TaskDialog.Show("Aviso", "O documento não é editável.");
                return;
            }

            double alturaMilimetros = alturaMetros / 0.3048;
            double alturaMILPRI = alturaPrimeiroNivel / 0.3048;

            using (Transaction trans = new Transaction(doc, "Criar Níveis"))
            {
                try
                {
                    trans.Start();

                    for (int i = 0; i < quantidade; i++)
                    {
                        double elevacaoAtual = alturaMILPRI + alturaMilimetros * i;

                        Level novoNivel = Level.Create(doc, elevacaoAtual);
                        // ?? verifica se existe trim apaga espacos em branco
                        string nomeNivelAtual = $"{preFix ?? ""} {nomeNivel ?? ""} {i + 1} {susFix ?? ""} ".Trim();
                        novoNivel.Name = nomeNivelAtual;
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Erro", $"Ocorreu um erro ao criar níveis: {ex.Message}");
                    trans.RollBack();
                }
            }
        }
    }
}
