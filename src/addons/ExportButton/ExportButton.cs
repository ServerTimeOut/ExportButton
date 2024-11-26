using System.Reflection;
using Godot;

public partial class ExportButton : Button
{
    public static ExportButton CreateStaticMethodButton(MethodInfo methodInfo, string text) 
        => new ExportButton(methodInfo, text);
    
    public static ExportButton CreateInstanceMethodButton(GodotObject target, string methodName, string text)
        => new ExportButton(target, methodName, text);
    
    
    private bool IsStatic { get; set; }
    private MethodInfo MethodInfo { get; set; }
    private GodotObject Target { get; set; }
    private string FunctionName { get; set; }
    
    
    private ExportButton()
    {
    }
    
    private ExportButton(MethodInfo methodInfo, string text)
    {
        IsStatic = true;
        MethodInfo = methodInfo;
        Text = text;
    }

    private ExportButton(GodotObject target, string methodName, string text)
    {
        IsStatic = false;
        Target = target;
        FunctionName = methodName;
        Text = text;
    }

    public override void _EnterTree()
    {
        if (IsStatic)
            Connect(BaseButton.SignalName.Pressed, new Callable(this, MethodName.InvokeStaticMethod));
        else
            Connect(BaseButton.SignalName.Pressed, new Callable(Target, FunctionName));
    }
    
    public override void _ExitTree()
    {
        if (IsStatic)
            Disconnect(BaseButton.SignalName.Pressed, new Callable(this, MethodName.InvokeStaticMethod));
        else
            Disconnect(BaseButton.SignalName.Pressed, new Callable(Target, FunctionName));
    }
    
    private void InvokeStaticMethod()
    {
        MethodInfo.Invoke(null, null);
    }
}