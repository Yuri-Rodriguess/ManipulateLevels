using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Windows.Forms;

namespace Eletric.editarNiveis
{
    [Transaction(TransactionMode.Manual)]
    public class EditarNivel : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            // Obtém a aplicação Revit e o documento ativo
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            // Cria uma janela de diálogo para a interface de edição
            EditarNivelForm editForm = new EditarNivelForm(commandData, doc);

            // Exibe a janela de diálogo
            editForm.ShowDialog();

            return Result.Succeeded;
        }
    }

    public class EditarNivelForm : System.Windows.Forms.Form
    {
        private readonly List<Level> niveis; // Corrigindo a referência para a lista de níveis

        private readonly System.Windows.Forms.CheckedListBox checkedListBoxNiveis;
        private readonly System.Windows.Forms.TextBox textBoxNovoNome;
        private readonly System.Windows.Forms.TextBox textBoxNovaElevacao;
        private readonly Label labelNome;
        private readonly Label labelElevacao;
        private readonly System.Windows.Forms.Button buttonEditar;
        private readonly System.Windows.Forms.Button buttonElevacao;
        private readonly ExternalCommandData commandData; // Novo campo para armazenar ExternalCommandData

        public EditarNivelForm(ExternalCommandData commandData, Document doc)
        {
            this.commandData = commandData; // Armazena ExternalCommandData

            // Configuração da janela de diálogo
            this.Text = "Editar Níveis";
            this.Size = new System.Drawing.Size(400, 400); // Ajuste o tamanho da janela principal

            // Inicialização dos controles
            checkedListBoxNiveis = new System.Windows.Forms.CheckedListBox();
            textBoxNovoNome = new System.Windows.Forms.TextBox();
            buttonEditar = new System.Windows.Forms.Button();
            buttonElevacao = new System.Windows.Forms.Button();
            labelNome = new Label();
            textBoxNovaElevacao = new System.Windows.Forms.TextBox();
            labelElevacao = new Label();

            // Configuração da CheckedListBox com os nomes e elevações dos níveis
            niveis = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Levels)
                .OfClass(typeof(Level))
                .Cast<Level>()
                .ToList();

            foreach (var nivel in niveis)
            {
                // Convertendo a elevação de pés para metros (1 pé = 0.3048 metros)
                double elevacaoEmMetros = nivel.Elevation * 0.3048;

                string nomeElevacao = $"{nivel.Name} - Elevação: {elevacaoEmMetros} metros";
                checkedListBoxNiveis.Items.Add(nomeElevacao);
            }


            labelNome.Text = "Novo nome";
            labelNome.Location = new System.Drawing.Point(150, 110);

            labelElevacao.Text = "Nova Elevação";
            labelElevacao.Location = new System.Drawing.Point(30, 110);

            // Configuração do TextBox e do Button
            textBoxNovoNome.Location = new System.Drawing.Point(150, 130);
            textBoxNovaElevacao.Location = new System.Drawing.Point(30, 130);

            buttonEditar.Location = new System.Drawing.Point(150, 160);
            buttonEditar.Text = "Editar Nome";
            buttonEditar.Click += new EventHandler(ButtonEditar_Click);

            buttonElevacao.Location = new System.Drawing.Point(30, 160);
            buttonElevacao.Text = "Editar Elevação";
            buttonElevacao.Click += new EventHandler(ButtonElevacao_Click);

            // Definindo o tamanho do CheckedListBox
            checkedListBoxNiveis.Size = new System.Drawing.Size(300, 100); // Ajuste o tamanho do CheckedListBox

            // Adiciona os controles à janela de diálogo
            this.Controls.Add(checkedListBoxNiveis);
            this.Controls.Add(textBoxNovoNome);
            this.Controls.Add(buttonEditar);
            this.Controls.Add(labelNome);
            this.Controls.Add(labelElevacao);
            this.Controls.Add(buttonElevacao);
            this.Controls.Add(textBoxNovaElevacao);
        }
        private void RefreshList()
        {
            // Limpa os itens do CheckedListBox
            checkedListBoxNiveis.Items.Clear();

            // Adiciona os nomes e elevações atualizados dos níveis à CheckedListBox
            foreach (var nivel in niveis)
            {
                // Convertendo a elevação de pés para metros (1 pé = 0.3048 metros)
                double elevacaoEmMetros = nivel.Elevation * 0.3048;

                string nomeElevacao = $"{nivel.Name} - Elevação: {elevacaoEmMetros} metros";
                checkedListBoxNiveis.Items.Add(nomeElevacao);
            }
        }
        private void ButtonEditar_Click(object sender, EventArgs e)
        {
            // Obtém o novo nome do TextBox
            string novoNome = textBoxNovoNome.Text;

            // Armazena os índices dos níveis selecionados
            List<int> indicesSelecionados = new List<int>();
            foreach (int index in checkedListBoxNiveis.CheckedIndices)
            {
                indicesSelecionados.Add(index);
            }

            // Realiza a edição do nome do nível no documento para cada nível selecionado
            foreach (int index in indicesSelecionados)
            {
                // Obtendo o nível correspondente ao índice
                if (index >= 0 && index < niveis.Count)
                {
                    Level nivel = niveis[index];

                    // Adiciona um número ao nome do nível para evitar nomes duplicados
                    string nomeEditado = $"{novoNome} {index + 1}";

                    EditarNivelCommand.EditarNomeNivel(commandData, nivel.Name, nomeEditado);
                }
            }

            RefreshList(); // Atualiza o ListBox após editar o nome
                           //this.Close();
        }

        private void ButtonElevacao_Click(object sender, EventArgs e)
        {
            // Obtém a nova elevação do TextBox
            string novaElevacaoText = textBoxNovaElevacao.Text;

            // Convertendo a nova elevação de pés para metros
            if (!double.TryParse(novaElevacaoText, out double novaElevacaoEmPes))
            {
                MessageBox.Show("A elevação deve ser um valor numérico.");
                return;
            }

            // Convertendo de pés para metros (1 pé = 0.3048 metros)
            double novaElevacaoEmMetros = novaElevacaoEmPes / 0.3048;

            // Armazena os índices dos níveis selecionados
            List<int> indicesSelecionados = new List<int>();
            foreach (int index in checkedListBoxNiveis.CheckedIndices)
            {
                indicesSelecionados.Add(index);
            }

            // Realiza a edição da elevação do nível no documento para cada nível selecionado
            foreach (int index in indicesSelecionados)
            {
                // Obtendo o nível correspondente ao índice
                if (index >= 0 && index < niveis.Count)
                {
                    Level nivel = niveis[index];

                    // Calcula a elevação do nível anterior, se existir
                    double elevacaoNivelAnterior = 0.0;
                    if (index > 0) // Verifica se não é o primeiro nível
                    {
                        Level nivelAnterior = niveis[index - 1];
                        elevacaoNivelAnterior = nivelAnterior.Elevation;
                    }

                    // Soma a nova elevação com a elevação do nível anterior
                    double novaElevacaoComElevacaoAnterior = novaElevacaoEmMetros + elevacaoNivelAnterior;

                    // Edita a elevação do nível
                    EditarNivelCommand.EditarElevacaoNivel(commandData, nivel, novaElevacaoComElevacaoAnterior);
                }
            }

            RefreshList(); // Atualiza o ListBox após editar a elevação
                           //this.Close();
        }
    }
}