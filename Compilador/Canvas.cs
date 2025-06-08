using System.Diagnostics;
using System.Drawing;
public enum Colors{ Red, Blue, Green, Yellow, Orange, Purple, Black, White, Transparent }
public class Canvas
{
    public string[,] board;
    public int ActualX;
    public int ActualY;
    public string BrushColor = "Transparent";
    public string BoardColor;
    public int BrushSize = 1;
    public int Size;
    public bool IsSpawnCalled = false;
    public Walle Walle;
    public Canvas(int size)
    {
        Size = size;
        board = new string[size, size];
        Clear();
    }
    public void Clear()
    {
        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                board[i, j] = "White";
    }
    public void MoveTo(int x, int y)
    {
        ActualX = x;
        ActualY = y;
    }
    public void IsCanvasColor(string color, int vertical, int horizontal)
    {
        if (IsPositionValid(ActualX + horizontal, ActualY + vertical))
        {
            if (GetPixel(ActualX + horizontal, ActualY + vertical) == color) System.Console.WriteLine(1);
            System.Console.WriteLine(0);
        }
        else IsCanvasColor();
    }
    public bool IsCanvasColor()
    {
        return false;
    }
    public int IsBrushSize(int size)
    {
        if (BrushSize == size) return 1;
        else return 0;
    }
    public int IsBrushColor(string color)
    {
        if (BrushColor == color) return 1;
        else return 0;
    }
    public int GetCanvasSize()
    {
        return Size;
    }
    public int GetActualX()
    {
        return ActualX;
    }
    public int GetActualY()
    {
        return ActualX;
    }
    public string GetPixel(int x, int y) => board[x, y];
    public void Spawn(int x, int y)
    {
        // Lógica para posicionar a Wall-E
        if (IsPositionValid(x, y))
        {
            ActualX = x;
            ActualY = y;
        }
    }

    public void DrawLine(int dirX, int dirY, int distance)
    {
        int radius = (BrushSize - 1) / 2;

        for (int step = 0; step < distance; step++)
        {
            // Calcular nueva posición
            int newX = ActualX + dirX;
            int newY = ActualY + dirY;

            // Detener si se sale del canvas
            if (!IsPositionValid(newX, newY)) break;

            ActualX = newX;
            ActualY = newY;

            // Pintar área del pincel
            for (int dx = -radius; dx <= radius; dx++)
                for (int dy = -radius; dy <= radius; dy++)
                    SetPixel(ActualX + dx, ActualY + dy);
        }
    }

    private void SetPixel(int x, int y)
    {
        if (IsPositionValid(x, y) && BoardColor != "Transparent")
            board[x, y] = BoardColor;
    }
    private bool IsPositionValid(int x, int y)
        => x >= 0 && x < Size && y >= 0 && y < Size;
    public void DrawMidpointCircle(int cx, int cy, int radius)
    {
        if (BoardColor == "Transparent") return;

        int x = radius;
        int y = 0;
        int decisionOver2 = 1 - x;
        int halfBrush = BrushSize / 2;

        while (x >= y)
        {
            PaintOctants(cx, cy, x, y, halfBrush);

            y++;
            if (decisionOver2 <= 0)
                decisionOver2 += 2 * y + 1;
            else
            {
                x--;
                decisionOver2 += 2 * (y - x) + 1;
            }
        }
    }

    private void PaintOctants(int cx, int cy, int x, int y, int halfBrush)
    {
        PaintWithBrush(cx + x, cy + y, halfBrush);
        PaintWithBrush(cx + y, cy + x, halfBrush);
        PaintWithBrush(cx - y, cy + x, halfBrush);
        PaintWithBrush(cx - x, cy + y, halfBrush);
        PaintWithBrush(cx - x, cy - y, halfBrush);
        PaintWithBrush(cx - y, cy - x, halfBrush);
        PaintWithBrush(cx + y, cy - x, halfBrush);
        PaintWithBrush(cx + x, cy - y, halfBrush);
    }

    private void PaintWithBrush(int x, int y, int halfBrush)
    {
        for (int dx = -halfBrush; dx <= halfBrush; dx++)
        {
            for (int dy = -halfBrush; dy <= halfBrush; dy++)
            {
                int px = x + dx;
                int py = y + dy;
                if (IsWithinCanvas(px, py, Size))
                    SetPixel(px, py);
            }
        }
    }
    public void DrawRectangleOutline(int cx, int cy, int w, int h)
    {
        // Calcular límites del rectángulo
        int left = cx - w / 2;
        int right = cx + (w - 1) / 2;
        int top = cy - h / 2;
        int bottom = cy + (h - 1) / 2;
        int halfBrush = BrushSize / 2;

        // Dibujar bordes horizontales
        for (int x = left; x <= right; x++)
        {
            PaintWithBrush(x, top, halfBrush);
            PaintWithBrush(x, bottom, halfBrush);
        }

        // Dibujar bordes verticales (excluyendo esquinas)
        for (int y = top + 1; y < bottom; y++)
        {
            PaintWithBrush(left, y, halfBrush);
            PaintWithBrush(right, y, halfBrush);
        }
    }
    public bool IsWithinCanvas(int x, int y, int canvasSize)
    {
        return x >= 0 && x < canvasSize && y >= 0 && y < canvasSize;
    }
    public int GetColorCount(int x1, int x2, int y1, int y2, string color)
    {
        int count = 0;
        if (x1 > Size || y1 > Size || x2 > Size || y2 > Size) return 0;
        if (x1 < x2) (x1, x2) = (x2, x1);
        if (y1 > y2) (y1, y2) = (y2, y1);
        for (int i = x1; i <= x2; i++)
        {
            for (int j = y1; j <= y2; j++)
            {
                if (GetPixel(i, j) == color) count++;
            }
        }
        return count;
    }
    public void FillSpace(int startX, int startY, string targetColor)
    {
        bool[,] visited = new bool[Size, Size];
        Queue<(int x, int y)> queue = new Queue<(int x, int y)>();

        queue.Enqueue((startX, startY));
        visited[startX, startY] = true;

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();

            // 7. Pintar solo si sigue siendo el color objetivo
            if (GetPixel(x, y) == targetColor)
            {
                SetPixel(x, y);

                // 8. Explorar vecinos en 4 direcciones
                ExploreNeighbor(x + 1, y, queue, visited, targetColor);
                ExploreNeighbor(x - 1, y, queue, visited, targetColor);
                ExploreNeighbor(x, y + 1, queue, visited, targetColor);
                ExploreNeighbor(x, y - 1, queue, visited, targetColor);
            }
        }
    }
    private void ExploreNeighbor(int x, int y, Queue<(int x, int y)> queue, bool[,] visited, string targetColor)
    {
        // 9. Validar límites del canvas
        if (IsPositionValid(x, y))
            return;

        // 10. Evitar reprocesar celdas
        if (!visited[x, y] && GetPixel(x, y) == targetColor)
        {
            visited[x, y] = true;
            queue.Enqueue((x, y));
        }
    }
}