using System;
using System.Collections.Generic;
using System.Linq;

namespace SemanticXamlPrint.Parser.Components
{
    public class GridRowComponent : XamlComponentCommonProperties, IXamlComponent
    {
        public string Name => Type.Name;
        public Type Type => this.GetType();
        public List<IXamlComponent> Children { get; private set; } = new List<IXamlComponent>();
        public bool TrySetProperty(string propertyName, string value)
        {
            try
            {
                if (base.SetCommonProperties(propertyName, value)) return true;
                switch (propertyName)
                {
                    default:
                        CustomProperties.AddCustomProperty(propertyName, value);
                        break;
                }
                return true;
            }
            catch { return false; }
        }
        public void AddChild(IXamlComponent child)
        {
            Children.Add(child);
        }
        public IEnumerable<string> ReferencedFontFamilies
        {
            get
            {
                if (Font != null) yield return Font;
                if (Children == null || Children.Count == 0) yield break;

                foreach (var font in Children.SelectMany(child => child.ReferencedFontFamilies))
                {
                    yield return font;
                }
            }
        }
    }
}
