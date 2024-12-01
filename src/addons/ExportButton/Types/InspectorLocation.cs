using Godot;

public readonly record struct InspectorLocation
{
    public InspectorLocationType Type { get; }
    public string Name { get; }
    
    public InspectorLocation(InspectorLocationType type, string name)
    {
        Type = type;
        Name = name;
        
        if (Type is InspectorLocationType.Header or InspectorLocationType.Footer && Name != null)
        {
            GD.PushWarning($"InspectorLocation: Header and Footer locations do not require a Name.");
            Name = null;
        }
    }
    
    private InspectorLocation(InspectorLocationType type) : this(type, null) { }
    
    public static InspectorLocation Header => new(InspectorLocationType.Header);
    public static InspectorLocation Footer => new(InspectorLocationType.Footer);
}