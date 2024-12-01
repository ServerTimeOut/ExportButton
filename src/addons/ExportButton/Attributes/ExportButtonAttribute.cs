using System;
using Godot;

[AttributeUsage(AttributeTargets.Method)]
public class ExportButtonAttribute : Attribute
{
    public string Text { get; } = null;
    public int? TextSize { get; } = null;
    
    public InspectorLocation LocationType { get; private set; } = InspectorLocation.Header;
    public string LocationName { get; private set; }



    public ExportButtonAttribute(string text)
    {
        Text = text;
    }
    public ExportButtonAttribute(string text, InspectorLocation location, string locationName = null)
    {
        Text = text;
        LocationType = location;
        LocationName = locationName;

        if (LocationType is InspectorLocation.Header or InspectorLocation.Footer && LocationName != null)
        {
            GD.PushWarning($"ExportButtonAttribute: [{Text}] Header and Footer locations do not require a LocationName.");
            LocationName = null;
        }
            
    }
    
    
    public void AddLocationAttribute(InspectorLocationAttribute locationAttribute)
    {
        LocationType = locationAttribute.LocationType;
        LocationName = locationAttribute.LocationName;
    }
}