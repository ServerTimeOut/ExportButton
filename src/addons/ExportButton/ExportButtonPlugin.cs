#if TOOLS
using Godot;
using System;

[Tool]
public partial class ExportButtonPlugin : EditorPlugin
{
	private ExportButtonInspectorPlugin _exportButtonInspectorPlugin;
	public override void _EnterTree()
	{
		_exportButtonInspectorPlugin = new ExportButtonInspectorPlugin();
		AddInspectorPlugin(_exportButtonInspectorPlugin);
	}

	public override void _ExitTree()
	{
		RemoveInspectorPlugin(_exportButtonInspectorPlugin);
	}
}
#endif