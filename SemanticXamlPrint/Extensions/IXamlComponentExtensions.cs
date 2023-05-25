﻿using SemanticXamlPrint.Components;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;

namespace SemanticXamlPrint
{
    internal static class IXamlComponentExtensions
    {
        public static IXamlComponent CreateComponentFromXml(this XmlNode node)
        {
            IXamlComponent component;
            switch (node.Name?.ToLower()?.Trim())
            {
                case "template":
                    component = new TemplateComponent();
                    break;
                case "image":
                    component = new ImageComponent();
                    break;
                case "grid":
                    component = new GridComponent();
                    break;
                case "gridrow":
                    component = new GridRowComponent();
                    break;
                case "cells":
                    component = new CellsComponent();
                    break;
                case "cell":
                    component = new CellComponent();
                    break;
                case "qrcode":
                    component = new QRCodeComponent();
                    break;
                case "data":
                    component = new DataComponent();
                    break;
                case "line":
                    component = new LineComponent();
                    break;
                case "linebreak":
                    component = new LineBreakComponent();
                    break;
                case "#text":
                    component = new TextBlockComponent(node.Value);
                    break;
                case "#comment":
                    return null;
                default:
                    throw new Exception($"Invalid node name {node.Name}");
            }
            // parse attributes
            if (node?.Attributes != null)
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    _ = component.TrySetProperty(attribute.Name?.ToLower()?.Trim(), attribute.Value);
                }
            // parse child nodes
            if (node?.ChildNodes != null)
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    IXamlComponent childObject = childNode.CreateComponentFromXml();
                    if (childObject != null)
                    {
                        if (childObject.Type == typeof(TemplateComponent)) throw new Exception($"{nameof(TemplateComponent)} can NOT be a child class");
                        component.AddChild(childObject);
                    }
                }
            return component;
        }

        public static ComponentDrawingFormatting GetSystemDrawingProperties(this IXamlComponent component, ComponentDrawingFormatting parentFormatting)
        {

            if (!component.Type.IsSubclassOf(typeof(XamlComponentCommonProperties))) return parentFormatting;
            XamlComponentCommonProperties styleFmt = (XamlComponentCommonProperties)component;
            //Return Custom
            return new ComponentDrawingFormatting
            {
                Font = new Font((string.IsNullOrEmpty(styleFmt.Font) ? parentFormatting.Font.Name : styleFmt.Font),
                                ((styleFmt.FontSize <= 0) ? parentFormatting.Font.Size : styleFmt.FontSize),
                                (string.IsNullOrEmpty(styleFmt.FontStyle) ? parentFormatting.Font.Style : GetOverridedFontStyle(styleFmt.FontStyle))),

                StringFormat = string.IsNullOrEmpty(styleFmt.Align) ? parentFormatting.StringFormat : GetConvertedStringFormat(styleFmt.Align),
                Brush = string.IsNullOrEmpty(styleFmt.Color) ? parentFormatting.Brush : GetSolidBrushFromColorString(styleFmt.Color)
            };
        }

        private static FontStyle GetOverridedFontStyle(string fontStyle)
        {
            switch (fontStyle?.Trim()?.ToLower())
            {
                case "bold":
                    return FontStyle.Bold;
                case "italic":
                    return FontStyle.Italic;
                case "underline":
                    return FontStyle.Underline;
                case "strikeout":
                    return FontStyle.Strikeout;
                default:
                    return FontStyle.Regular;
            }
        }
        private static StringFormat GetConvertedStringFormat(string alignment)
        {
            switch (alignment?.Trim()?.ToLower())
            {
                case "center":
                    return new StringFormat { Alignment = StringAlignment.Center };
                case "right":
                    return new StringFormat { Alignment = StringAlignment.Far };
                default:
                    return new StringFormat { Alignment = StringAlignment.Near };
            }
        }

        public static DashStyle ToDashStyle(this string style)
        {
            switch (style?.Trim()?.ToLower())
            {
                case "dash":
                    return DashStyle.Dash;
                case "dot":
                    return DashStyle.Dot;
                case "dashdot":
                    return DashStyle.DashDot;
                case "dashdotdot":
                    return DashStyle.DashDotDot;
                default:
                    return DashStyle.Solid;
            }
        }
        public static Brush GetSolidBrushFromColorString(string colorString)
        {
            if (string.IsNullOrEmpty(colorString)) return Brushes.Black;
            string colorCode = colorString.ToLower().Trim();
            //support html colors e.g. #B56E22
            if (colorCode.StartsWith("#") && colorCode.Length == 7) return GetHtmlColor(colorCode);
            switch (colorCode)
            {
                case "red":
                    return new SolidBrush(Color.Red);
                case "green":
                    return new SolidBrush(Color.Green);
                case "blue":
                    return new SolidBrush(Color.Blue);
                case "yellow":
                    return new SolidBrush(Color.Yellow);
                case "orange":
                    return new SolidBrush(Color.Orange);
                case "purple":
                    return new SolidBrush(Color.Purple);
                case "pink":
                    return new SolidBrush(Color.Pink);
                case "black":
                    return new SolidBrush(Color.Black);
                case "white":
                    return new SolidBrush(Color.White);
                case "gray":
                    return new SolidBrush(Color.Gray);
                case "brown":
                    return new SolidBrush(Color.Brown);
                case "cyan":
                    return new SolidBrush(Color.Cyan);
                case "magenta":
                    return new SolidBrush(Color.Magenta);
                // Add more cases for additional color names
                default:
                    return new SolidBrush(Color.Black);
            }
        }
        private static Brush GetHtmlColor(string colorString)
        {
            try { return new SolidBrush(ColorTranslator.FromHtml(colorString)); }
            catch { return new SolidBrush(Color.Black); }
        }
    }
    public class ComponentDrawingFormatting
    {
        public Font Font { get; set; }
        public StringFormat StringFormat { get; set; }
        public Brush Brush { get; set; }
    }
}
