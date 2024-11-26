using System;

[AttributeUsage(AttributeTargets.Method)]
public class ExportButtonAttribute : Attribute
{
    public string Text { get; }
    public InspectorLocation LocationType { get; private set; }
    public string LocationName { get; private set; }
    
    public string ButtonPropertyName { get; private set; } = null;
    
    public ExportButtonAttribute(string text) : this(text, InspectorLocation.Begin) { }
    public ExportButtonAttribute(string text, InspectorLocation location, string locationName = null)
    {
        Text = text;
        LocationType = location;
        LocationName = locationName;
        
        
        if (LocationType == InspectorLocation.Begin || LocationType == InspectorLocation.End)
            LocationName = null;
    }
    
    
    public void AddLocationAttribute(InspectorLocationAttribute locationAttribute)
    {
        LocationType = locationAttribute.LocationType;
        LocationName = locationAttribute.LocationName;
    }
    
    public void AddPropertyNameAttribute(ButtonPropertyNameAttribute buttonPropertyName)
    {
        ButtonPropertyName = buttonPropertyName.Name;
    }
    
    
}