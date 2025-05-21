using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace Eletric.editarNiveis
{
    [Transaction(TransactionMode.Manual)]
    public class CLevel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Transaction transaction = null;
            try
            {
                // Obter o documento ativo do Revit
                Document doc = commandData.Application.ActiveUIDocument.Document;


                // Obter todos os níveis no documento
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(Level));
                List<Level> levels = collector.Cast<Level>().ToList();


                // Solicitar ao usuário a altura do primeiro nível, o número de cópias e a distância entre elas
                FormCopiarNivel formData = new FormCopiarNivel(levels);
                if (formData.ShowDialog() == DialogResult.OK)
                {
                    // Obter o nível selecionado no ComboBox
                    Level selectedLevel = formData.SelectedLevel;

                    int numberOfCopies = formData.NumberOfCopies;
                    double distanceBetweenCopies = formData.DistanceBetweenCopies;

                    // Converta a altura e a distância para milímetros usando a fórmula desejada
                    double distanceBetweenCopiesMilimetros = (distanceBetweenCopies * 39.37007) / 12;

                    // Iniciar a transação
                    transaction = new Transaction(doc, "Duplicar Nível");
                    transaction.Start();

                    // Obter o parâmetro de altura do nível
                    Parameter heightParameter = selectedLevel.get_Parameter(BuiltInParameter.LEVEL_ELEV);

                    // Obter o valor do parâmetro
                    double originalHeight = heightParameter.AsDouble();

                    // Obter o tipo de nível
                    ElementId levelTypeId = selectedLevel.GetTypeId();
                    LevelType levelType = doc.GetElement(levelTypeId) as LevelType;

                    // Criar cópias adicionais do nível com a distância especificada
                    for (int i = 0; i < numberOfCopies; i++)
                    {
                        double newHeight = originalHeight + (i + 1) * distanceBetweenCopiesMilimetros;
                        Level newLevel = Level.Create(doc, newHeight);
                        newLevel.get_Parameter(BuiltInParameter.ELEM_TYPE_PARAM).Set(levelTypeId);
                    }

                    // Commit na transação
                    transaction.Commit();

                    // Exibir uma mensagem de sucesso
                    TaskDialog.Show("Sucesso", $"{numberOfCopies} cópias do nível foram criadas com sucesso!");
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                // Em caso de erro, exibir uma mensagem de erro
                if (transaction != null && transaction.HasStarted())
                {
                    transaction.RollBack();
                }

                TaskDialog.Show("Erro", $"Ocorreu um erro: {ex.Message}");
                return Result.Failed;
            }
        }
    }
}