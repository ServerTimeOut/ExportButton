using Godot;

[Tool] // ExportButton must have the [Tool] attribute to work in the editor.
public partial class Example : Node
{
    [ExportCategory("Character Stats")]
    [Export] public int Health = 100;
    [Export] public int Mana = 50;
    [Export] public int Stamina = 75;

    [ExportGroup("Inventory")]
    [Export] public int Gold = 1000;
    [Export] public int PotionCount = 5;

    
    
    // Basic ExportButton usage, adds a button at the top of the inspector.
    [ExportButton("Print Debug Info")]
    private void OnPrintDebugInfoBtnPressed() => GD.Print($"Health: {Health}, Mana: {Mana}, Stamina: {Stamina}, Gold: {Gold}, Potions: {PotionCount}");
    
    
    
    // ExportButton with a specific location: this button appears in the "Character Stats" category.
    [ExportButton("Heal Character", InspectorLocationType.Category, "Character Stats")]
    private void OnHealCharacterBtnPressed() => Health = Mana = Stamina = 100;
    
   

    // ExportButton placed under the "Character Stats" category using ExportButtonLocationCategory attribute.
    [ExportButton("Cast Spell")]
    [InspectorLocationCategory("Character Stats")]
    private void OnCastSpellBtnPressed()
    {
        if (Mana >= 10)
            GD.Print("Cast a spell! Mana left: ", Mana -= 10);
        else
            GD.Print("Not enough mana to cast a spell!");
    }
    
    
    // ExportButton linked to a specific property: this button appears above "Mana" in the inspector.
    [ExportButton("Drink Potion", InspectorLocationType.Property, nameof(Mana))]
    private void OnDrinkPotionBtnPressed()
    {
        if (PotionCount-- > 0)
        {
            Health = Mathf.Min(Health + 20, 100);
            GD.Print("Drank a potion! Health: ", Health, ", Potions left: ", PotionCount);
        }
        else
            GD.Print("No potions left!");
    }
    
    
    // ExportButton appears under the "Inventory" group in the inspector.
    [ExportButton("Add Gold", InspectorLocationType.Group, "Inventory")]
    private void OnAddGoldBtnPressed() => Gold += 50;
    


    
    // Appears at the very end of the inspector.
    [ExportButton("Reset Stats", InspectorLocationType.Footer)]
    private void OnResetStatsBtnPressed()
    {
        Health = 100;
        Mana = 50;
        Stamina = 75;
        Gold = 1000;
        PotionCount = 5;
    }
    

    public override void _Ready()
    {
        if (Engine.IsEditorHint())
            return; // Exit early if running in the editor
    }
}
