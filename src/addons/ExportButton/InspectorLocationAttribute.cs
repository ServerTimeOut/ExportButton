using System;

[AttributeUsage(AttributeTargets.Method)]
public class InspectorLocationAttribute : Attribute
{
    public InspectorLocation LocationType { get; }
    public string LocationName { get; }
    
    
    public InspectorLocationAttribute(InspectorLocation location, string locationName = null)
    {
        LocationType = location;
        LocationName = locationName;
        
        if (LocationType == InspectorLocation.Begin || LocationType == InspectorLocation.End) LocationName = null;
    }
}




public class InspectorLocationBeginAttribute : InspectorLocationAttribute
{
    public InspectorLocationBeginAttribute() : base(InspectorLocation.Begin) { }
}

public class InspectorLocationCategoryAttribute : InspectorLocationAttribute
{
    public InspectorLocationCategoryAttribute(string name) : base(InspectorLocation.Category, name) { }
}

public class InspectorLocationGroupAttribute : InspectorLocationAttribute
{
    public InspectorLocationGroupAttribute(string name) : base(InspectorLocation.Group, name) { }
}

public class InspectorLocationLocationEndAttribute : InspectorLocationAttribute
{
    public InspectorLocationLocationEndAttribute() : base(InspectorLocation.End) { }
}


[AttributeUsage(AttributeTargets.Method)]
public class ButtonPropertyNameAttribute : Attribute
{
    public string Name { get; }
    
    public ButtonPropertyNameAttribute(string name)
    {
        Name = name;
    }
}