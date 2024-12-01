using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;


public abstract partial class CustomInspectorBuilderPlugin<T> : EditorInspectorPlugin
{
	public record InspectorLocationData<T> (InspectorLocation Location, T LocationData);
	
	private readonly Dictionary<string, Type> _resPathCache = new();
	private readonly Dictionary<Type, InspectorLocationData<T>[]> _inspectorLocationsCache = new();
	
	private Type _currentType;

	public override bool _CanHandle(GodotObject @object)
	{
		if (@object == null)
		{
			_currentType = null;
			return false;
		}

		if (TryGetScriptType(@object, out _currentType))
		{
			if (!_inspectorLocationsCache.TryGetValue(_currentType, out InspectorLocationData<T>[] inspectorLocations))
			{
				//check if the class has the [Tool] attribute
				if (_currentType.GetCustomAttribute<ToolAttribute>() == null)
				{
					GD.PrintErr($"CustomInspectorBuilderPlugin: The class {_currentType.Name} must have the [Tool] attribute. ExportButton will not work.");
					_inspectorLocationsCache[_currentType] = Array.Empty<InspectorLocationData<T>>();
				}
				else
				{
					CacheInspectorLocations(@object, _currentType, out inspectorLocations);
					_inspectorLocationsCache[_currentType] = inspectorLocations;
				}
			}
			
			if (inspectorLocations.Length > 0) 
				return true;
			
		}
		
		_currentType = null;
		return false;		
	}
	
	public override void _ParseBegin(GodotObject @object) => TryAddControls(@object, InspectorLocation.Header);
	public override void _ParseCategory(GodotObject @object, string category) => TryAddControls(@object, new InspectorLocation(InspectorLocationType.Category, category));
	public override void _ParseGroup(GodotObject @object, string group) => TryAddControls(@object,  new InspectorLocation(InspectorLocationType.Group, group));
	public override void _ParseEnd(GodotObject @object) => TryAddControls(@object,  InspectorLocation.Footer);
	public override bool _ParseProperty(GodotObject @object, Variant.Type type, string name, PropertyHint hintType, string hintString,
		PropertyUsageFlags usageFlags, bool wide)
	{
		TryAddControls(@object,  new InspectorLocation(InspectorLocationType.Property, name));
		return base._ParseProperty(@object, type, name, hintType, hintString, usageFlags, wide);
	}

	private bool TryGetScriptType(GodotObject obj, out Type type)
	{
		type = null;
		
		CSharpScript csScript = obj.GetScript().As<CSharpScript>();
		if (csScript == null) 
			return false;

	
		if (!_resPathCache.TryGetValue(csScript.ResourcePath, out type))
		{
			GodotObject tempObject = csScript.New().AsGodotObject();
			type = tempObject.GetType();

			tempObject.Dispose();
				
			
			_resPathCache[csScript.ResourcePath] = type;
		}
		
		return type != null;
	}
	
	
	private void TryAddControls(GodotObject @object, InspectorLocation currentLocation)
	{
		if (@object == null || _currentType == null) return;
		
		if (!_inspectorLocationsCache.TryGetValue(_currentType, out InspectorLocationData<T>[] inspectorLocations) || 
		    inspectorLocations.Length == 0) return;

		foreach (var locationData in inspectorLocations)
		{
			if (locationData.Location.Equals(currentLocation))
			{
				AddControl(@object, _currentType, locationData);
			}
		}
	}

	protected abstract void CacheInspectorLocations(GodotObject godotObject, Type currentType, out InspectorLocationData<T>[] inspectorLocations);
	protected abstract void AddControl(GodotObject @object, Type currentType, InspectorLocationData<T> currentLocationData);
}