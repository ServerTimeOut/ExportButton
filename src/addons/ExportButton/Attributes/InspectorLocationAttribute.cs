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
        
        if (LocationType == InspectorLocation.Header || LocationType == InspectorLocation.Footer) LocationName = null;
    }
}




public class InspectorLocationHeaderAttribute : InspectorLocationAttribute
{
    public InspectorLocationHeaderAttribute() : base(InspectorLocation.Header) { }
}

public class InspectorLocationCategoryAttribute : InspectorLocationAttribute
{
    public InspectorLocationCategoryAttribute(string name) : base(InspectorLocation.Category, name) { }
}

public class InspectorLocationGroupAttribute : InspectorLocationAttribute
{
    public InspectorLocationGroupAttribute(string name) : base(InspectorLocation.Group, name) { }
}

public class InspectorLocationPropertyAttribute : InspectorLocationAttribute
{
    public InspectorLocationPropertyAttribute(string name) : base(InspectorLocation.Property, name) { }
}


public class InspectorLocationLocationEndAttribute : InspectorLocationAttribute
{
    public InspectorLocationLocationEndAttribute() : base(InspectorLocation.Footer) { }
}