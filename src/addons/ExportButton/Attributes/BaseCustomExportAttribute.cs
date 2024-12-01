using System;
using Godot;

public abstract class BaseCustomExportAttribute : Attribute
{
    public InspectorLocation Location { get; protected set; } = InspectorLocation.Header;
    

    
    public void AddLocation(InspectorLocationAttribute locationAttribute) => AddLocation(locationAttribute.Location);
    protected void AddLocation(InspectorLocation location) => Location = location;
    protected void AddLocation(InspectorLocationType locationType, string locationName = null) 
        => Location = new InspectorLocation(locationType, locationName);
    
}