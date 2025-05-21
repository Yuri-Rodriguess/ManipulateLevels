using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Eletric.editarNiveis
{
    [Transaction(TransactionMode.Manual)]
    public partial class MainForm : System.Windows.Forms.Form, IExternalCommand
    {
        private ExternalCommandData commandData;
        private Document doc;
        private Button btnAbrirEditarNivel; // Botão para abrir o formulário EditarNivel
        private Button btnAbrirFormCopiarNivel;
        private Button btnCriarNiveis;

        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            // Define as variáveis commandData e doc para serem acessadas pelo resto da classe
            this.commandData = commandData;
            this.doc = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            // Inicializa o formulário principal
            InitializeForm();

            // Exibe o formulário principal
            Application.Run(this);

            return Result.Succeeded;
        }

        private void InitializeForm()
        {
            // Inicializa os componentes do formulário
            this.Text = "Niveis";
            this.Size = new System.Drawing.Size(250, 250);

            // Inicializa o botão para abrir o formulário EditarNivel
            btnAbrirEditarNivel = new Button();
            btnAbrirEditarNivel.Text = "Editar Níveis";
            btnAbrirEditarNivel.Size = new System.Drawing.Size(120, 40);
            btnAbrirEditarNivel.Location = new System.Drawing.Point(10, 10);
            btnAbrirEditarNivel.Click += new EventHandler(btnAbrirEditarNivel_Click);
            this.Controls.Add(btnAbrirEditarNivel);

            // Inicializa o botão para abrir o formulário FormCopiarNivel
            btnAbrirFormCopiarNivel = new Button();
            btnAbrirFormCopiarNivel.Text = "Copiar Nivel";
            btnAbrirFormCopiarNivel.Size = new System.Drawing.Size(120, 40);
            btnAbrirFormCopiarNivel.Location = new System.Drawing.Point(btnAbrirEditarNivel.Left, btnAbrirEditarNivel.Bottom + 10);
            btnAbrirFormCopiarNivel.Click += new EventHandler(btnAbirFormCopiarNivel_Click);
            this.Controls.Add(btnAbrirFormCopiarNivel);

            // Inicializa o botão para executar o comando de criar níveis
            Button btnCriarNiveis = new Button();
            btnCriarNiveis.Text = "Criar Níveis";
            btnCriarNiveis.Size = new System.Drawing.Size(120, 40);
            btnCriarNiveis.Location = new System.Drawing.Point(btnAbrirFormCopiarNivel.Left, btnAbrirFormCopiarNivel.Bottom + 10);
            btnCriarNiveis.Click += new EventHandler(btnCriarNiveis_Click);
            this.Controls.Add(btnCriarNiveis);

            // Inicializa o botão para executar o comando de copiar níveis de vínculo
            Button btnCopiarNiveisVinculo = new Button();
            btnCopiarNiveisVinculo.Text = "Copiar do Vínculo";
            btnCopiarNiveisVinculo.Size = new System.Drawing.Size(120, 40);
            btnCopiarNiveisVinculo.Location = new System.Drawing.Point(btnCriarNiveis.Left, btnCriarNiveis.Bottom + 10);
            btnCopiarNiveisVinculo.Click += new EventHandler(btnCopiarNiveisVinculo_Click);
            this.Controls.Add(btnCopiarNiveisVinculo);
        }

        private void btnCopiarNiveisVinculo_Click(object sender, EventArgs e)
        {
            // Cria uma nova instância do comando CopiarLVinculo
            CopiarLVinculo copiarLVinculoCommand = new CopiarLVinculo();

            // Declara uma variável message local
            string message = "";

            // Executa o comando
            copiarLVinculoCommand.Execute(commandData, ref message, null);
        }


        private void btnCriarNiveis_Click(object sender, EventArgs e)
        {
            // Cria uma nova instância do comando ComandoCriarNiveis
            ComandoCriarNiveis criarNiveisCommand = new ComandoCriarNiveis();

            // Declara uma variável message local
            string message = "";

            // Executa o comando
            criarNiveisCommand.Execute(commandData, ref message, null);
        }

        private void btnAbirFormCopiarNivel_Click(object sender, EventArgs e)
        {
            // Cria uma nova instância do comando ComandoCriarNiveis
            CLevel Levell = new CLevel();

            // Declara uma variável message local
            string message = "";

            // Executa o comando
            Levell.Execute(commandData, ref message, null);
        }
        private void btnAbrirEditarNivel_Click(object sender, EventArgs e)
        {
            // Cria uma nova instância do formulário EditarNivelForm
            EditarNivelForm editarNivelForm = new EditarNivelForm(commandData, doc);

            // Exibe o novo formulário
            editarNivelForm.ShowDialog();
        }
    }
}