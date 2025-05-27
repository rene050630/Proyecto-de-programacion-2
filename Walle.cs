using System.Diagnostics;

public class Walle
{
    public int ActualX;
    public int ActualY;
    public Walle(int ActualX, int ActualY)
    {
        this.ActualX = ActualX;
        this.ActualY = ActualY;
    }
    public void MoveTo(int x, int y)
    {
        ActualX = x;
        ActualY = y;
    }
}