using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Ligg.Base.Extension;
using Ligg.WinForm.Controls;
using Ligg.WinForm.DataModel.Enums;
using ContentAlignment = System.Drawing.ContentAlignment;

namespace Ligg.WinForm.Helpers
{
    public static class ControlHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        //#Panel
        public static void SetPanelStyleByClass(Panel control, string styleClass)
        {
            try
            {
                if (styleClass.IsNullOrEmpty()) return;
                var myType = typeof(StyleSheet);
                var myInfo = myType.GetField("PanelClass" + "_" + styleClass);
                if (myInfo == null) return;
                var styleText = Convert.ToString(myInfo.GetValue(null));
                if (styleText.IsNullOrEmpty()) return;
                SetPanelStyleByText(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetSplitRectangleStyleByClass Error: " + ex.Message);
            }
        }

        public static void SetPanelStyleByText(Panel control, string styleText)
        {
            try
            {
                if (styleText.IsNullOrEmpty()) return;
                ControlHelper.SetControlBackColor(control, styleText);
                SetControlPadding(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetPanelStyleByText Error: " + ex.Message);
            }
        }

        //#ShadowPanel
        public static void SetShadowPanelStyleByClass(Panel control, string styleClass)
        {
            try
            {
                if (styleClass.IsNullOrEmpty()) return;
                var myType = typeof(StyleSheet);
                var myInfo = myType.GetField("ShadowPanelClass" + "_" + styleClass);
                if (myInfo == null) return;
                var styleText = Convert.ToString(myInfo.GetValue(null));
                if (styleText.IsNullOrEmpty()) return;
                SetShadowPanelStyleByText(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetShadowPanelStyleByClass Error: " + ex.Message);
            }
        }

        public static void SetShadowPanelStyleByText(Panel control, string styleText)
        {
            try
            {
                if (styleText.IsNullOrEmpty()) return;
                SetPanelStyleByText(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetShadowPanelStyleByText Error: " + ex.Message);
            }
        }


        //#ContainerPanel
        public static void SetContainerPanelStyleByClass(ContainerPanel control, string styleClass)
        {
            try
            {
                if (styleClass.IsNullOrEmpty()) return;
                var myType = typeof(StyleSheet);
                var myInfo = myType.GetField("ContainerPanelClass" + "_" + styleClass);
                if (myInfo == null) return;
                var styleText = Convert.ToString(myInfo.GetValue(null));
                if (styleText.IsNullOrEmpty()) return;
                SetContainerPanelStyleByText(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetContainerPanelStyleByClass Error: " + ex.Message);
            }
        }

        public static void SetContainerPanelStyleByText(ContainerPanel control, string styleText)
        {
            try
            {
                if (styleText.IsNullOrEmpty()) return;
                ControlHelper.SetControlBackColor(control, styleText);
                SetControlPadding(control, styleText);
                var style = styleText.GetStyleValue("Style");
                if (!string.IsNullOrEmpty(style))
                {
                    if (style == "borders" | style == "rounded")
                    {
                        var borderColor = styleText.GetStyleValue("BorderColor");
                        if (!borderColor.IsNullOrEmpty())
                        {
                            var boderColorArray = borderColor.Split(',');
                            control.BorderColor = Color.FromArgb(Convert.ToInt32(boderColorArray[0]),
                                                                       Convert.ToInt32(boderColorArray[1]),
                                                                       Convert.ToInt32(boderColorArray[2]));

                        }

                        if (style == "borders")
                        {
                            control.StyleType = ContainerPanel.ContainerPanelStyle.Borders;

                            var borderWidth = styleText.GetStyleValue("BorderWidth");
                            if (!borderWidth.IsNullOrEmpty())
                            {
                                var borderWidthArray = borderWidth.Split(',');
                                if (borderWidthArray.Length == 1)
                                {
                                    var w = Convert.ToInt16(borderWidthArray[0]);
                                    control.BorderWidthOnTop = w;
                                    control.BorderWidthOnRight = w;
                                    control.BorderWidthOnBottom = w;
                                    control.BorderWidthOnLeft = w;
                                }
                                else if (borderWidthArray.Length == 2)
                                {
                                    var w1 = Convert.ToInt16(borderWidthArray[0]);
                                    var w2 = Convert.ToInt16(borderWidthArray[1]);
                                    control.BorderWidthOnTop = w1;
                                    control.BorderWidthOnRight = w2;
                                    control.BorderWidthOnBottom = w1;
                                    control.BorderWidthOnLeft = w2;
                                }
                                else if (borderWidthArray.Length == 4)
                                {
                                    var w1 = Convert.ToInt16(borderWidthArray[0]);
                                    var w2 = Convert.ToInt16(borderWidthArray[1]);
                                    var w3 = Convert.ToInt16(borderWidthArray[2]);
                                    var w4 = Convert.ToInt16(borderWidthArray[3]);
                                    control.BorderWidthOnTop = w1;
                                    control.BorderWidthOnRight = w2;
                                    control.BorderWidthOnBottom = w3;
                                    control.BorderWidthOnLeft = w4;
                                }
                            }
                        }
                        else if (style == "rounded")
                        {
                            control.StyleType = ContainerPanel.ContainerPanelStyle.Rounded;
                            control.RoundStyle = RoundStyle.All;
                            var radius = styleText.GetStyleValue("Radius");
                            if (!radius.IsNullOrEmpty())
                            {
                                control.Radius = Convert.ToInt16(radius);
                            }
                        }
                    }
                    SetPanelStyleByText(control, styleText);
                }


            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetContainerPanelStyleByText Error: " + ex.Message);
            }
        }

        //#SplitRectangle
        public static void SetSplitRectangleStyleByClass(SplitRectangle control, string styleClass)
        {
            try
            {
                if (styleClass.IsNullOrEmpty()) return;
                var myType = typeof(StyleSheet);
                var myInfo = myType.GetField("SplitRectangleClass" + "_" + styleClass);
                if (myInfo == null) return;
                var styleText = Convert.ToString(myInfo.GetValue(null));
                if (styleText.IsNullOrEmpty()) return;
                SetSplitRectangleStyleByText(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetSplitRectangleStyleByClass Error: " + ex.Message);
            }
        }

        public static void SetSplitRectangleStyleByText(SplitRectangle cpnt, string styleText)
        {
            try
            {
                if (styleText.IsNullOrEmpty()) return;
                SetControlBackColor(cpnt, styleText);
                SetControlPadding(cpnt, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>>  " + TypeName + ".SetSplitRectangleStyleByText Error: " + ex.Message);
            }
        }

        public static void SetToolStripMenuItemStyleByText(ToolStripMenuItem cpnt, string styleText)
        {
            try
            {
                /* not work
                var backColorStr = styleText.GetStyleValue("BackColor");
                if (!string.IsNullOrEmpty(backColorStr))
                {
                    if (backColorStr.ToLower() == "transparent")
                    {
                        cpnt.BackColor = Color.Transparent;
                    }
                    else
                    {
                        var backColorStrArray = backColorStr.Split(',');
                        cpnt.BackColor = Color.FromArgb(Convert.ToInt32(backColorStrArray[0]),
                            Convert.ToInt32(backColorStrArray[1]),
                            Convert.ToInt32(backColorStrArray[2]));
                    }

                }
                */

                //*valid for each level.
                var foreColorStr = styleText.GetStyleValue("ForeColor");
                if (!string.IsNullOrEmpty(foreColorStr))
                {
                    if (foreColorStr.ToLower() == "transparent")
                    {
                        cpnt.ForeColor = Color.Transparent;
                    }
                    else
                    {
                        var backColorStrArray = foreColorStr.Split(',');
                        cpnt.ForeColor = Color.FromArgb(Convert.ToInt32(backColorStrArray[0]),
                            Convert.ToInt32(backColorStrArray[1]),
                            Convert.ToInt32(backColorStrArray[2]));
                    }
                }

                //*only valid&works for top level 
                var textAlign = styleText.GetStyleValue("TextAlign").ToLower();
                if (!string.IsNullOrEmpty(textAlign))
                {
                    switch (textAlign)
                    {
                        case "middleleft":
                            cpnt.TextAlign = ContentAlignment.MiddleLeft;
                            break;
                        case "middleRight":
                            cpnt.TextAlign = ContentAlignment.MiddleLeft;
                            break;
                        default:
                            cpnt.TextAlign = ContentAlignment.MiddleCenter;
                            break;
                    }
                }

                //*only valid for top level. works form top level to end level
                //*when culture is zh-CN, western font not work. When change to en-US, works
                if (styleText.IsNullOrEmpty()) return;
                var fontStr = styleText.GetStyleValue("Font");
                if (string.IsNullOrEmpty(fontStr))
                {
                    fontStr = "Font:Microsoft Sans Serif";
                }

                //*valid for each level. works from self level to end level
                float fontSize = 8;
                var fontSizeStr = styleText.GetStyleValue("FontSize");
                if (string.IsNullOrEmpty(fontSizeStr)) fontSize = cpnt.Font.Size;
                else fontSize = Convert.ToSingle(fontSizeStr, CultureInfo.InvariantCulture);

                //*valid for each level. works from self level to next level
                var fontStyleStrs = styleText.GetStyleValue("FontStyle");
                var fontStyle = new FontStyle();
                if (!string.IsNullOrEmpty(fontStyleStrs))
                {
                    var fontStyleStrArry = fontStyleStrs.Split(',');
                    foreach (var fontStyleStr in fontStyleStrArry)
                    {
                        if (fontStyleStr.ToLower() == "bold")
                        {
                            fontStyle = fontStyle | FontStyle.Bold;
                        }
                        else if (fontStyleStr.ToLower() == "italic")
                        {
                            fontStyle = fontStyle | FontStyle.Italic;
                        }
                        else if (fontStyleStr.ToLower() == "underline")
                        {
                            fontStyle = fontStyle | FontStyle.Underline;
                        }
                        else if (fontStyleStr.ToLower() == "strikeout")
                        {
                            fontStyle = fontStyle | FontStyle.Strikeout;
                        }
                    }

                }
                else
                {
                    fontStyle = cpnt.Font.Style;
                }
                cpnt.Font = new Font(fontStr, fontSize, fontStyle);



            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>>  " + TypeName + ".ToolStripMenuItemStyleByText Error: " + ex.Message);
            }
        }






        //#ImageTextButton
        public static void SetImageTextButtonStyleByClass(ImageTextButton control, string styleClass)
        {
            try
            {
                if (styleClass.IsNullOrEmpty()) return;
                var myType = typeof(StyleSheet);
                var myInfo = myType.GetField("ImageTextButtonClass" + "_" + styleClass);
                if (myInfo == null) return;
                var styleText = Convert.ToString(myInfo.GetValue(null));
                if (styleText.IsNullOrEmpty()) return;
                SetImageTextButtonStyleByText(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetImageTextButtonStyleByClass Error: " + ex.Message);
            }
        }



        public static void SetImageTextButtonStyleByText(ImageTextButton control, string styleText)
        {
            try
            {
                if (styleText.IsNullOrEmpty()) return;
                var hasBorder = styleText.GetStyleValue("HasBorder");
                if (!hasBorder.IsNullOrEmpty())
                {
                    if (hasBorder == "false")
                    {
                        control.HasBorder = false;
                    }
                    else
                    {
                        control.HasBorder = true;
                    }
                }

                var offset = styleText.GetStyleValue("Offset");

                if (!offset.IsNullOrEmpty())
                {
                    control.Offset = offset;
                }

                SetButtonTextAlign(control, styleText);

                SetControlBackColor(control, styleText);
                SetControlForeColor(control, styleText);
                SetControlFont(control, styleText);
                SetControlBackgroundImage(control, styleText);
                SetControlPadding(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetImageTextButtonStyleByText Error: " + ex.Message);
            }
        }

        //#TextButton
        public static void SetTextButtonStyleByClass(TextButton control, string styleClass)
        {
            try
            {
                if (styleClass.IsNullOrEmpty()) return;
                var myType = typeof(StyleSheet);
                var styleText = "";
                var myInfo = myType.GetField("TextButtonClass" + "_" + styleClass);
                if (myInfo == null) return;
                styleText = Convert.ToString(myInfo.GetValue(null));

                SetTextButtonStyleByText(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetTextButtonStyleByClass Error: " + ex.Message);
            }
        }

        public static void SetTextButtonStyleByText(TextButton control, string styleText)
        {
            try
            {
                if (styleText.IsNullOrEmpty()) return;
                var hasBorder = styleText.GetStyleValue("HasBorder");
                if (!hasBorder.IsNullOrEmpty())
                {
                    if (hasBorder == "false")
                    {
                        control.HasBorder = false;
                    }
                    else
                    {
                        control.HasBorder = true;
                    }
                }

                var sensitiveType = styleText.GetStyleValue("SensitiveType");
                if (!sensitiveType.IsNullOrEmpty())
                {
                    if (sensitiveType == "check")
                    {
                        control.SensitiveType = ControlSensitiveType.Check;
                    }
                    else if (sensitiveType == "focus")
                    {
                        control.SensitiveType = ControlSensitiveType.Focus;
                    }
                    else
                    {
                        control.SensitiveType = ControlSensitiveType.None;
                    }
                }


                SetButtonTextAlign(control, styleText);
                SetControlBackColor(control, styleText);
                SetControlForeColor(control, styleText);
                SetControlFont(control, styleText);
                SetControlPadding(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetTextButtonStyleByText Error: " + ex.Message);
            }
        }



        public static void SetButtonTextAlign(Button control, string styleText)
        {
            try
            {
                var textAlign = styleText.GetStyleValue("TextAlign").ToLower();
                if (!string.IsNullOrEmpty(textAlign))
                {
                    switch (textAlign)
                    {
                        case "middleleft":
                            control.TextAlign = ContentAlignment.MiddleLeft;
                            break;
                        case "middleRight":
                            control.TextAlign = ContentAlignment.MiddleLeft;
                            break;
                        default:
                            control.TextAlign = ContentAlignment.MiddleCenter;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetButtonTextAlign Error: " + ex.Message);
            }
        }

        //#StatusLight
        public static void SetStatusLightStyleByClass(StatusLight control, string styleClass)
        {
            try
            {
                if (styleClass.IsNullOrEmpty()) return;
                var myType = typeof(StyleSheet);
                var styleText = "";
                var myInfo = myType.GetField("StatusLightClass" + "_" + styleClass);
                if (myInfo == null) return;
                styleText = Convert.ToString(myInfo.GetValue(null));
                if (styleText.IsNullOrEmpty()) return;
                SetStatusLightStyleByText(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetStatusLightStyleByClass Error: " + ex.Message);
            }
        }

        public static void SetStatusLightStyleByText(StatusLight control, string styleText)
        {
            try
            {
                if (styleText.IsNullOrEmpty()) return;
                control.LabelStyle = styleText;
                SetControlBackColor(control, styleText);
                SetControlForeColor(control, styleText);
                SetControlFont(control, styleText);
                SetControlPadding(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetStatusLightStyleByText Error: " + ex.Message);
            }

        }

        //#ScoreLight
        public static void SetScoreLightStyleByClass(ScoreLight control, string styleClass)
        {
            try
            {
                if (styleClass.IsNullOrEmpty()) return;
                var myType = typeof(StyleSheet);
                var styleText = "";
                var myInfo = myType.GetField("ScoreLightClass" + "_" + styleClass);
                if (myInfo == null) return;
                styleText = Convert.ToString(myInfo.GetValue(null));
                if (styleText.IsNullOrEmpty()) return;
                SetScoreLightStyleByText(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetScoreLightStyleByClass Error: " + ex.Message);
            }
        }

        public static void SetScoreLightStyleByText(ScoreLight control, string styleText)
        {
            try
            {
                if (styleText.IsNullOrEmpty()) return;
                control.LabelStyle = styleText;
                SetControlBackColor(control, styleText);
                SetControlForeColor(control, styleText);
                SetControlFont(control, styleText);
                SetControlPadding(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetScoreLightStyleByText Error: " + ex.Message);
            }

        }

        //#CommandLabel
        public static void SetCommandLabelStyleByClass(CommandLabel control, string styleClass)
        {
            try
            {
                if (styleClass.IsNullOrEmpty()) return;
                var myType = typeof(StyleSheet);
                var myInfo = myType.GetField("CommandLabelClass" + "_" + styleClass);
                if (myInfo == null) return;
                var styleText = Convert.ToString(myInfo.GetValue(null));
                if (styleText.IsNullOrEmpty()) return;
                SetCommandLabelStyleByText(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetCommandLabelStyleByClass Error: " + ex.Message);
            }

        }


        public static void SetCommandLabelStyleByText(CommandLabel control, string styleText)
        {
            try
            {
                if (styleText.IsNullOrEmpty()) return;
                var changeColorOnClickStr = styleText.GetStyleValue("ChangeColorOnClick");
                if (!string.IsNullOrEmpty(changeColorOnClickStr))
                {
                    var changeColorOnClick = changeColorOnClickStr.ToLower() == "true" ? true : false;
                    control.ChangeColorOnClick = changeColorOnClick;
                }

                var hasBottomLineStr = styleText.GetStyleValue("HasBottomLine");
                if (!string.IsNullOrEmpty(hasBottomLineStr))
                {
                    var hasBottomLine = hasBottomLineStr.ToLower() == "true" ? true : false;
                    control.HasBottomLine = hasBottomLine;
                }

                var bottomLineColorStr = styleText.GetStyleValue("BottomLineColor");
                if (!string.IsNullOrEmpty(bottomLineColorStr))
                {
                    var bottomLineColorStrArray = bottomLineColorStr.Split(',');
                    control.BottomLineColor = Color.FromArgb(Convert.ToInt32(bottomLineColorStrArray[0]), Convert.ToInt32(bottomLineColorStrArray[1]), Convert.ToInt32(bottomLineColorStrArray[2]));
                }

                var hoveringColorStr = styleText.GetStyleValue("HoveringColor");
                if (!string.IsNullOrEmpty(hoveringColorStr))
                {
                    var hoveringColorStrArray = hoveringColorStr.Split(',');
                    control.HoveringColor = Color.FromArgb(Convert.ToInt32(hoveringColorStrArray[0]), Convert.ToInt32(hoveringColorStrArray[1]), Convert.ToInt32(hoveringColorStrArray[2]));
                }

                var focusedColorColorStr = styleText.GetStyleValue("FocusedColor");
                if (!string.IsNullOrEmpty(focusedColorColorStr))
                {
                    var focusedColorColorStrArray = focusedColorColorStr.Split(',');
                    control.FocusedColor = Color.FromArgb(Convert.ToInt32(focusedColorColorStrArray[0]), Convert.ToInt32(focusedColorColorStrArray[1]), Convert.ToInt32(focusedColorColorStrArray[2]));
                }


                ControlHelper.SetLabelStyleByText(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetCommandLabelStyleByText Error: " + ex.Message);
            }

        }


        //#TitleLabel
        public static void SetTitleLabelStyleByClass(TitleLabel control, string styleClass)
        {
            try
            {
                if (styleClass.IsNullOrEmpty()) return;
                var myType = typeof(StyleSheet);
                var myInfo = myType.GetField("TitleLabelClass" + "_" + styleClass);
                if (myInfo == null) return;
                var styleText = Convert.ToString(myInfo.GetValue(null));
                if (styleText.IsNullOrEmpty()) return;
                SetTitleLabelStyleByText(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetTitleLabelStyleByClass Error: " + ex.Message);
            }

        }


        public static void SetTitleLabelStyleByText(TitleLabel control, string styleText)
        {
            try
            {
                if (styleText.IsNullOrEmpty()) return;
                var hasBottomLineStr = styleText.GetStyleValue("HasBottomLine");
                if (!string.IsNullOrEmpty(hasBottomLineStr))
                {
                    var hasBottomLine = hasBottomLineStr.ToLower() == "true" ? true : false;
                    control.HasBottomLine = hasBottomLine;
                }

                var bottomLineColorStr = styleText.GetStyleValue("BottomLineColor");
                if (!string.IsNullOrEmpty(bottomLineColorStr))
                {
                    var fornColorStrArray = bottomLineColorStr.Split(',');
                    control.BottomLineColor = Color.FromArgb(Convert.ToInt32(fornColorStrArray[0]), Convert.ToInt32(fornColorStrArray[1]), Convert.ToInt32(fornColorStrArray[2]));
                }

                ControlHelper.SetLabelStyleByText(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetTitleLabelStyleByText Error: " + ex.Message);
            }

        }

        //#Label
        public static void SetLabelStyleByClass(Label control, string styleClass)
        {
            try
            {
                if (styleClass.IsNullOrEmpty()) return;
                var myType = typeof(StyleSheet);
                var myInfo = myType.GetField("LabelClass" + "_" + styleClass);
                if (myInfo == null) return;
                var styleText = Convert.ToString(myInfo.GetValue(null));
                if (styleText.IsNullOrEmpty()) return;
                SetLabelStyleByText(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetLabelStyleByClass Error: " + ex.Message);
            }
        }

        public static void SetLabelStyleByText(Label control, string styleText)
        {
            try
            {
                if (styleText.IsNullOrEmpty()) return;
                var textAlign = styleText.GetStyleValue("TextAlign");
                if (!string.IsNullOrEmpty(textAlign))
                {
                    switch (textAlign)
                    {
                        case "middlecenter":
                            control.TextAlign = ContentAlignment.MiddleCenter;
                            break;
                        case "middleleft":
                            control.TextAlign = ContentAlignment.MiddleLeft;
                            break;
                        case "middleright":
                            control.TextAlign = ContentAlignment.MiddleRight;
                            break;
                        case "bottomleft":
                            control.TextAlign = ContentAlignment.BottomLeft;
                            break;
                        case "bottomright":
                            control.TextAlign = ContentAlignment.BottomRight;
                            break;
                        default:
                            control.TextAlign = ContentAlignment.MiddleLeft;
                            break;
                    }
                }

                SetControlBackColor(control, styleText);
                SetControlForeColor(control, styleText);
                SetControlFont(control, styleText);
                SetControlBackgroundImage(control, styleText);
                SetControlPadding(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>>  " + TypeName + ".SetLabelStyleByText Error: " + ex.Message);
            }
        }

        //#CheckBox
        public static void SetCheckBoxStyleByClass(CheckBox control, string styleClass)
        {
            try
            {
                if (styleClass.IsNullOrEmpty()) return;
                var myType = typeof(StyleSheet);
                var myInfo = myType.GetField("CheckBoxClass" + "_" + styleClass);
                if (myInfo == null) return;
                var styleText = Convert.ToString(myInfo.GetValue(null));
                if (styleText.IsNullOrEmpty()) return;
                SetCheckBoxStyleByText(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetCheckBoxStyleByClass Error: " + ex.Message);
            }
        }

        public static void SetCheckBoxStyleByText(CheckBox control, string styleText)
        {
            try
            {
                if (styleText.IsNullOrEmpty()) return;
                var textAlign = styleText.GetStyleValue("TextAlign");
                if (!string.IsNullOrEmpty(textAlign))
                {
                    switch (textAlign)
                    {
                        case "middlecenter":
                            control.TextAlign = ContentAlignment.MiddleCenter;
                            break;
                        case "middleleft":
                            control.TextAlign = ContentAlignment.MiddleLeft;
                            break;
                        case "middleright":
                            control.TextAlign = ContentAlignment.MiddleRight;
                            break;
                        case "bottomleft":
                            control.TextAlign = ContentAlignment.BottomLeft;
                            break;
                        case "bottomright":
                            control.TextAlign = ContentAlignment.BottomRight;
                            break;
                        default:
                            control.TextAlign = ContentAlignment.MiddleLeft;
                            break;
                    }
                }

                SetControlBackColor(control, styleText);
                SetControlForeColor(control, styleText);
                SetControlFont(control, styleText);
                SetControlPadding(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetCheckBoxStyleByText Error: " + ex.Message);
            }
        }

        //#RichTextBox
        public static void SetRichTextBoxStyleByClass(RichTextBox control, string styleClass)
        {
            try
            {
                if (styleClass.IsNullOrEmpty()) return;
                var myType = typeof(StyleSheet);
                var myInfo = myType.GetField("RichTextBoxClass" + "_" + styleClass);
                if (myInfo == null) return;
                var styleText = Convert.ToString(myInfo.GetValue(null));
                if (styleText.IsNullOrEmpty()) return;
                SetRichTextBoxStyleByText(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetCheckBoxStyleByClass Error: " + ex.Message);
            }
        }

        public static void SetRichTextBoxStyleByText(RichTextBox control, string styleText)
        {
            try
            {
                if (styleText.IsNullOrEmpty()) return;
                var borderStyle = styleText.GetStyleValue("BorderStyle");
                if (!string.IsNullOrEmpty(borderStyle))
                {
                    switch (borderStyle)
                    {
                        case "none":
                            control.BorderStyle = BorderStyle.None;
                            break;
                        case "fixed3d":
                            control.BorderStyle = BorderStyle.Fixed3D;
                            break;
                        case "fixedsingle":
                            control.BorderStyle = BorderStyle.FixedSingle;
                            break;
                        default:
                            control.BorderStyle = BorderStyle.None;
                            break;
                    }
                }

                SetControlBackColor(control, styleText);
                SetControlForeColor(control, styleText);
                SetControlFont(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetCheckBoxStyleByText Error: " + ex.Message);
            }
        }

        //#Radio
        public static void SetRadioButtonStyleByClass(RadioButton control, string styleClass)
        {
            try
            {
                if (styleClass.IsNullOrEmpty()) return;
                var myType = typeof(StyleSheet);
                var myInfo = myType.GetField("RadioButton" + "_" + styleClass);
                if (myInfo == null) return;
                var styleText = Convert.ToString(myInfo.GetValue(null));
                if (styleText.IsNullOrEmpty()) return;
                SetRadioButtonStyleByText(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetRadioButtonByClass Error: " + ex.Message);
            }
        }

        public static void SetRadioButtonStyleByText(RadioButton control, string styleText)
        {
            try
            {
                if (styleText.IsNullOrEmpty()) return;
                var textAlign = styleText.GetStyleValue("TextAlign");
                if (!string.IsNullOrEmpty(textAlign))
                {
                    switch (textAlign)
                    {
                        case "middlecenter":
                            control.TextAlign = ContentAlignment.MiddleCenter;
                            break;
                        case "middleleft":
                            control.TextAlign = ContentAlignment.MiddleLeft;
                            break;
                        case "middleright":
                            control.TextAlign = ContentAlignment.MiddleRight;
                            break;
                        case "bottomleft":
                            control.TextAlign = ContentAlignment.BottomLeft;
                            break;
                        case "bottomright":
                            control.TextAlign = ContentAlignment.BottomRight;
                            break;
                        default:
                            control.TextAlign = ContentAlignment.MiddleLeft;
                            break;
                    }
                }

                SetControlBackColor(control, styleText);
                SetControlForeColor(control, styleText);
                SetControlFont(control, styleText);
                SetControlPadding(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetRadioButtonStyleByText Error: " + ex.Message);
            }
        }

        //#ProgressBar
        public static void SetProgressBarStyleByClass(ProgressBar control, string styleClass)
        {
            try
            {
                if (styleClass.IsNullOrEmpty()) return;
                var myType = typeof(StyleSheet);
                var myInfo = myType.GetField("ProgressBarClass" + "_" + styleClass);
                if (myInfo == null) return;
                var styleText = Convert.ToString(myInfo.GetValue(null));
                if (styleText.IsNullOrEmpty()) return;
                SetProgressBarStyleByText(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetProgressBarStyleByClass Error: " + ex.Message);
            }
        }

        public static void SetProgressBarStyleByText(ProgressBar control, string styleText)
        {
            try
            {
                if (styleText.IsNullOrEmpty()) return;
                SetControlBackColor(control, styleText);
                SetControlForeColor(control, styleText);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetProgressBarStyleByText Error: " + ex.Message);
            }
        }




        //##func
        //##set

        public static void SetControlDockStyleAndLocationAndSize(Control control, int dockStyle, int width, int height, int x, int y)
        {
            try
            {
                if (dockStyle == (int)ControlDockType.None)
                {
                    control.Dock = DockStyle.None;
                    control.Width = width;
                    control.Height = height;
                    control.Location = new Point(x, y);
                }
                else if (dockStyle == (int)ControlDockType.Top)
                {
                    control.Dock = DockStyle.Top;
                    control.Height = height;
                }
                else if (dockStyle == (int)ControlDockType.Right)
                {
                    control.Dock = DockStyle.Right;
                    control.Width = width;
                }
                else if (dockStyle == (int)ControlDockType.Bottom)
                {
                    control.Dock = DockStyle.Bottom;
                    control.Height = height;
                }
                else if (dockStyle == (int)ControlDockType.Left)
                {
                    control.Dock = DockStyle.Left;
                    control.Width = width;
                }
                else if (dockStyle == (int)ControlDockType.Fill)
                {
                    control.Dock = DockStyle.Fill;
                }
                //else //regard as none
                //{
                //    control.Dock = DockStyle.None;
                //    control.Width = width;
                //    control.Height = height;
                //    control.Location = new Point(x == -1 ? StyleSet.DefaultOffsetX : x, y == -1 ? StyleSet.DefaultOffsetY : y);
                //}

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetControlDockStyleAndLocationAndSize Error: " + ex.Message);
            }
        }

        public static void SetControlOffsetByDockStyle(Control offsetCrtl, int dockStyle, int offsetX, int offsetY)
        {
            try
            {
                if (dockStyle == (int)ControlDockType.Top)
                {
                    offsetCrtl.Dock = DockStyle.Top;
                    offsetCrtl.Height = offsetY;
                }
                else if (dockStyle == (int)ControlDockType.Right)
                {
                    offsetCrtl.Dock = DockStyle.Right;
                    offsetCrtl.Width = offsetX;
                }
                else if (dockStyle == (int)ControlDockType.Bottom)
                {
                    offsetCrtl.Dock = DockStyle.Bottom;
                    offsetCrtl.Height = offsetY;
                }
                else if (dockStyle == (int)ControlDockType.Left)
                {
                    offsetCrtl.Dock = DockStyle.Left;
                    offsetCrtl.Width = offsetX;
                }
                else //none or fill
                {
                    //no this case
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetControlOffsetByDockStyle Error: " + ex.Message);
            }
        }

        public static void HideControlByDockStyle(Control control, int dockStyle)
        {
            try
            {
                if (dockStyle == (int)ControlDockType.None)
                {
                    control.Dock = DockStyle.None;
                    control.Width = 0;
                    control.Height = 0;
                }
                else if (dockStyle == (int)ControlDockType.Top)
                {
                    control.Dock = DockStyle.Top;
                    control.Height = 0;
                }
                else if (dockStyle == (int)ControlDockType.Right)
                {
                    control.Dock = DockStyle.Right;
                    control.Width = 0;
                }
                else if (dockStyle == (int)ControlDockType.Bottom)
                {
                    control.Dock = DockStyle.Bottom;
                    control.Height = 0;
                }
                else if (dockStyle == (int)ControlDockType.Left)
                {
                    control.Dock = DockStyle.Left;
                    control.Width = 0;
                }
                else if (dockStyle == (int)ControlDockType.Fill)
                {
                    control.Dock = DockStyle.None;
                    control.Width = 0;
                    control.Height = 0;
                }
                else //regard as none
                {
                    control.Dock = DockStyle.None;
                    control.Width = 0;
                    control.Height = 0;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".HideControlByDockStyle Error: " + ex.Message);
            }
        }

        public static void SetControlPadding(Control control, string styleText)
        {
            try
            {
                var padding = styleText.GetStyleValue("Padding").ToLower();
                if (!string.IsNullOrEmpty(padding))
                {
                    if (!padding.Contains(","))
                    {
                        control.Padding = new Padding(Convert.ToInt32(padding));
                    }
                    else if (padding.GetQtyOfIncludedChar(',') == 1)
                    {
                        var paddingArry = padding.Split(',');
                        control.Padding = new Padding(Convert.ToInt32(paddingArry[1]), Convert.ToInt32(paddingArry[0]), Convert.ToInt32(paddingArry[1]), Convert.ToInt32(paddingArry[0]));
                    }
                    else if (padding.GetQtyOfIncludedChar(',') == 3)
                    {
                        var paddingArry = padding.Split(',');
                        control.Padding = new Padding(Convert.ToInt32(paddingArry[3]), Convert.ToInt32(paddingArry[0]), Convert.ToInt32(paddingArry[1]), Convert.ToInt32(paddingArry[2]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetControlPadding Error: " + ex.Message);
            }
        }


        public static void SetControlBackColor(Control control, string styleText)
        {
            try
            {
                var backColorStr = styleText.GetStyleValue("BackColor");

                if (!string.IsNullOrEmpty(backColorStr))
                {
                    if (backColorStr.ToLower() == "transparent")
                    {
                        control.BackColor = Color.Transparent;
                    }
                    else
                    {
                        var backColorStrArray = backColorStr.Split(',');
                        control.BackColor = Color.FromArgb(Convert.ToInt32(backColorStrArray[0]),
                                                                   Convert.ToInt32(backColorStrArray[1]),
                                                                   Convert.ToInt32(backColorStrArray[2]));
                    }

                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetControlBackColor Error: " + ex.Message);
            }
        }

        public static void SetControlForeColor(Control control, string styleText)
        {
            try
            {
                var foreColorStr = styleText.GetStyleValue("ForeColor");

                if (!string.IsNullOrEmpty(foreColorStr))
                {
                    if (foreColorStr.ToLower() == "transparent")
                    {
                        control.ForeColor = Color.Transparent;
                    }
                    else
                    {
                        var backColorStrArray = foreColorStr.Split(',');
                        control.ForeColor = Color.FromArgb(Convert.ToInt32(backColorStrArray[0]),
                            Convert.ToInt32(backColorStrArray[1]),
                            Convert.ToInt32(backColorStrArray[2]));
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetControlForeColor Error: " + ex.Message);
            }
        }

        public static void SetControlFont(Control control, string styleText)
        {
            try
            {
                var fontStr = styleText.GetStyleValue("Font");
                if (string.IsNullOrEmpty(fontStr))
                {
                    fontStr = "Font:Microsoft Sans Serif";
                }

                float fontSize = 8;
                var fontSizeStr = styleText.GetStyleValue("FontSize");
                if (string.IsNullOrEmpty(fontSizeStr)) fontSize = control.Font.Size;
                else fontSize = Convert.ToSingle(fontSizeStr, CultureInfo.InvariantCulture);

                var fontStyleStrs = styleText.GetStyleValue("FontStyle");
                var fontStyle = new FontStyle();
                if (!string.IsNullOrEmpty(fontStyleStrs))
                {
                    var fontStyleStrArry = fontStyleStrs.Split(',');

                    foreach (var fontStyleStr in fontStyleStrArry)
                    {
                        if (fontStyleStr.ToLower() == "bold")
                        {
                            fontStyle = fontStyle | FontStyle.Bold;
                        }
                        else if (fontStyleStr.ToLower() == "italic")
                        {
                            fontStyle = fontStyle | FontStyle.Italic;
                        }
                        else if (fontStyleStr.ToLower() == "underline")
                        {
                            fontStyle = fontStyle | FontStyle.Underline;
                        }
                        else if (fontStyleStr.ToLower() == "strikeout")
                        {
                            fontStyle = fontStyle | FontStyle.Strikeout;
                        }
                    }

                }
                else
                {
                    fontStyle = control.Font.Style;
                    //control.Font = new Font(fontStr, fontSize);
                }
                control.Font = new Font(fontStr, fontSize, fontStyle);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetControlFont Error: " + ex.Message);
            }
        }

        public static void SetControlBackgroundImage(Control control, string imgUrl)
        {
            try
            {
                if (!string.IsNullOrEmpty(imgUrl) && System.IO.File.Exists(imgUrl))
                {
                    control.BackgroundImage = ControlHelper.GetImage(imgUrl);
                    control.BackgroundImageLayout = ImageLayout.Zoom;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n >> " + TypeName + ".SetControlBackgroundImage Error: " + ex.Message);
            }
        }




        //##get
        public static Control GetControlByNameByParent(string name, Control parent)
        {
            if (parent == null)
            {
                throw new ArgumentException("parent can't be null! ");
            }

            foreach (Control Item in parent.Controls)
            {
                if (Item.Name == name)
                {
                    return Item;
                }
                else
                {
                    var ctrl = GetControlByNameByParentForRecursionSubCall(name, Item);
                    if (ctrl != null)
                    {
                        return ctrl;
                    }
                }
            }

            throw new ArgumentException("\n>>  " + TypeName + ".GetControlByNameByContainer Error: GetControlByNameByParent Error; name,parent=" + name + "," + parent.Name + ", No Control Found");
        }

        public static Control GetControlByNameByParentForRecursionSubCall(string name, Control parent)
        {
            try
            {
                foreach (Control ctrl in parent.Controls)
                {
                    if (ctrl.Name == name)
                    {
                        return ctrl;
                    }
                    else
                    {
                        var subCtrl = GetControlByNameByParentForRecursionSubCall(name, ctrl);
                        if (subCtrl != null)
                        {
                            return subCtrl;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>>  " + TypeName + ".GetControlByNameByParentForRecursion Error: Name=" + name + ex.Message);
            }
        }

        public static Control GetControlByNameByContainer(Control container, string ctrlName)
        {
            try
            {
                var ctrl = container.Controls.Find(ctrlName, true)[0];
                return ctrl;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>>  " + TypeName + ".GetControlByNameByContainer Error: Control does not exist! ctrlName=" + ctrlName + "; container=" + container);
            }
        }


        //###common
        public static Image GetImage(string imgUrl)
        {
            try
            {
                if (!string.IsNullOrEmpty(imgUrl) && System.IO.File.Exists(imgUrl))
                {
                    Stream strm = File.Open(imgUrl, FileMode.Open, FileAccess.Read, FileShare.Read);
                    var image = Image.FromStream(strm);
                    strm.Close();
                    return image;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



    }
}