using System;
using Godot;
using System.Collections.Generic;
using System.Reflection;
using Godot.Collections;
using Array = System.Array;

public partial class ExportButtonInspectorPlugin : EditorInspectorPlugin
{
	private static readonly System.Collections.Generic.Dictionary<string, Type> ResPathCache = new();
	private static readonly System.Collections.Generic.Dictionary<Type, ExportButtonMethodData[]> MethodCache = new();
	
	private Type _currentType;
	string _previousProperty = null;
	
	public override bool _CanHandle(GodotObject @object)
	{
		if (@object == null)
		{
			_currentType = null;
			return false;
		}
		
		
		if (TryGetScriptType(@object, out _currentType))
		{
			CacheExportButtonData(_currentType);
			return MethodCache[_currentType].Length > 0;
		}
		
		_currentType = null;
		return false;		
	}
	

	public override void _ParseBegin(GodotObject @object) => AddExportButtons(@object, InspectorLocation.Header);
	public override void _ParseCategory(GodotObject @object, string category) => AddExportButtons(@object, InspectorLocation.Category, category);
	public override void _ParseGroup(GodotObject @object, string group) => AddExportButtons(@object, InspectorLocation.Group, group);
	public override void _ParseEnd(GodotObject @object) => AddExportButtons(@object, InspectorLocation.Footer);
	

	
	
	public override bool _ParseProperty(GodotObject @object, Variant.Type type, string name, PropertyHint hintType, string hintString,
		PropertyUsageFlags usageFlags, bool wide)
	{
		AddExportButtons(@object, InspectorLocation.Property, name);
		return base._ParseProperty(@object, type, name, hintType, hintString, usageFlags, wide);
	}



	private bool TryGetScriptType(GodotObject obj, out Type type)
	{
		type = null;
		
		CSharpScript csScript = obj.GetScript().As<CSharpScript>();
		if (csScript == null) 
			return false;
		

		if (!ResPathCache.TryGetValue(csScript.ResourcePath, out type))
		{
			GodotObject tempObject = csScript.New().AsGodotObject();
			type = tempObject.GetType();

			tempObject.Dispose();
				
			
			ResPathCache[csScript.ResourcePath] = type;
		}
		
		return type != null;
	}
	
	
	private void CacheExportButtonData(Type type)
	{
		if (MethodCache.ContainsKey(type)) return;
		
		if (type.GetCustomAttribute<ToolAttribute>() == null)
		{
			GD.PrintErr($"ExportButton: The class {type.Name} must have the [Tool] attribute. ExportButton will not work.");
			MethodCache[type] = Array.Empty<ExportButtonMethodData>();
			return;
		}
		
		
		List<ExportButtonMethodData> methods = new();
		foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Default | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static))
		{
			ExportButtonAttribute attribute = methodInfo.GetCustomAttribute<ExportButtonAttribute>();
			if (attribute == null) continue;
			
			//has to be a void method with no parameters
			if (methodInfo.GetParameters().Length > 0)
			{
				GD.PrintErr($"ExportButton: The method {methodInfo.Name} in {type.Name} must be parameterless.");
				continue;
			}
			
			
			InspectorLocationAttribute locationAttribute = methodInfo.GetCustomAttribute<InspectorLocationAttribute>();
			if (locationAttribute != null) attribute.AddLocationAttribute(locationAttribute);

			
			//check if the location is correct
			if (attribute.LocationType == InspectorLocation.Category && string.IsNullOrEmpty(attribute.LocationName))
			{
				GD.PrintErr($"ExportButton: The method {methodInfo.Name} in {type.Name} has a category location type but no category name.");
				continue;
			}
			if (attribute.LocationType == InspectorLocation.Group && string.IsNullOrEmpty(attribute.LocationName))
			{
				GD.PrintErr($"ExportButton: The method {methodInfo.Name} in {type.Name} has a group location type but no group name.");
				continue;
			}
			
			
			
			methods.Add(new ExportButtonMethodData(attribute, methodInfo));
		}
		
		MethodCache[type] = methods.ToArray();
	}
	

	private void AddExportButtons(GodotObject @object, InspectorLocation locationType, string locationName = null)
	{
		if (@object == null || _currentType == null) return;
		if (!MethodCache.TryGetValue(_currentType, out ExportButtonMethodData[] exportButtonDatas) ||
		    exportButtonDatas.Length == 0) return;

		foreach (ExportButtonMethodData data in MethodCache[_currentType])
		{
			// Skip if the category doesn't match
			if (data.Attribute.LocationType != locationType || 
			    data.Attribute.LocationName != locationName) continue;
			
			
			ExportButton button = data.MethodInfo.IsStatic ? 
				ExportButton.CreateStaticMethodButton(data.MethodInfo, data.Attribute.Text) : 
				ExportButton.CreateInstanceMethodButton(@object, data.MethodInfo.Name, data.Attribute.Text);
			if (data.Attribute.TextSize != null) button.AddThemeFontSizeOverride("font_size", data.Attribute.TextSize!.Value);
			
			MarginContainer marginContainer = new MarginContainer();
			marginContainer.AddThemeConstantOverride("margin_left", 4);
			marginContainer.AddThemeConstantOverride("margin_right", 4);
			marginContainer.AddThemeConstantOverride("margin_top", 4);
			marginContainer.AddThemeConstantOverride("margin_bottom", 4);
			marginContainer.AddChild(button);

			AddCustomControl(marginContainer);
		}
	}

	
	private struct ExportButtonMethodData
	{
		public readonly ExportButtonAttribute Attribute;
		public readonly MethodInfo MethodInfo;
		
		public ExportButtonMethodData(ExportButtonAttribute attribute, MethodInfo methodInfo)
		{
			Attribute = attribute;
			MethodInfo = methodInfo;
		}
	}
}