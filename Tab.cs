using Autodesk.Revit.UI;
using Eletric.editarNiveis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Eletric
{
    // Esta class serve apenas para criar o tab,painel e butao no revit
    public class Tab : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            throw new NotImplementedException();
        }
        
        public Result OnStartup(UIControlledApplication application)
        {
            // Nome do tab e painel
            string tabName = "Manipular niveis";
            string panelName = "Painel";

            // Cria o tab na interface do usuário do Revit
            application.CreateRibbonTab(tabName);

            // Cria um painel para o tab
            RibbonPanel panel = application.CreateRibbonPanel(tabName, panelName);

            // Define o caminho para o assembly que contém a classe FormsSpace
            string assemblyLocation1 = typeof(MainForm).Assembly.Location;
            BitmapImage bitmap1 = new BitmapImage(new Uri("pack://application:,,,/Eletric;component/Resources/NíveisBlack.png"));
            PushButtonData button1 = new PushButtonData("Niveis", "Niveis", assemblyLocation1, "Eletric.editarNiveis.MainForm");
            button1.ToolTip = "Niveis";
            button1.LongDescription = "Niveis";
            button1.LargeImage = bitmap1;
            PushButton Button1 = (PushButton)panel.AddItem(button1);

            return Result.Succeeded;
        }
    }
}
