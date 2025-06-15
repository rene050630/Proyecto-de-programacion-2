using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace WindowsFormsApp1
{
    public enum Colors { Red, Blue, Green, Yellow, Orange, Purple, Black, White, Transparent }
    public class Canvas
    {
        public Colors[,] board;
        public int ActualX;
        public int ActualY;
        public Colors BrushColor = Colors.Yellow;
        public int BrushSize = 1;
        public int Size;
        public bool IsSpawnCalled = false;
     
        public Canvas() : this(25) { }
        public Canvas(int size)
        {
            Size = size;
            board = new Colors[size, size];
            Clear();
        }
        public void Clear()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    board[i, j] = Colors.Transparent;
        }
        public void Resize(int newSize)
        {
            Size = newSize;
            board = new Colors[newSize, newSize];
            for (int x = 0; x < newSize; x++)
            {
                for (int y = 0; y < newSize; y++)
                {
                    board[x, y] = Colors.Transparent;
                }
            }
            ActualX = 0;
            ActualY = 0;
        }
        public void MoveTo(int x, int y)
        {
            ActualX = x;
            ActualY = y;
        }
        public int IsCanvasColor(Colors color, int vertical, int horizontal)
        {
            if (GetPixel(ActualX + horizontal, ActualY + vertical) == color) return 1;
            else return 0;
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
        public int IsBrushColor(Colors color)
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
            return ActualY;
        }
        public Colors GetPixel(int x, int y) => board[x, y];
        public void Spawn(int x, int y)
        {
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
                int newX = ActualX + dirX;
                int newY = ActualY + dirY;

                if (!IsPositionValid(newX, newY)) break;

                ActualX = newX;
                ActualY = newY;
                for (int dx = -radius; dx <= radius; dx++)
                    for (int dy = -radius; dy <= radius; dy++)
                        SetPixel(ActualX + dx, ActualY + dy);
            }
        }

        private void SetPixel(int x, int y)
        {
            if (IsPositionValid(x, y) && BrushColor != Colors.Transparent)
                board[x, y] = BrushColor;
        }
        public bool IsPositionValid(int x, int y)
            => x >= 0 && x < Size && y >= 0 && y < Size;
        public void DrawMidpointCircle(int cx, int cy, int radius)
        {
            if (BrushColor == Colors.Transparent) return;

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

            int left = cx - w / 2;
            int right = cx + (w - 1) / 2;
            int top = cy - h / 2;
            int bottom = cy + (h - 1) / 2;
            int halfBrush = BrushSize / 2;

            for (int x = left; x <= right; x++)
            {
                PaintWithBrush(x, top, halfBrush);
                PaintWithBrush(x, bottom, halfBrush);
            }

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
        public int GetColorCount(int x1, int x2, int y1, int y2, Colors color)
        {
            int count = 0;
            if (x1 > Size || y1 > Size || x2 > Size || y2 > Size) return 0;
            if (x1 < x2) (x1, x2) = (x2, x1);
            if (y1 > y2) (y1, y2) = (y2, y1);
            
            for (int i = x2; i <= x1; i++)
            {
                for (int j = y1; j <= y2; j++)
                {
                    if (GetPixel(i, j) == color) count++;
                }
            }
            return count;
        }
        public void FillSpace(int startX, int startY, Colors targetColor)
        {
            bool[,] visited = new bool[Size, Size];
            Queue<(int x, int y)> queue = new Queue<(int x, int y)>();

            queue.Enqueue((startX, startY));
            visited[startX, startY] = true;

            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();

                if (GetPixel(x, y) == targetColor)
                {
                    SetPixel(x, y);

                    ExploreNeighbor(x + 1, y, queue, visited, targetColor);
                    ExploreNeighbor(x - 1, y, queue, visited, targetColor);
                    ExploreNeighbor(x, y + 1, queue, visited, targetColor);
                    ExploreNeighbor(x, y - 1, queue, visited, targetColor);
                }
            }
        }
        private void ExploreNeighbor(int x, int y, Queue<(int x, int y)> queue, bool[,] visited, Colors targetColor)
        {

            if (!IsPositionValid(x, y))
                return;

            if (!visited[x, y] && GetPixel(x, y) == targetColor)
            {
                visited[x, y] = true;
                queue.Enqueue((x, y));
            }
        }
    }
}