using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace Eletric.editarNiveis
{
    public static class EditarNivelCommand
    {
        public static void EditarNomeNivel(ExternalCommandData commandData, string nomeNivel, string novoNome)
        {
            // Obtém o documento ativo
            Document doc = commandData.Application.ActiveUIDocument.Document;

            // Encontra o nível com o nome fornecido
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> levels = collector.OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType().ToElements();
            Level level = null;
            foreach (Element elem in levels)
            {
                if (elem.Name == nomeNivel)
                {
                    level = elem as Level;
                    break;
                }
            }

            // Verifica se o nível foi encontrado
            if (level != null)
            {
                // Inicia uma transação para modificar o nível
                using (Transaction trans = new Transaction(doc, "Editar Nome do Nível"))
                {
                    if (trans.Start() == TransactionStatus.Started)
                    {
                        // Altera o nome do nível
                        level.Name = novoNome;
                        trans.Commit();
                    }
                }
            }
        }

        public static void EditarElevacaoNivel(ExternalCommandData commandData, Level nivel, double novaElevacao)
        {
            // Obtém o documento ativo
            Document doc = commandData.Application.ActiveUIDocument.Document;

            // Verifica se o nível foi encontrado
            if (nivel != null)
            {
                // Inicia uma transação para modificar a elevação do nível
                using (Transaction trans = new Transaction(doc, "Editar Elevação do Nível"))
                {
                    if (trans.Start() == TransactionStatus.Started)
                    {
                        // Altera a elevação do nível
                        nivel.Elevation = novaElevacao;
                        trans.Commit();
                    }
                }
            }
        }
    }
}
