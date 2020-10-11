#region Namespaces
using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;
using VCRevitRibbonUtil;
#endregion

namespace MultiJoin
{
    class App : IExternalApplication
    {
        static String addinAssmeblyPath = typeof(App).Assembly.Location;

        public Result OnStartup(UIControlledApplication a)
        {
            createPanel(a);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

        void createPanel(UIControlledApplication application)
        {
            Ribbon ribbon = new Ribbon(application);
            ribbon
                .Tab("АО Казанский Гипронииавиапром")
                .Panel("Изменить")
                .CreateStackedItems(si =>
                    si
                        .CreateButton<Join>("Join_button", "Присоединить", btn => btn.SetSmallImage(MultiJoin.Properties.Resources.Join))
                        .CreateButton<Unjoin>("Unjoin_button", "Отcоединить", btn => btn.SetSmallImage(MultiJoin.Properties.Resources.Unjoin))
                        .CreateButton<Switchjoin>("Swit_button", "Переключить", btn => btn.SetSmallImage(MultiJoin.Properties.Resources.Switchjoin)));
        }
    }
}
