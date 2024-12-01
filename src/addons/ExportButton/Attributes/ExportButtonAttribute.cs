using System;
using Godot;

[AttributeUsage(AttributeTargets.Method)]
public class ExportButtonAttribute : BaseCustomExportAttribute
{
    public ExportTextInfo Info { get; private set; }
    

    public ExportButtonAttribute(string text)
    {
        Info =new (text);
    }
    public ExportButtonAttribute(string text, InspectorLocationType location)
    {
        Info =new (text);
        AddLocation(location);
    }
    
    public ExportButtonAttribute(string text, InspectorLocationType locationType, String locationName)
    {
        Info =new (text);
        AddLocation(locationType, locationName);
    }
}