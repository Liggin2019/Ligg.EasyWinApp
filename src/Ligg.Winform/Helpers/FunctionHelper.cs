using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Ligg.Base.DataModel;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.Base.Handlers;
using Ligg.Winform.Dialogs;

namespace Ligg.Winform.Helpers
{
    public static class FunctionHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        //#ResolveConstants
        public static string ResolveConstants(string text)
        {
            try
            {
                if (!text.Contains("%")) return text;
                var toBeRplStr = "";
                toBeRplStr = "%Now%".ToLower();
                if (text.ToLower().Contains(toBeRplStr))
                {
                    var rplStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    text = Regex.Replace(text, toBeRplStr, rplStr, RegexOptions.IgnoreCase);
                }
                toBeRplStr = "%UtcNow%".ToLower();
                if (text.ToLower().Contains(toBeRplStr))
                {
                    var rplStr = SystemTimeHelper.UtcNow().ToString("yyyy-MM-dd HH:mm:ss");
                    text = Regex.Replace(text, toBeRplStr, rplStr, RegexOptions.IgnoreCase);
                }

                return ResolveSpecialFolder(text);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".ResolveDefaultVariables Error: " + ex.Message);
            }
        }

        //#GetText
        public static string GetText(string funcName, string[] funcParamArray)
        {
            try
            {
                //common
                if (funcName.ToLower() == "empty" | funcName.ToLower() == "null")
                {
                    return string.Empty;
                }


                else if (funcName.ToLower() == "GetInputText".ToLower())
                {
                    var dlg = new TextInputDialog();
                    var verifyRule = funcParamArray.Length > 0 ? funcParamArray[0] : "";
                    var verifyParams = funcParamArray.Length > 1 ? funcParamArray[1] : "";
                    dlg.VerificationRule = verifyRule;
                    dlg.VerificationParams = verifyParams;
                    dlg.ShowDialog();
                    return dlg.InputText;
                }

                else if (funcName.ToLower() == "GetInputDateTime".ToLower())
                {
                    var dlg = new DateTimeInputDialog();
                    var customFormat = funcParamArray.Length > 0 ? funcParamArray[0] : "";
                    var verifyRule = funcParamArray.Length > 1 ? funcParamArray[1] : "";
                    var verifyParams = funcParamArray.Length > 2 ? funcParamArray[2] : "";

                    dlg.VerificationRule = verifyRule;
                    dlg.VerificationParams = verifyParams;
                    dlg.CustomFormat = customFormat;
                    dlg.ShowDialog();
                    return dlg.InputText;
                }

                else if (funcName.ToLower() == "LineQty".ToLower())
                {
                    int qty = funcParamArray[0].Split('\n').Length;
                    return Convert.ToString(qty);
                }
                else if (funcName.ToLower() == "LinesBySearch".ToLower())
                {
                    var strArry = funcParamArray[0].Split('\n');
                    var schStrArry = funcParamArray[1].Split(',');
                    var strList = new List<string>();
                    foreach (var v in strArry)
                    {
                        foreach (var s in schStrArry)
                        {
                            if (v.ToLower().Contains(s.ToLower()))
                            {
                                strList.Add(v);
                            }
                        }
                    }

                    var strList1 = strList.Distinct();
                    var strBlder = new StringBuilder();
                    foreach (var v in strList1)
                    {
                        if (!string.IsNullOrEmpty(v))
                        {
                            strBlder.AppendLine(v);
                        }
                    }

                    return strBlder.ToString();
                }

                else if (funcName.ToLower() == "DateTime".ToLower())
                {
                    var customFormat = "yyyy-MM-dd HH:mm:ss";
                    if (funcParamArray[0].ToLower() == "UtcNow".ToLower())
                    {
                        var time = SystemTimeHelper.UtcNow(); //
                        return time.ToString(customFormat, DateTimeFormatInfo.InvariantInfo);
                    }
                    else if (funcParamArray[0].ToLower() == "Now".ToLower())
                    {
                        var time = SystemTimeHelper.Now(); //
                        return time.ToString(customFormat, DateTimeFormatInfo.InvariantInfo);
                    }
                    else throw new ArgumentException("funcName: " + funcName + " has no param " + funcParamArray[0] + "! ");
                }
                else if (funcName.ToLower() == "UniqueString".ToLower())
                {
                    if (funcParamArray[0] == "ByNow")
                    {
                        var seperator = funcParamArray[1];
                        return funcParamArray[2].ToUniqueStringByNow(seperator);
                    }
                    else throw new ArgumentException("funcName: " + funcName + " has no param '" + funcParamArray[0] + "'! ");
                }

                else if (funcName.ToLower() == "Format".ToLower())
                {
                    if (funcParamArray[0].ToLower() == "upper")
                    {
                        return funcParamArray[1].ToUpper();
                    }
                    else if (funcParamArray[0].ToLower() == "lower")
                    {
                        return funcParamArray[1].ToLower();
                    }
                    else if (funcParamArray[0].ToLower() == "timespan")
                    {
                        return SystemTimeHelper.GetTimeSpanString(Convert.ToDouble(funcParamArray[1]), funcParamArray[2],
                            false);
                    }
                    else if (funcParamArray[0].ToLower() == "real")
                    {
                        return string.Format(funcParamArray[2], Convert.ToDouble(funcParamArray[1]));
                    }

                    else if (funcParamArray[0].ToLower() == "FormatString")
                    {
                        return string.Format(funcParamArray[1], funcParamArray[2]);
                    }
                    else throw new ArgumentException(funcName + " has no param '" + funcParamArray[0] + "'! ");
                }
                else if (funcName.ToLower() == "Replace".ToLower())
                {
                    return funcParamArray[1].Length == 0 ? funcParamArray[0]
                        : funcParamArray[0].Replace(funcParamArray[1], funcParamArray[2]);
                }

                else if (funcName.ToLower() == "Split".ToLower())
                {
                    var seperator = funcParamArray[1][0];
                    var tmpStrArray = funcParamArray[0].Split(seperator);
                    var index = Convert.ToInt16(funcParamArray[2]);
                    if (index > tmpStrArray.Length || index == tmpStrArray.Length)
                    {
                        return "";
                    }
                    else
                    {
                        return tmpStrArray[index];
                    }
                }
                else if (funcName.ToLower() == "Combine".ToLower())
                {
                    var separator = funcParamArray[0].GetSubParamSeparator();
                    var tmpStrArray = funcParamArray[0].Split(separator);
                    var rtStr = "";
                    var i = 0;
                    foreach (var tmpStr in tmpStrArray)
                    {
                        rtStr = rtStr + tmpStr;
                    }
                    return rtStr;
                }
                else if (funcName.ToLower() == "SubString".ToLower())
                {
                    var tmStr = funcParamArray[0];
                    Int16 sttIndex = Convert.ToInt16(funcParamArray[1]);
                    Int16 len = Convert.ToInt16(funcParamArray[2]);
                    return tmStr.Substring(sttIndex, len);
                }

                //file
                else if (funcName.ToLower() == "FileDetail".ToLower())
                {
                    if (funcParamArray[1].IsNullOrEmpty()) throw new ArgumentException("file path can't be empty! ");
                    if (funcParamArray[0].ToLower() == "Directory".ToLower())
                    {
                        return FileHelper.GetFileDetailByOption(funcParamArray[1], FilePathComposition.Directory);
                    }
                    else if (funcParamArray[0].ToLower() == "FileName".ToLower())
                    {
                        return FileHelper.GetFileDetailByOption(funcParamArray[1], FilePathComposition.FileName);
                    }
                    else if (funcParamArray[0].ToLower() == "FileTitle".ToLower())
                    {
                        return FileHelper.GetFileDetailByOption(funcParamArray[1], FilePathComposition.FileTitle);
                    }
                    else if (funcParamArray[0].ToLower() == "Suffix".ToLower())
                    {
                        return FileHelper.GetFileDetailByOption(funcParamArray[1], FilePathComposition.Suffix);
                    }
                    else if (funcParamArray[0].ToLower() == "Postfix".ToLower())
                    {
                        return FileHelper.GetFileDetailByOption(funcParamArray[1], FilePathComposition.Postfix);
                    }
                    else throw new ArgumentException("funcName: " + funcName + " has no param '" + funcParamArray[0] + "'! ");
                }
                else if (funcName.ToLower() == "ChooseFile".ToLower())
                {
                    var dlg = new OpenFileDialog();
                    dlg.Title = "Choose File";
                    if (!string.IsNullOrEmpty(funcParamArray[0]))
                        dlg.InitialDirectory = funcParamArray[0];
                    if (!string.IsNullOrEmpty(funcParamArray[1]))
                    {
                        dlg.Filter = funcParamArray[1];
                    }

                    dlg.Multiselect = false;
                    dlg.RestoreDirectory = true;
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        if (dlg.FileNames.Count() > 0)
                        {
                            var firstFilePath = dlg.FileNames[0];
                            return firstFilePath;
                        }
                    }
                    return string.Empty;
                }

                //no use yet
                else if (funcName.ToLower() == "CompareFile".ToLower())
                {
                    //var result = FileHelper.Compare2Files(funcParamArray[1], funcParamArray[2]).ToString();
                    return string.Empty;
                }

                //dir
                else if (funcName.ToLower() == "ChooseDirectory".ToLower())
                {
                    var dlg = new FolderBrowserDialog();
                    dlg.Description = @"Choose Directory";
                    dlg.SelectedPath = funcParamArray[0];
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        return dlg.SelectedPath;
                    }
                    return string.Empty;
                }

                //calc
                else if (funcName.ToLower() == "Calc".ToLower())
                {
                    if (funcParamArray[0].ToLower() == "add".ToLower())
                    {
                        return (Convert.ToDouble(funcParamArray[1]) + Convert.ToDouble(funcParamArray[2])).ToString();
                    }
                    else if (funcParamArray[0].ToLower() == "sub".ToLower())
                    {
                        return (Convert.ToDouble(funcParamArray[1]) - Convert.ToDouble(funcParamArray[2])).ToString();
                    }
                    else if (funcParamArray[0].ToLower() == "mtp".ToLower())
                    {
                        return (Convert.ToDouble(funcParamArray[1]) * Convert.ToDouble(funcParamArray[2])).ToString();
                    }
                    else if (funcParamArray[0].ToLower() == "div".ToLower())
                    {
                        return (Convert.ToDouble(funcParamArray[1]) / Convert.ToDouble(funcParamArray[2])).ToString();
                    }
                    else if (funcParamArray[0].ToLower() == "rnd".ToLower())
                    {
                        return (Math.Round(Convert.ToDouble(funcParamArray[1]))).ToString();
                    }
                    else if (funcParamArray[0].ToLower() == "spls".ToLower())
                    {
                        return (Convert.ToDouble(funcParamArray[1]) % (Convert.ToDouble(funcParamArray[2]))).ToString();
                    }
                    else throw new ArgumentException("funcName: " + funcName + " has no param: " + funcParamArray[0] + "! ");

                }
                //ifelse
                else if (funcName.ToLower() == "IfElse".ToLower())
                {
                    var con = funcParamArray[0];
                    var returnVal = funcParamArray[1];
                    var returnVal1 = funcParamArray[2];
                    var conArry = con.Split(con.GetSubParamSeparator());
                    var compareVar = conArry[0];
                    var compareFlag = conArry[1];
                    var compareVal = "";
                    if (conArry.Length > 2)
                        compareVal = conArry[2];
                    if (compareFlag.ToLower().Trim() == "Contain".ToLower().Trim())
                    {
                        if (compareVar.Contains(compareVal))
                        {
                            if (!string.IsNullOrEmpty(compareVal))
                            {
                                return returnVal;
                            }
                        }
                    }
                    else if (compareFlag.ToLower().Trim() == "Equal".ToLower().Trim())
                    {
                        if (compareVar == compareVal)
                        {
                            return returnVal;
                        }
                    }
                    else if (compareFlag.ToLower() == "VEqual".ToLower())
                    {
                        if (Convert.ToDouble(compareVar) == Convert.ToDouble(compareVal))
                        {
                            return returnVal;
                        }
                    }
                    else if (compareFlag.ToLower() == "VGreater".ToLower())
                    {
                        if (Convert.ToDouble(compareVar) > Convert.ToDouble(compareVal))
                        {
                            return returnVal;
                        }
                    }
                    else if (compareFlag.ToLower() == "VGreaterEqual".ToLower())
                    {
                        if (Convert.ToDouble(compareVar) >= Convert.ToDouble(compareVal))
                        {
                            return returnVal;
                        }
                    }
                    else if (compareFlag.ToLower() == "VLess".ToLower())
                    {
                        if (Convert.ToDouble(compareVar) < Convert.ToDouble(compareVal))
                        {
                            return returnVal;
                        }
                    }
                    else if (compareFlag.ToLower() == "VLessEqual".ToLower())
                    {
                        if (Convert.ToDouble(compareVar) <= Convert.ToDouble(compareVal))
                        {
                            return returnVal;
                        }
                    }
                    else if (compareFlag.ToLower() == "NotEqual".ToLower())
                    {
                        if (compareVar.Trim() != compareVal.Trim())
                        {
                            return returnVal;
                        }
                    }
                    else if (compareFlag.ToLower() == "IsNull".ToLower())
                    {
                        if (compareVar.IsNullOrEmpty())
                        {
                            return returnVal;
                        }
                    }
                    else if (compareFlag.ToLower() == "IsNotNull".ToLower())
                    {
                        if (!compareVar.IsNullOrEmpty())
                        {
                            return returnVal;
                        }
                    }
                    else
                    {
                        throw new ArgumentException("funcName: " + funcName + " has no compare Flag: '" + compareFlag + "'! ");
                    }

                    return returnVal1;

                } //IfElse ends

                //Status
                else if (funcName.ToLower() == "GetFinalStatus".ToLower())
                {
                    if (funcParamArray.All(v => v.ToLower() == "true"))
                    {
                        return "true";
                    }
                    if (funcParamArray.Any(v => v.ToLower() == "unknown"))
                    {
                        return "unknown";
                    }
                    return "false";
                }

                //getbool
                else if (funcName.ToLower() == "GetBool".ToLower())
                {
                    if (funcParamArray[0].ToLower() == "TotalStatus".ToLower())
                    {
                        var returnStr = "true";
                        var subfuncParamArray = funcParamArray[1].Split(',');
                        if (subfuncParamArray.Any(v => v.ToLower() != "1"))
                        {
                            returnStr = "false";
                        }
                        return returnStr;
                    }
                    else if (funcParamArray[0].ToLower() == "And".ToLower())
                    {
                        var returnStr = "true";
                        var subfuncParamArray = funcParamArray[1].Split(',');
                        if (subfuncParamArray.Any(v => v.ToLower() != "true"))
                        {
                            returnStr = "false";
                        }
                        return returnStr;
                    }
                    else if (funcParamArray[0].ToLower() == "Or".ToLower())
                    {
                        var returnStr = "false";
                        var subfuncParamArray = funcParamArray[1].Split(',');
                        if (subfuncParamArray.Any(v => v.ToLower() == "true"))
                        {
                            returnStr = "true";
                        }
                        return returnStr;
                    }
                    else if (funcParamArray[0].ToLower() == "Not".ToLower())
                    {
                        var returnStr = "true";
                        if (funcParamArray[1].ToLower() == "true") returnStr = "false";
                        return returnStr;
                    }

                    else if (funcParamArray[0].ToLower() == "JudgeStringIsNull".ToLower())
                    {
                        var returnStr = "false";
                        if (string.IsNullOrEmpty(funcParamArray[1])) returnStr = "true";
                        return returnStr;
                    }
                    else if (funcParamArray[0].ToLower() == "JudgeStringIsNotNull".ToLower())
                    {
                        var returnStr = "false";
                        if (!string.IsNullOrEmpty(funcParamArray[1])) returnStr = "true";
                        return returnStr;
                    }
                    else if (funcParamArray[0].ToLower() == "JudgeIfDirectoryExists".ToLower())
                    {
                        var returnStr = "false";
                        if (Directory.Exists(funcParamArray[1])) returnStr = "true";
                        return returnStr;
                    }
                    else if (funcParamArray[0].ToLower() == "JudgeIsDirectoryHidden".ToLower())
                    {
                        if (!Directory.Exists(funcParamArray[1])) return "false";
                        var returnStr = "false";
                        var di = new DirectoryInfo(funcParamArray[1]);
                        if ((di.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                        {
                            returnStr = "true";
                        }
                        return returnStr;
                    }

                    else if (funcParamArray[0].ToLower() == "VerifyInput".ToLower())
                    {
                        var returnStr = "false";
                        var dlg = new TextInputDialog();
                        var verifyRule = funcParamArray[1];
                        var verifyParams = funcParamArray.Length > 2 ? funcParamArray[2] : "";
                        dlg.VerificationRule = verifyRule;
                        dlg.VerificationParams = verifyParams;
                        dlg.ShowDialog();
                        returnStr = dlg.IsOk.ToString().ToLower();
                        return returnStr;
                    }
                    else throw new ArgumentException("funcName: " + funcName + " has no param: " + funcParamArray[0] + "! ");
                }

                else if (funcName.ToLower() == "GetEncryptedText".ToLower())
                {
                    if (funcParamArray[0].ToLower() == "Md5".ToLower())
                    {
                        //return EncryptionHelper.Md5Encrypt(funcParamArray[1]);
                        return string.Empty;
                    }
                    else if (funcParamArray[0].ToLower() == "Rsa".ToLower())
                    {
                        //return EncryptionHelper.RsaEncrypt(funcParamArray[1]);
                        return string.Empty;
                    }
                    else if (funcParamArray[0].ToLower() == "Bitwise".ToLower())
                    {
                        //return EncryptionHelper.BitwiseEncrypt(funcParamArray[1]);
                        return string.Empty;
                    }
                    else if (funcParamArray[0].ToLower() == "Symmetric".ToLower())
                    {
                        return EncryptionHelper.SmEncrypt(funcParamArray[1]);
                    }
                    else if (funcParamArray[0].ToLower() == "TimeDynamic".ToLower())
                    {
                        return string.Empty;
                    }
                    else throw new ArgumentException("funcName: " + funcName + " has no param: " + funcParamArray[0] + "! ");
                }

                else if (funcName.ToLower() == "GetDecryptedText".ToLower())
                {
                    if (funcParamArray[0].ToLower() == "Rsa".ToLower())
                    {
                        //return EncryptionHelper.RsaDecrypt(funcParamArray[1]);
                        return string.Empty;
                    }
                    if (funcParamArray[0].ToLower() == "Symmetric".ToLower())
                    {
                        //return EncryptionHelper.SmDecrypt(funcParamArray[1]);
                        return string.Empty;
                    }
                    else throw new ArgumentException("funcName: " + funcName + " has no param: " + funcParamArray[0] + "! ");
                }

                //xml
                else if (funcName.ToLower() == "GetTableXmlNodeVal".ToLower())
                {
                    var path = funcParamArray[0];
                    var xmlMgr = new XmlHandler(path);
                    return xmlMgr.GetNodeInnerTextByTagName(funcParamArray[1], 0);
                }

                //following can be extended to impl in AdapterGetHelper
                else if (funcName.ToLower() == "GetValidationResult".ToLower())
                {

                    var retStr = TextValidationHelper.Validate(funcParamArray[0], funcParamArray[1]);
                    if (retStr == "OutOfScopeOfTextValidationHelper") return "OutOfScope";
                    else return retStr;
                }

                else if (funcName.ToLower() == "GetJson".ToLower())
                {
                    return "OutOfScope";
                }

                else return "OutOfScope";
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".GetText error: " + ex.Message);
            }
        }

        //#GetValueTextDataTable
        public static DataTable GetValueTextDataTable(string funcName, string[] funcParamArray)
        {
            try
            {
                var dt = new DataTable();
                var valueTexts = new List<ValueText>();
                if (funcName.ToLower() == ("FromXmlByNodeNames".ToLower()))
                {
                    var xmlMgr = new XmlHandler(funcParamArray[0].ToLower());
                    valueTexts = xmlMgr.GetNodeInnerTextsToValueTextList(funcParamArray[1], funcParamArray[2]);
                }
                else if (funcName.ToLower() == ("FromIdLinkedAnnexesXmlByTextTypeByLang".ToLower()))
                {
                    var xmlMgr = new XmlHandler(funcParamArray[0]);//path
                    var enumInt = EnumHelper.GetIdByName<AnnexTextType>(funcParamArray[1]);//type
                    var enum1 = (AnnexTextType)Enum.ToObject(typeof(AnnexTextType), enumInt);
                    valueTexts = xmlMgr.GetTextsToValueTextListByTextTypeByLangFromIdLinkedAnnexesXml(enum1, funcParamArray[2]);
                }
                else if (funcName.ToLower() == ("SplitBySeperator".ToLower()))
                {
                    var strArray = funcParamArray[1].Split(Convert.ToChar(funcParamArray[0]));
                    var i = 0;
                    foreach (var v in strArray)
                    {
                        var valueText = new ValueText();
                        valueText.Value = i.ToString();
                        valueText.Text = v;
                        valueTexts.Add(valueText);
                        i++;
                    }
                }
                else if (funcName.ToLower() == ("SplitBySeperators".ToLower()))
                {
                    var seperator0 = Convert.ToChar(funcParamArray[0]);
                    var seperator1 = Convert.ToChar(funcParamArray[1]);
                    var strArray = funcParamArray[2].Split(seperator0);
                    var i = 0;
                    foreach (var v in strArray)
                    {
                        var arry = v.Split(seperator1);
                        var valueText = new ValueText();
                        valueText.Value = arry[0];
                        valueText.Text = arry[1];
                        valueTexts.Add(valueText);
                        i++;
                    }
                }
                else
                {
                    return null;
                }

                dt.Columns.Add("Value");
                dt.Columns.Add("Text");

                foreach (var vt in valueTexts)
                {
                    DataRow dr;
                    dr = dt.NewRow();
                    dr["Value"] = vt.Value;
                    dr["Text"] = vt.Text;
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".GetValueTextDataTable error: " + ex.Message);
            }
        }

        //#act
        public static string Act(string funcName, string[] funcParamArray)
        {
            var returnStr = "";
            try
            {
                if (funcName.ToLower() == "CreateDirectory".ToLower())
                {
                    if (!Directory.Exists(funcParamArray[0]))
                    {
                        Directory.CreateDirectory(funcParamArray[0]);
                    }
                    if (funcParamArray[1] == "true")
                    {
                        var di = new DirectoryInfo(funcParamArray[0]);
                        if (!((di.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden))
                        {
                            di.Attributes = FileAttributes.Hidden;
                        }
                    }
                }
                else if (funcName.ToLower() == "OpenDirectory".ToLower())
                {
                    if (!Directory.Exists(funcParamArray[0]))
                    {
                        throw new ArgumentException("Directory does not exist! "); ;
                    }
                    else
                    {
                        System.Diagnostics.Process.Start(funcParamArray[0]);
                    }
                }
                else if (funcName.ToLower() == "SelectAndRenameFile".ToLower())
                {
                    var dlgOpenFile = new OpenFileDialog();

                    dlgOpenFile.Title = funcParamArray[0];
                    dlgOpenFile.Filter = funcParamArray[1];
                    dlgOpenFile.InitialDirectory = funcParamArray[2];
                    dlgOpenFile.Multiselect = false;
                    dlgOpenFile.RestoreDirectory = true;
                    if (dlgOpenFile.ShowDialog() == DialogResult.OK)
                    {
                        if (dlgOpenFile.FileNames.Length > 0)
                        {
                            var firstFilePath = dlgOpenFile.FileNames[0];
                            var folder = FileHelper.GetFileDetailByOption(firstFilePath, FilePathComposition.Directory);
                            if (File.Exists(firstFilePath))
                            {
                                File.Move(firstFilePath, folder + "\\" + funcParamArray[3]);
                            }
                            else
                            {
                                throw new ArgumentException("File does not exist! "); ;
                            }
                        }
                    }
                    else
                    {
                        //throw new ArgumentException("Operation Cancelled! ");
                    }
                }

                else if (funcName.ToLower() == "Run".ToLower())
                {
                    var actArgsStr = "";
                    if (funcParamArray.Length > 1)
                        actArgsStr = funcParamArray[1];
                    ProcessHelper.Run(funcParamArray[0], actArgsStr);
                }


                else if (funcName.ToLower() == "OpenFile".ToLower())
                {
                    var actArgsStr = "";
                    ProcessHelper.OpenFile(funcParamArray[0], actArgsStr);
                }
                else if (funcName.ToLower() == "OpenFolder".ToLower())
                {
                    ProcessHelper.OpenFolder(funcParamArray[0]);
                }


                else if (funcName.ToLower() == ("Redirect").ToLower())
                {
                    ProcessHelper.Redirect(funcParamArray[0]);
                }

                else if (funcName.ToLower() == "SendLocalEmail".ToLower())
                {
                    var mailTo = funcParamArray[0];
                    var subject = funcParamArray[1];
                    var body = "";
                    if (funcParamArray.Length > 2)
                        body = funcParamArray[2];
                    if (body.Contains("\r\n"))
                        body = body.Replace("\r\n", "%0D%0A");
                    if (body.Contains("\\r\\n"))
                        body = body.Replace("\\r\\n", "%0D%0A");
                    LocalEmailHelper.Send(mailTo, subject, body);
                }

                else if (funcName.ToLower() == ("RunCmd".ToLower()) | funcName.ToLower() == ("ExecCmd".ToLower()))
                {
                    var actArgsStr = "";
                    var inputStr = "";
                    var execCmdModeStr = "0";
                    if (funcName.ToLower() == ("RunCmd".ToLower()))
                    {
                        if (!File.Exists(funcParamArray[0])) throw new ArgumentException("File:'" + funcParamArray[0] + "' does not exist!");
                        if (funcParamArray.Length > 1)
                            actArgsStr = funcParamArray[1];
                        if (funcParamArray.Length > 2)
                        {
                            if (funcParamArray[2].IsPlusInteger())
                                execCmdModeStr = funcParamArray[2];
                        }
                        inputStr = actArgsStr.IsNullOrEmpty() ? funcParamArray[0] : funcParamArray[0] + " " + actArgsStr;
                    }

                    else if (funcName.ToLower() == "ExecCmd".ToLower())
                    {
                        if (funcParamArray.Length > 1)
                        {
                            if (funcParamArray[1].IsPlusInteger())
                                execCmdModeStr = funcParamArray[1];
                        }
                        inputStr = funcParamArray[0];
                    }

                    var execCmdModeInt = (ExecCmdMode)Convert.ToInt16(execCmdModeStr);
                    var execCmdMode = (ExecCmdMode)(execCmdModeInt);
                    returnStr = ProcessHelper.Cmd(inputStr, execCmdMode);

                }

                else if (funcName.ToLower() == "ExportToExcel".ToLower())
                {
                    var content = funcParamArray[0];
                    content = "UTF格式";
                    var title = "ExpertToExcel".ToUniqueStringByNow();
                    var folder = DirectoryHelper.GetSpecialDir("personal");

                    var path = folder + "\\" + title + ".xls";
                    File.WriteAllText(path, content, Encoding.Default);
                    ProcessHelper.OpenFile(path, "");
                }
                else if (funcName.ToLower() == "EncryptTextFile".ToLower())
                {
                    var path = funcParamArray[0];
                    path = FileHelper.GetFilePath(path, Path.GetDirectoryName(Application.ExecutablePath));
                    var folder = FileHelper.GetFileDetailByOption(path, FilePathComposition.Directory);
                    var fileTitle = FileHelper.GetFileDetailByOption(path, FilePathComposition.FileTitle);
                    var postfix = FileHelper.GetFileDetailByOption(path, FilePathComposition.Postfix);
                    var txt = EncryptionHelper.SmEncrypt(File.ReadAllText(path));
                    var path1 = folder + "\\" + fileTitle + ".E" + postfix.Substring(1, postfix.Length - 1);
                    File.WriteAllText(path1, txt, Encoding.Default);

                    if (funcParamArray.Length > 1 & funcParamArray[1].ToLower() == "true") File.Delete(path);
                }
                else
                {
                    return "OutOfScope";
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Act error: " + ex.Message);
            }
            return returnStr;
        }



        //##common
        public static string ResolveSpecialFolder(string filePath)
        {
            if (filePath.ToLower().Contains("%systemdrive%")) //C:
            {
                var tempStr = DirectoryHelper.GetSpecialDir("systemdrive");
                filePath = Regex.Replace(filePath, "%systemdrive%", tempStr, RegexOptions.IgnoreCase);
            }

            if (filePath.ToLower().Contains("%systemroot%")) //C:\WINDOWS
            {
                var tempStr = DirectoryHelper.GetSpecialDir("systemroot");
                filePath = Regex.Replace(filePath, "%systemroot%", tempStr, RegexOptions.IgnoreCase);
            }
            if (filePath.ToLower().Contains("%systemdirectory%")) //C:\WINDOWS\system32
            {
                var tempStr = DirectoryHelper.GetSpecialDir("systemdirectory");
                filePath = Regex.Replace(filePath, "%systemdirectory%", tempStr, RegexOptions.IgnoreCase);
            }
            if (filePath.ToLower().Contains("%myprofile%")) //win7 C:\Users\chris.li; xp C:\Documents and Settings\Administrator
            {
                var tempStr = DirectoryHelper.GetSpecialDir("myprofile");
                filePath = Regex.Replace(filePath, "%myprofile%", tempStr, RegexOptions.IgnoreCase);
            }

            if (filePath.ToLower().Contains("%mydocuments%")) //win7 C:\Users\chris.li\Documents; xp C:\Documents and Settings\Administrator\My Documents 
            {
                var tempStr = DirectoryHelper.GetSpecialDir("personal");
                filePath = Regex.Replace(filePath, "%mydocuments%", tempStr, RegexOptions.IgnoreCase);
            }
            if (filePath.ToLower().Contains("%personal%")) //win7 C:\Users\chris.li\Documents; xp C:\Documents and Settings\Administrator\My Documents 
            {
                var tempStr = DirectoryHelper.GetSpecialDir("personal");
                filePath = Regex.Replace(filePath, "%personal%", tempStr, RegexOptions.IgnoreCase);
            }

            if (filePath.ToLower().Contains("%commonapplicationdata%")) //win7 C:\ProgramData; xp C:\Documents and Settings\All Users\Application Data
            {
                var tempStr = DirectoryHelper.GetSpecialDir("commonapplicationdata");
                filePath = Regex.Replace(filePath, "%commonapplicationdata%", tempStr, RegexOptions.IgnoreCase);
            }

            if (filePath.ToLower().Contains("%Localapplicationdata%")) //win7 C:\Users\chris.li\AppData\Local; xp C:\Documents and Settings\Administrator\Local Settings\Application Data	
            {
                var tempStr = DirectoryHelper.GetSpecialDir("Localapplicationdata");
                filePath = Regex.Replace(filePath, "%Localapplicationdata%", tempStr, RegexOptions.IgnoreCase);
            }

            if (filePath.ToLower().Contains("%applicationdata%")) //win7 C:\Users\chris.li\AppData\Roaming; xp C:\Documents and Settings\Administrator\Application Data 
            {
                var tempStr = DirectoryHelper.GetSpecialDir("applicationdata");
                filePath = Regex.Replace(filePath, "%applicationdata%", tempStr, RegexOptions.IgnoreCase);
            }
            if (filePath.ToLower().Contains("%roamingapplicationdata%")) //win7 C:\Users\chris.li\AppData\Roaming; xp C:\Documents and Settings\Administrator\Application Data 
            {
                var tempStr = DirectoryHelper.GetSpecialDir("applicationdata");
                filePath = Regex.Replace(filePath, "%roamingapplicationdata%", tempStr, RegexOptions.IgnoreCase);
            }

            if (filePath.ToLower().Contains("%programfiles%"))
            {
                var tempStr = DirectoryHelper.GetSpecialDir("programfiles");
                filePath = Regex.Replace(filePath, "%programfiles%", tempStr, RegexOptions.IgnoreCase);
            }
            return filePath;
        }


        //##FormatRichText
        public static string FormatRichText(string text)
        {
            try
            {
                text = text.Replace("\\r\\t", "\t");
                text = text.Replace("\\r\\n", "\n");
                return text;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".FormatRichText Error: " + ex.Message);
            }
        }

        public static bool IsZoneInputVariable(string text)
        {
            try
            {
                var tempArry = text.Split('_');
                if (tempArry.Length > 1)
                {
                    var lastStr = tempArry[tempArry.Length - 1];
                    if (lastStr.EndsWith("#"))
                    {
                        var inputNo = lastStr.Split('#')[0];
                        if (inputNo.IsPlusInteger() | inputNo == "0")
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".IsZoneInputVariable Error: " + ex.Message);
            }
        }




    }
}