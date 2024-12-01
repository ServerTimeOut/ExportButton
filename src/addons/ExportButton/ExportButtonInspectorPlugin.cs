using Godot;

using System;
using System.Collections.Generic;
using System.Reflection;


public partial class ExportButtonInspectorPlugin : CustomInspectorBuilderPlugin<ExportButtonInspectorPlugin.ExportButtonData>
{
	public record ExportButtonData(ExportTextInfo ButtonInfo, MethodInfo MethodInfo)
	{
		public readonly ExportTextInfo ButtonInfo = ButtonInfo;
		public readonly MethodInfo MethodInfo = MethodInfo;
	}
	
	protected override void CacheInspectorLocations(GodotObject godotObject, Type currentType, out InspectorLocationData<ExportButtonData>[] inspectorLocations)
	{
		List<InspectorLocationData<ExportButtonData>> locationDatas = new();
		foreach (MethodInfo methodInfo in currentType.GetMethods(BindingFlags.Default | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static))
		{
			ExportButtonAttribute attribute = methodInfo.GetCustomAttribute<ExportButtonAttribute>();
			if (attribute == null) continue;
			
			//has to be a void method with no parameters
			if (methodInfo.GetParameters().Length > 0)
			{
				GD.PrintErr($"ExportButton: The method {methodInfo.Name} in {currentType.Name} must be parameterless.");
				continue;
			}
			
			
			InspectorLocationAttribute locationAttribute = methodInfo.GetCustomAttribute<InspectorLocationAttribute>();
			if (locationAttribute != null) attribute.AddLocation(locationAttribute);

			
			//check if the location is correct
			if (attribute.Location.Type == InspectorLocationType.Category && string.IsNullOrEmpty(attribute.Location.Name))
			{
				GD.PrintErr($"ExportButton: The method {methodInfo.Name} in {currentType.Name} has a category location type but no category name.");
				continue;
			}
			if (attribute.Location.Type == InspectorLocationType.Group && string.IsNullOrEmpty(attribute.Location.Name))
			{
				GD.PrintErr($"ExportButton: The method {methodInfo.Name} in {currentType.Name} has a group location type but no group name.");
				continue;
			}
			
			locationDatas.Add(new InspectorLocationData<ExportButtonData>(attribute.Location, new ExportButtonData(attribute.Info, methodInfo)));
		}
		
		inspectorLocations = locationDatas.ToArray();
	}
	
	protected override void AddControl(GodotObject @object, Type currentType, InspectorLocationData<ExportButtonData> currentLocationData)
	{
		MethodInfo methodInfo = currentLocationData.LocationData.MethodInfo;

		ExportButton button = methodInfo.IsStatic
			? ExportButton.CreateStaticMethodButton(methodInfo, currentLocationData.Location.Name)
			: ExportButton.CreateInstanceMethodButton(@object, methodInfo.Name, currentLocationData.LocationData.ButtonInfo.Text);
		if (currentLocationData.LocationData.ButtonInfo.TextSize != null)
			button.AddThemeFontSizeOverride("font_size", currentLocationData.LocationData.ButtonInfo.TextSize!.Value);

		MarginContainer marginContainer = new MarginContainer();
		marginContainer.AddThemeConstantOverride("margin_left", 4);
		marginContainer.AddThemeConstantOverride("margin_right", 4);
		marginContainer.AddThemeConstantOverride("margin_top", 4);
		marginContainer.AddThemeConstantOverride("margin_bottom", 4);
		marginContainer.AddChild(button);

		AddCustomControl(marginContainer);
	}
}