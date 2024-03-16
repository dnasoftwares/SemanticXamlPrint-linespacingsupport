using System;
using System.Collections.Generic;

namespace SemanticXamlPrint.Parser.Components
{
    public abstract class XamlComponentCommonProperties
    {
        public string Font { get; set; } = null;
        public string FontStyle { get; set; } = null;
        public float FontSize { get; set; } = 0;
        public int LineHeight { get; set; }=Int32.MinValue;
        public string Align { get; set; } = null;
        public string Color { get; set; } = null;
        public List<XamlComponentCustomProperty> CustomProperties { get; private set; } = new List<XamlComponentCustomProperty>();
        public bool SetCommonProperties(string propertyName, string value)
        {
            try
            {
                switch (propertyName)
                {
                    case "font":
                        Font = value;
                        break;
                    case "fontstyle":
                        FontStyle = value;
                        break;
                    case "fontweight":
                        FontStyle = value;
                        break;
                    case "fontsize":
                        FontSize = float.TryParse(value, out float fontSize) ? fontSize : 0;
                        break;
                    case "align":
                        Align = value;
                        break;
                    case "textalign":
                        Align = value;
                        break;
                    case "foreground":
                        Color = value;
                        break;
                    case "color":
                        Color = value;
                        break;
                    case "lineheight":
                        LineHeight= int.TryParse(value, out int lineHeight) ? lineHeight : 0;
                        break;
                    default:
                        return false;
                }
                return true;
            }
            catch { return false; }
        }
    }
    public class XamlComponentCustomProperty
    {
        public XamlComponentCustomProperty(string key, string value)
        {
            Key = key;
            Value = value;
        }
        public string Key { get; }
        public string Value { get; }
    }
}
