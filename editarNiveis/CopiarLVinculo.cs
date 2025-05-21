using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Eletric.editarNiveis
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class CopiarLVinculo : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Solicita a seleção do vinculado
            string selectedLinkName = SelecionarVinculado(uidoc);
            if (!string.IsNullOrEmpty(selectedLinkName))
            {
                // Inicia uma transação
                using (Transaction transaction = new Transaction(doc, "Copiar Níveis"))
                {
                    transaction.Start();

                    // Encontra o vinculado selecionado
                    RevitLinkInstance selectedLink = new FilteredElementCollector(doc)
                        .OfCategory(BuiltInCategory.OST_RvtLinks)
                        .OfClass(typeof(RevitLinkInstance))
                        .Cast<RevitLinkInstance>()
                        .FirstOrDefault(link => link.Name == selectedLinkName);

                    // Obtém a transformação do vínculo
                    Transform transform = selectedLink.GetTotalTransform();

                    // Copia os níveis
                    foreach (ElementId levelId in new FilteredElementCollector(selectedLink.GetLinkDocument())
                        .OfClass(typeof(Level))
                        .ToElementIds())
                    {
                        Level linkedLevel = selectedLink.GetLinkDocument().GetElement(levelId) as Level;

                        // Obtém a elevação original do nível no vínculo
                        double elevation = linkedLevel.Elevation;

                        // Obtém a posição original do nível no vínculo
                        XYZ originalPosition = new XYZ(0, 0, elevation);

                        // Aplica a transformação para obter a nova posição transformada
                        XYZ transformedPosition = transform.OfPoint(originalPosition);

                        // Cria níveis correspondentes no modelo atual na nova posição do vinculado
                        Level newLevel = Level.Create(doc, transformedPosition.Z);
                        newLevel.Name = linkedLevel.Name;
                    }


                    // Completa a transação
                    transaction.Commit();

                    TaskDialog.Show("Sucesso", "Níveis copiados com sucesso!");
                    return Result.Succeeded;
                }
            }
            return Result.Cancelled;
        }

        private string SelecionarVinculado(UIDocument uidoc)
        {
            // Cria o formulário
            using (System.Windows.Forms.Form form = new System.Windows.Forms.Form())
            {
                form.Text = "Selecionar Vinculado";
                form.Size = new System.Drawing.Size(300, 150);

                System.Windows.Forms.Label label = new System.Windows.Forms.Label
                {
                    Text = "Selecione o Vinculado:",
                    Location = new System.Drawing.Point(10, 20)
                };

                System.Windows.Forms.ComboBox comboBox = new System.Windows.Forms.ComboBox
                {
                    Location = new System.Drawing.Point(10, 50)
                };

                // Lista todos os vinculados presentes no modelo Revit
                List<RevitLinkInstance> linkedInstances = new FilteredElementCollector(uidoc.Document)
                    .OfCategory(BuiltInCategory.OST_RvtLinks)
                    .OfClass(typeof(RevitLinkInstance))
                    .Cast<RevitLinkInstance>()
                    .ToList();

                // Preenche o ComboBox com os nomes dos vinculados
                foreach (RevitLinkInstance linkedInstance in linkedInstances)
                {
                    comboBox.Items.Add(linkedInstance.Name);
                }

                System.Windows.Forms.Button buttonOk = new System.Windows.Forms.Button
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Location = new System.Drawing.Point(50, 80)
                };

                form.Controls.Add(label);
                form.Controls.Add(comboBox);
                form.Controls.Add(buttonOk);

                DialogResult result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    return comboBox.SelectedItem?.ToString();
                }
            }

            return null;
        }
    }
}