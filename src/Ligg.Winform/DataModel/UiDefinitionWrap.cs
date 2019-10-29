using System;
using Ligg.Winform.DataModel.Enums;

namespace Ligg.Winform.DataModel
{
    public class FunctionInitParamSet
    {
        public bool IsFormInvisible { get; set; }
        public FunctionFormType FormType { get; set; }
        public string AssemblyCode { get; set; }
        public string ApplicationCode { get; set; }
        public string ApplicationVersion { get; set; }
        public string FunctionCode { get; set; }
        public string ZoneLocationForNonMutiViewForm { get; set; }
        public string StartParams { get; set; }
        public string InputZoneVariablesForNonMutiViewForm { get; set; }
        public int StartViewMenuId { get; set; }
        public string StartActions { get; set; }
        public string StartPassword { get; set; }
        public string HelpdeskEmail { get; set; }
        public bool SupportMutiCultures { get; set; }
        public string FormTitle { get; set; }

        public string ImplementationDllPath { get; set; }
        public string AdapterFullClassName { get; set; }
    }

    public class FunctionFormStyle
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int TopLocationX { get; set; }
        public int TopLocationY { get; set; }
        public bool MaximizeBox { get; set; }
        public bool MinimizeBox { get; set; }
        public bool HasNoControlBox { get; set; }
        public bool TreatCloseBoxAsMinimizeBox { get; set; }
        public int ViewMenuMode { get; set; }
        public string StartWindowState { get; set; }//x
        public string OrdinaryWindowStatus { get; set; }
        public bool DrawIcon { get; set; }
        public int WindowRadius { get; set; }
        public string IconUrl { get; set; }
        public bool HasTray { get; set; }
        public string TrayIconUrl { get; set; }
        public string TrayDataSource { get; set; }
        public string ResizeRegionParams { get; set; }

    }

    public class LayoutElement
    {
        public int Id;
        public string Name;
        public int ParentViewMenuId;
        public string Container;
        public int Type;
        public string TypeName;
        public string ControlTypeName;
        public string DisplayName;
        public string Remark;
        public string Location;
        public string InputVariables;
        public string DataSource;


        public string View;
        public string DefaultViewMenuIdFlag;
        public string Action;
        public bool IsRendered;
        public bool IsChecked;
        public string InvalidFlag;
        public string DisabledFlag;
        public string InvisibleFlag;

        public string StyleClass;
        public string StyleText;
        public bool IsPopup;
        public int ZoneArrangementType;
        public int DockType;
        public string DockTypeName;
        public string DockOrder;
        public int OffsetOrPositionX;
        public int OffsetOrPositionY;
        public int Width;
        public int Height;
        public string ImageUrl;
        public int ImageWidth;
        public int ImageHeight;
        public string ResizeRegionParams;
    }

    public class RenderedViewStatus
    {
        public string Name { get; set; }
        public bool IsChecked;
        //public string LastTimeLanguageCode;
        // public RenderedViewStatus(string name, bool isChecked)//string lastTimeLanguageCode)
        public RenderedViewStatus(string name, bool isChecked)
        {
            Name = name;
            IsChecked = isChecked;
            // LastTimeLanguageCode = lastTimeLanguageCode;
        }
    }

    public class ZoneFeature
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int ArrangementType { get; set; }
        public string StyleText { get; set; }
        public bool OnlyForSingleView { get; set; }

        public bool HasNoControlBox { get; set; }
        public bool TreatCloseBoxAsMinimizeBox { get; set; }
        public bool DrawIcon { get; set; }
        public string IconUrl { get; set; }
        public bool HasTray { get; set; }
        public string TrayIconUrl { get; set; }
        public string TrayDataSource { get; set; }

        public bool HasRunningStatusSection { get; set; }
        public bool VerifyPasswordOnLoad { get; set; }
        public string PasswordVerification { get; set; }

    }


    public class ZoneItem
    {
        public int Id;
        public int Type;
        public string TypeName;
        public string Name;
        public int RowId;
        public string ControlTypeName;
        public string StyleClass;
        public string StyleText;
        //public bool IsPopup;
        public string DisplayName;
        //public string Description;
        //public string Remark;
        public string DataSource;
        public string DefaultValue;
        public string ValidationRules;
        public string Action;
        public string Action1;
        //public string ActionParams;
        public string InvalidFlag;
        public string DisabledFlag;
        public string InvisibleFlag;
        public string ContainerName;//only for radio
        public int DockType;
        public string DockTypeName;

        public string DockOrder;
        public int OffsetOrPositionX;
        public int OffsetOrPositionY;
        public int Width;
        public int Height;
    }



    public class VariableItem
    {
        public long Id;
        public int Type;
        public string TypeName;
        public string Name;
        public int GroupId;
        public string ZoneName;
        public string Condition;
        public string Formula;
        public string Value;
        public string NameToBeSet;
    }

    public class ContextMenuItem
    {
        public int Id;
        public int Type;//such as select one/many/none
        public string TypeName;//such as select one/many/none
        public string ControlTypeName;//Seperator
        public int ParentId;
        public string Name;
        public string DisplayName;
        public string Description;
        public string VisibleFlag;
        public string EnabledFlag;
        public string Action;
        public string ImageUrl;
    }

    public class ControlAction
    {
        public string Action;
        public string DisplayName;
    }



}


