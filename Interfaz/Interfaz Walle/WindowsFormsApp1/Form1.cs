using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Canvas drawingCanvas;
        private int pixelSize = 12;
        public Form1()
        {
            InitializeComponent();
            InitializeCanvas();
            SetupPictureBox();
            TextEditor.VScroll += rtbCode_VScroll;
            TextEditor.TextChanged += rtbCode_TextChanged;
            TextEditor.Resize += rtbCode_Resize;
            pnlLineNumbers.Paint += pnlLineNumbers_Paint;
            TextEditor.Select();
        }
        private void InitializeCanvas()
        {
            int initialSize = 50;
            drawingCanvas = new Canvas(initialSize);
            numericUpDown1.Value = initialSize;
            UpdatePictureBoxSize();
        }

        private void SetupPictureBox()
        {
            picCanvas.SizeMode = PictureBoxSizeMode.Normal;
            picCanvas.Paint += PicCanvas_Paint;
            picCanvas.MouseDown += PicCanvas_MouseDown;
            UpdatePictureBoxSize();
        }

        private void ResizeButton_Click_1(object sender, EventArgs e)
        {
            if (numericUpDown1.Value <= 0) numericUpDown1.Value = 1;
            int newSize = (int)numericUpDown1.Value;

            drawingCanvas.Resize(newSize);
            UpdatePictureBoxSize();
            picCanvas.Invalidate();
        }

        private void PicCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            int canvasX = e.X / pixelSize;
            int canvasY = e.Y / pixelSize;

            if (canvasX >= 0 && canvasX < drawingCanvas.Size &&
                canvasY >= 0 && canvasY < drawingCanvas.Size)
            {
                drawingCanvas.ActualX = canvasX;
                drawingCanvas.ActualY = canvasY;
                drawingCanvas.board[canvasX, canvasY] = drawingCanvas.BrushColor;
                picCanvas.Invalidate();
            }
        }
        private void DrawWallEPositionIndicator(Graphics g)
        {
            if (drawingCanvas == null) return;

            int x = drawingCanvas.ActualX;
            int y = drawingCanvas.ActualY;

            // Calcular posición en el PictureBox
            int screenX = x * pixelSize;
            int screenY = y * pixelSize;

            // Dibujar un indicador "X" roja
            using (Pen redPen = new Pen(Color.Blue, 2))
            {
                // Dibujar X
                g.DrawLine(redPen, screenX + 2, screenY + 2,
                           screenX + pixelSize - 2, screenY + pixelSize - 2);
                g.DrawLine(redPen, screenX + pixelSize - 2, screenY + 2,
                           screenX + 2, screenY + pixelSize - 2);

                // Dibujar círculo alrededor
                g.DrawEllipse(redPen, screenX + 2, screenY + 2,
                              pixelSize - 4, pixelSize - 4);
            }

        }
        private void PicCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (drawingCanvas == null) return;

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

            for (int x = 0; x < drawingCanvas.Size; x++)
            {
                for (int y = 0; y < drawingCanvas.Size; y++)
                {
                    var rect = new Rectangle(
                        x * pixelSize,
                        y * pixelSize,
                        pixelSize,
                        pixelSize
                    );

                    Color color = ConvertCanvasColor(drawingCanvas.board[x, y]);

                    using (var brush = new SolidBrush(color))
                    {
                        e.Graphics.FillRectangle(brush, rect);
                    }

                    e.Graphics.DrawRectangle(Pens.LightGray, rect);
                }
            }
            DrawWallEPositionIndicator(e.Graphics);
        }

        public Color ConvertCanvasColor(Colors canvasColor)
        {
            switch (canvasColor)
            {
                case Colors.Blue: return Color.Blue;
                case Colors.Red: return Color.Red;
                case Colors.Green: return Color.Green;
                case Colors.Yellow: return Color.Yellow;
                case Colors.Black: return Color.Black;
                case Colors.White: return Color.White;
                case Colors.Purple: return Color.Purple;
                case Colors.Orange: return Color.Orange;
                case Colors.Transparent: return Color.Transparent;
                default: return Color.White;
            }
        }

        private void UpdatePictureBoxSize()
        {
            if (drawingCanvas != null)
            {
                picCanvas.Size = new Size(
                    drawingCanvas.Size * pixelSize,
                    drawingCanvas.Size * pixelSize
                );
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Archivos pixel Walle (*.pw)|*.pw";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(openFileDialog1.FileName).Equals(".pw", StringComparison.OrdinalIgnoreCase))
                {
                    TextEditor.Text = File.ReadAllText(openFileDialog1.FileName);
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Archivos pixel Walle (*.pw)|*.pw";
            saveFileDialog1.DefaultExt = "pw";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                if (!Path.GetExtension(fileName).Equals(".pw", StringComparison.OrdinalIgnoreCase))
                {
                    fileName += ".pw";
                }
                File.WriteAllText(fileName, TextEditor.Text);
            }
        }

        private void executeButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = string.Empty;
            string codigoFuente = TextEditor.Text;

            // Crea un NUEVO contexto y lista de errores en cada ejecución
            Context context = new Context();
            List<CompilingError> errors = new List<CompilingError>();

            var lexer = Compiling.Lexical;
            var tokens = lexer.GetTokens(codigoFuente, errors);

            // Crea un NUEVO parser con los nuevos tokens y contexto
            Parser parser = new Parser(
                tokens.ToList(),
                new TokenStream(tokens),
                drawingCanvas,
                context,
                errors
            );

            ProgramNode block = parser.ParseProgram();
            block.checksemantic(context, errors);

            if (errors.Count == 0)
            {
                block.Execute();
            }
            else
            {
                foreach (CompilingError item in errors)
                {
                    richTextBox1.Text += item.ToString() + "\n";
                }
            }
            UpdatePictureBoxSize();
            picCanvas.Invalidate();
        }

        private void pnlLineNumbers_Paint(object sender, PaintEventArgs e)
        {
            using (var brush = new SolidBrush(Color.DimGray))
            {
                int lineHeight = TextEditor.Font.Height;
                int firstIndex = TextEditor.GetCharIndexFromPosition(Point.Empty);
                int firstLine = TextEditor.GetLineFromCharIndex(firstIndex);
                int currentY = 0;
                for (int i = firstLine; i < TextEditor.Lines.Length; i++)
                {
                    if (currentY > pnlLineNumbers.Height) break;

                    e.Graphics.DrawString(
                        (i + 1).ToString(),
                        TextEditor.Font,
                        brush,
                        pnlLineNumbers.Width - TextRenderer.MeasureText((i + 1).ToString(), TextEditor.Font).Width - 5,
                        currentY
                    );

                    currentY += lineHeight;
                }
            }
        }

        private void rtbCode_VScroll(object sender, EventArgs e)
        {
            pnlLineNumbers.Invalidate();
        }

        private void rtbCode_TextChanged(object sender, EventArgs e)
        {
            pnlLineNumbers.Invalidate();
        }

        private void rtbCode_Resize(object sender, EventArgs e)
        {
            pnlLineNumbers.Invalidate();
        }

        
    }
}
