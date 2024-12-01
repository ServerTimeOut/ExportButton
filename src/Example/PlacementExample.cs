using Godot;

[Tool] // ExportButton must have the [Tool] attribute to work in the editor.
public partial class PlacementExample : Node
{
    
    // Create a new category in the inspector.
    [ExportCategory("My Category")]
    [Export] public int MyProperty = 0;
    
    // Add button to category
    [ExportButton("My Button", InspectorLocation.Category, "My Category") ]
    public void OnMyButtonPressed() => GD.Print($"Button clicked {MyProperty++} times!");
    
    //Or 
    
    // Add button to category using InspectorLocationCategory attribute
    [ExportButton("My Button 2") ]
    [InspectorLocationCategory("My Category")]
    public void OnMyButton2Pressed() => GD.Print($"Button clicked {MyProperty++} times!");
    
    
    [ExportButton("Set My Property to 0")]
    [InspectorLocationProperty(nameof(MyProperty))]
    public void OnSetMyPropertyTo0Pressed() => MyProperty = 0;
}