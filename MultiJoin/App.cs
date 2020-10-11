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
                .Tab("�� ��������� ����������������")
                .Panel("��������")
                .CreateStackedItems(si =>
                    si
                        .CreateButton<Join>("Join_button", "������������", btn => btn.SetSmallImage(MultiJoin.Properties.Resources.Join))
                        .CreateButton<Unjoin>("Unjoin_button", "��c��������", btn => btn.SetSmallImage(MultiJoin.Properties.Resources.Unjoin))
                        .CreateButton<Switchjoin>("Swit_button", "�����������", btn => btn.SetSmallImage(MultiJoin.Properties.Resources.Switchjoin)));
        }
    }
}
