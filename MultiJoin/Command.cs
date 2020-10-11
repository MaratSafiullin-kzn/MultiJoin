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
    class Command 
    {
        public static bool Main(
            int buttonId,
            ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            List<Element> preSelectionElem = new List<Element>();
            List<Element> postSelectionElem = new List<Element>();

            foreach (ElementId elId in uidoc.Selection.GetElementIds())
            {
                preSelectionElem.Add(doc.GetElement(elId));
            }

            if (preSelectionElem.Count < 1)
            {
                TaskDialog.Show("Множественное соединение", "Необходимо выделить объекты перед запуском модуля.");
                return false;
            }
            else
            {
                try
                {
                    IList<Reference> picked = uidoc.Selection.PickObjects(ObjectType.Element);
                    foreach (Reference refer in picked)
                    {
                        postSelectionElem.Add(doc.GetElement(refer.ElementId));
                    }
                }
                catch
                {
                    return false;
                }

                if (postSelectionElem.Count < 1)
                {
                    TaskDialog.Show("Множественное соединение", "Необходимо выделить объекты для соединения.");
                    return false;
                }
                else
                {
                    using (Autodesk.Revit.DB.Transaction t = new Autodesk.Revit.DB.Transaction(doc))
                    {
                        t.Start("Join");

                        foreach (Element preEl in preSelectionElem)
                        {
                            FilteredElementCollector allElementsInView = new FilteredElementCollector(doc, doc.ActiveView.Id);

                            BoundingBoxXYZ bb = preEl.get_BoundingBox(doc.ActiveView);
                            Outline outline = new Outline(bb.Min, bb.Max);
                            BoundingBoxIntersectsFilter bbfilter = new BoundingBoxIntersectsFilter(outline);

                            allElementsInView.WherePasses(bbfilter);

                            IEnumerable<Element> elementsFilteredAndWithoutPreEl = allElementsInView.Where(c => c.Id != preEl.Id);

                            foreach (Element el in elementsFilteredAndWithoutPreEl)
                            {
                                try
                                {
                                    if(buttonId == 1) //Join
                                    {
                                        if (!postSelectionElem.Any(x => x.Id == el.Id)) continue;
                                        if (JoinGeometryUtils.AreElementsJoined(doc, preEl, el)) continue;

                                        JoinGeometryUtils.JoinGeometry(doc, preEl, el);
                                    }

                                    if (buttonId == 2) //Unjoin
                                    {
                                        if (!postSelectionElem.Any(x => x.Id == el.Id)) continue;
                                        if (!JoinGeometryUtils.AreElementsJoined(doc, preEl, el)) continue;

                                        JoinGeometryUtils.UnjoinGeometry(doc, preEl, el);
                                    }

                                    if (buttonId == 3) //Switch join
                                    {
                                        if (!postSelectionElem.Any(x => x.Id == el.Id)) continue;
                                        if (!JoinGeometryUtils.AreElementsJoined(doc, preEl, el)) continue;

                                        JoinGeometryUtils.SwitchJoinOrder(doc, preEl, el);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    TaskDialog.Show("Ошибка", ex.Message);
                                }
                            }
                        }
                        t.Commit();
                    }
                }
            }
            return true;
        }
    }
}
