using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ligg.Base.DataModel;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.WinForm.DataModel;
using Ligg.WinForm.DataModel.Enums;

namespace Ligg.WinForm.Helpers
{
    public static class LayoutHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        //#set
        public static void SetLayoutElementType(LayoutElement layoutElement)
        {
            try
            {
                layoutElement.Type = EnumHelper.GetIdByName<LayoutElementType>(layoutElement.TypeName);
                layoutElement.DockType = EnumHelper.GetIdByName<ControlDockType>(layoutElement.DockTypeName);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".SetLayoutElementType Error: " + ex.Message);
            }
        }

        public static void SetZoneItemType(ZoneItem zoneItem)
        {
            try
            {
                zoneItem.Type = EnumHelper.GetIdByName<LayoutElementType>(zoneItem.TypeName);
                zoneItem.DockType = EnumHelper.GetIdByName<ControlDockType>(zoneItem.DockTypeName);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".SetZoneItemType Error: " + ex.Message);
            }
        }

        public static void SetProcedureType(ProcedureItem variableItem)
        {
            try
            {
                variableItem.Type = EnumHelper.GetIdByName<ProcedureItemType>(variableItem.TypeName);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".SetProcedureType Error: " + ex.Message);
            }
        }

        //#check
        public static void CheckMenuItems(FunctionFormViewMenuMode menuMode, List<LayoutElement> elmts)
        {
            try
            {
                foreach (var elmt in elmts)
                {
                    LayoutHelper.CheckElementName(elmt.Name);
                    if (elmt.Id < 1)
                    {
                        throw new ArgumentException("LayoutElement Id can't be less than 1! elmt.Id=" + elmt.Id + ", elmt.Name=" + elmt.Name);
                    }

                    if (elmts.FindAll(x => x.Id == elmt.Id).Count > 1)
                    {
                        throw new ArgumentException("LayoutElement can't have duplicated Id! elmt.Id=" + elmt.Id + ", elmt.Name=" + elmt.Name);
                    }

                    if (elmts.FindAll(x => x.Name == elmt.Name).Count > 1)
                    {
                        throw new ArgumentException("LayoutElement can't have duplicated name! elmt.Id=" + elmt.Id + ", elmt.Name=" + elmt.Name);
                    }

                    if (elmt.DockType < 0 || elmt.DockType > 5)
                    {
                        throw new ArgumentException("LayoutElement's DockType can't be less than 0 or greater than 5! area.Id=" + elmt.Id + ", area.Name=" + elmt.Name);
                    }

                    if (elmt.Type == (int)LayoutElementType.ToolMenuArea)
                    {
                        var toolMenuItems = elmts.FindAll(x => x.Container == elmt.Name);
                        foreach (var toolMenuItem in toolMenuItems)
                        {
                            if (toolMenuItem.Type == (int)LayoutElementType.ViewMenuItem)
                            {
                                throw new ArgumentException("ToolMenuArea can't contain a ViewMenuItem! ToolMenuArea.Id=" + elmt.Id + ", ToolMenuArea.Name=" + elmt.Name
                                                            + ", viewMenuItem.Id=" + toolMenuItem.Id + ", viewMenuItem.Name=" + toolMenuItem.Name);
                            }
                        }
                    }
                    if (menuMode == FunctionFormViewMenuMode.Customized)
                    {

                        if (elmt.Type != (int)LayoutElementType.TransactionOnlyItem)
                        {
                            if (elmt.Container.IsNullOrEmpty())
                            {
                                throw new ArgumentException("Element except for TransactionOnlyItem must have container! elmt.Id=" + elmt.Id + ", elmt.Name=" + elmt.Name);
                            }
                        }

                        if (elmt.Type == (int)LayoutElementType.ViewMenuArea)
                        {
                            var sameAreaItems = elmts.FindAll(x => x.Container == elmt.Name);
                            foreach (var item in sameAreaItems)
                            {
                                if (sameAreaItems.FindAll(x => x.Name == item.Name).Count > 1)
                                {
                                    throw new ArgumentException("ViewMenuArea  can't have duplicated contained elmt name! area.Id=" + elmt.Id + ",area.Name=" + elmt.Name + ", item.Id=" + item.Id + ", item.Name=" + item.Name);
                                }
                            }
                        }

                        if (elmt.Type == (int)LayoutElementType.ViewMenuItem)
                        {
                            if (elmts.FindAll(x => x.Type == (int)LayoutElementType.ViewMenuArea && x.ParentViewMenuId == elmt.Id).Count == 0)
                            {
                                if (elmt.View.IsNullOrEmpty() & !elmt.ControlTypeName.Contains("Label"))
                                {
                                    throw new ArgumentException("Last level contained view menu must have a view! elmt.Id=" + elmt.Id + ", elmt.Name=" + elmt.Name + ", ControlTypeName=" + elmt.ControlTypeName);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".CheckLayoutElements Error: " + ex.Message);
            }
        }

        public static void CheckViewItems(string viewName, List<LayoutElement> items)
        {
            try
            {
                foreach (var item in items)
                {
                    LayoutHelper.CheckElementName(item.Name);
                    if (item.Type == (int)LayoutElementType.ContentArea | item.Type == (int)LayoutElementType.Zone)
                    {
                        if (item.Container.IsNullOrEmpty()) throw new ArgumentException("ContentArea or Zone ViewItem container can't be null!  viewName=" + viewName + ", item.Id=" + item.Id + ", item.Name=" + item.Name);
                    }

                    if (item.Type == (int)LayoutElementType.ContentArea)
                    {
                        if (item.DockType < 1 || item.DockType > 5)
                        {
                            throw new ArgumentException("Content area's DockTypeName should be ‘Top’, ‘Right’, ‘Bottom’, ‘Left' or 'Fill’! viewName=" + viewName + ", area.Id=" + item.Id + ", area.Name=" + item.Name);
                        }

                        var sameAreaItems = items.FindAll(x => x.Container == item.Name);
                        foreach (var subItem in sameAreaItems)
                        {
                            if (sameAreaItems.FindAll(x => x.Name == subItem.Name).Count > 1)
                            {
                                throw new ArgumentException("Content area can't have duplicated contained item name! viewName=" + viewName + ", area.Id=" + item.Id + ",area.Name=" + item.Name + ", subItem.Id=" + subItem.Id + ", subItem.Name=" + subItem.Name);
                            }
                        }
                    }


                    if (items.FindAll(x => x.Name == item.Name && ((item.Type == (int)LayoutElementType.FollowingTransactionItem | item.Type == (int)LayoutElementType.TransactionOnlyItem | item.Type == (int)LayoutElementType.ContentArea))).Count > 1)
                    {
                        throw new ArgumentException("FollowingTransactionItem/TransactionOnlyItem/ContentArea can't have duplicated name! viewName=" + viewName + ", item.Id=" + item.Id + ", item.Name=" + item.Name);
                    }

                    if (items.FindAll(x => x.Id == item.Id).Count > 1)
                    {
                        throw new ArgumentException("View Item can't have duplicated Id! viewName=" + viewName + ", item.Id=" + item.Id + ", item.Name=" + item.Name);
                    }


                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".CheckViewItems Error: " + ex.Message);
            }
        }

        public static void CheckProcedures(bool isFunctionLevel, string zoneOrFuncName, List<ProcedureItem> procItems)
        {
            try
            {
                foreach (var item in procItems)
                {
                    LayoutHelper.CheckElementName(item.Name);
                    var zoneOrFunctionName = isFunctionLevel ? "functionName" : "zoneName";
                    if (string.IsNullOrEmpty(item.Name))
                    {
                        throw new ArgumentException("procItem Name  can't be null! " + zoneOrFunctionName + "=" + zoneOrFuncName + ", procItem.Id=" + item.Id);
                    }
                    if (!string.IsNullOrEmpty(item.Name))
                    {
                        //var sameIdVars = procItems.FindAll(x => x.Id == item.Id);
                        //if (sameIdVars.Count > 1)
                        //{
                        //    throw new ArgumentException("procItem can't have duplicated Id! " + zoneOrFunctionName + "=" + zoneOrFuncName + ", procItem.Id=" + item.Id + ", procItem.Name=" + item.Name);
                        //}

                        var sameNameVars = procItems.FindAll(x => x.Name == item.Name);
                        if (sameNameVars.Count > 1)
                        {
                            throw new ArgumentException("procItem can't have duplicated name! " + zoneOrFunctionName + "=" + zoneOrFuncName + ", procItem.Id=" + item.Id + ", procItem.Name=" + item.Name);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".CheckProcedures Error: " + ex.Message);
            }
        }

        public static void CheckZoneItems(string zoneName, List<ZoneItem> zoneItems)
        {
            try
            {
                foreach (var item in zoneItems)
                {
                    LayoutHelper.CheckElementName(item.Name);
                    if (item.Name.IsNullOrEmpty())
                    {
                        throw new ArgumentException("ZoneItem name can't be empty! zoneName=" + zoneName + ", item.Id=" + item.Id + ", item.Name=" + item.Name);
                    }

                    if (zoneItems.FindAll(x => x.Name == item.Name).Count > 1)
                    {
                        throw new ArgumentException("ZoneItem can't have duplicated name! zoneName=" + zoneName + ", item.Id=" + item.Id + ", item.Name=" + item.Name);
                    }

                    //if (zoneItems.FindAll(x => x.Id == item.Id).Count > 1)
                    //{
                    //    throw new ArgumentException("ZoneItem can't have duplicated Id! zonaName=" + zoneName + ", item.Id=" + item.Id + ", item.Name=" + item.Name);
                    //}
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".CheckZoneItems Error: " + ex.Message);
            }
        }

        public static string GetControlDisplayName(bool supportMultiLangs, string className, string ctrlName, List<Annex> annexes, string defDisplayName)
        {
            try
            {
                if (supportMultiLangs)
                {
                    var text = AnnexHelper.GetText(className, ctrlName, annexes, AnnexTextType.DisplayName, CultureHelper.CurrentLanguageCode, GetAnnexMode.OnlyByCurLang);
                    if (text.IsNullOrEmpty()) text = defDisplayName;
                    return text;
                }
                else
                {

                    return defDisplayName;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".GetDisplayName Error: " + ex.Message);
            }
        }

        public static string GetControlRemark(bool supportMutiLangs, string className, string ctrlName, List<Annex> annexes, string defRemark)
        {
            try
            {
                if (defRemark.IsNullOrEmpty())
                {
                    //if (!ctrlName.Contains("_")) defRemark = ctrlName;
                    //else defRemark = ctrlName.GetLastSeparatedString('_');
                }

                if (supportMutiLangs)
                {
                    var text = AnnexHelper.GetText(className, ctrlName, annexes, AnnexTextType.Remark, CultureHelper.CurrentLanguageCode, GetAnnexMode.OnlyByCurLang);
                    if (text.IsNullOrEmpty()) text = defRemark;
                    return text;
                }
                else return defRemark;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".GetRemark Error: " + ex.Message);
            }
        }

        //#common
        //##CheckElementName
        private static void CheckElementName(string text)
        {
            try
            {
                if (!text.IsAlphaNumeralAndHyphenOrEmpty())
                {
                    throw new ArgumentException("Element Name [" + text + "] is not in valid format, Element Name can only includes alpha numeral and hyphen! ");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".CheckElementName Error: " + ex.Message);
            }
        }




    }

}