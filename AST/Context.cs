public class Context 
{
    public (int, int) Canvas { get; set; } // Estado del canvas
    public List<string> ValidColors { get; } = new List<string> { "Red", "Blue", "Green", "Yellow", "Orange", "Purple", "Black", "White", "Transparent"};
    
    public bool IsValidColor(string color) => ValidColors.Contains(color);
}