using System;

[AttributeUsage(AttributeTargets.Method)]
public class InspectorLocationAttribute : BaseCustomExportAttribute
{
    protected InspectorLocationAttribute(InspectorLocationType location, string locationName = null)
    {
        AddLocation(new InspectorLocation(location, locationName));
    }
}

public class InspectorLocationHeaderAttribute : InspectorLocationAttribute
{
    public InspectorLocationHeaderAttribute() : base(InspectorLocationType.Header) { }
}

public class InspectorLocationCategoryAttribute : InspectorLocationAttribute
{
    public InspectorLocationCategoryAttribute(string name) : base(InspectorLocationType.Category, name) { }
}

public class InspectorLocationGroupAttribute : InspectorLocationAttribute
{
    public InspectorLocationGroupAttribute(string name) : base(InspectorLocationType.Group, name) { }
}

public class InspectorLocationPropertyAttribute : InspectorLocationAttribute
{
    public InspectorLocationPropertyAttribute(string name) : base(InspectorLocationType.Property, name) { }
}


public class InspectorLocationLocationEndAttribute : InspectorLocationAttribute
{
    public InspectorLocationLocationEndAttribute() : base(InspectorLocationType.Footer) { }
}