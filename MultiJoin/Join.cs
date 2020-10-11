#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion

namespace MultiJoin
{
    [Transaction(TransactionMode.Manual)]
    public class Join : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            if(Command.Main(1, commandData))
            {
                return Result.Succeeded;
            }else
            {
                return Result.Failed;
            }
        }
    }
}
