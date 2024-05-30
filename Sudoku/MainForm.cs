using BL;
using BL.Enums;
using Sudoku.Components;
using System.Runtime.CompilerServices;

namespace Sudoku
{
    public partial class MainForm : Form
    {
        private bool loading = false;
        private bool showErrors = false;
        private int boardTopMargin = 60;
        private int boardLeftMargin = 30;
        private int cellSize = 40;
        private int? selectedCellRow = null;
        private int? selectedCellCol = null;
        private List<CellButton> cellButtons = new List<CellButton>();

        public Game Game { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void RedrawTimer()
        {
            if (this.Game == null)
            {
                TimerLabel.Text = "";
                return;
            }

            var diff = DateTime.Now - this.Game.StartAt;
            TimerLabel.Text = $"{diff.Minutes:00}:{diff.Seconds:00}";
        }


        private void DrawBoard()
        {
            if (this.Game == null)
            {
                return;
            }

            var gap = 10;

            for (int i = 0; i < this.Game.Board.Cells.Count; i++)
            {
                for (int j = 0; j < this.Game.Board.Cells[i].Count; j++)
                {
                    var x = boardLeftMargin + ((gap + cellSize) * j);
                    var y = boardTopMargin + ((gap + cellSize) * i);

                    var cellButton = new CellButton(i, j);
                    cellButton.Location = new Point(x, y);
                    cellButton.Width = cellSize;
                    cellButton.Height = cellSize;
                    cellButton.BackColor = Color.White;
                    cellButton.Text = this.Game.Board.Cells[i][j].Value != 0 ? this.Game.Board.Cells[i][j].Value.ToString() : "";
                    cellButton.Click += (object? sender, EventArgs e) =>
                    {
                        var button = (CellButton)sender!;
                        this.selectedCellRow = button.Row;
                        this.selectedCellCol = button.Col;
                        this.RedrawBoard();
                    };
                    Controls.Add(cellButton);
                    this.cellButtons.Add(cellButton);
                }
            }
        }

        public void RedrawBoard()
        {
            if (this.Game == null)
            {
                return;
            }

            if (this.cellButtons.Count == 0)
            {
                this.DrawBoard();
            }

            var blueCells = new List<List<int>>();
            if (selectedCellRow != null && selectedCellCol != null)
            {
                blueCells = this.Game.GetBlueCells(this.selectedCellRow.Value, this.selectedCellCol.Value);
            }

            var yellowCells = new List<List<int>>();
            if (selectedCellRow != null && selectedCellCol != null)
            {
                yellowCells = this.Game.GetYellowCells(this.selectedCellRow.Value, this.selectedCellCol.Value);
            }

            for (int i = 0; i < this.Game.Board.Cells.Count; i++)
            {
                for (int j = 0; j < this.Game.Board.Cells[i].Count; j++)
                {

                    var cellButton = this.cellButtons[i * 9 + j];
                    cellButton.BackColor = Color.White;
                    cellButton.Text = this.Game.Board.Cells[i][j].Value != 0 ? this.Game.Board.Cells[i][j].Value.ToString() : "";

                    if (this.Game.IsInitialCell(i, j))
                    {
                        cellButton.BackColor = Color.LightGray;
                    }

                    if (blueCells.Any(x => x[0] == i && x[1] == j))
                    {
                        cellButton.BackColor = Color.LightBlue;
                    }

                    if (yellowCells.Any(x => x[0] == i && x[1] == j))
                    {
                        cellButton.BackColor = Color.LightYellow;
                    }

                    if (selectedCellRow == i && selectedCellCol == j)
                    {
                        cellButton.BackColor = Color.Yellow;
                    }

                    if (this.showErrors && !this.Game.IsCorrectCell(i, j) && this.Game.Board.Cells[i][j].Value != 0)
                    {
                        cellButton.BackColor = Color.IndianRed;
                    }
                }
            }
        }

        private void StartEasyButton_Click(object sender, EventArgs e)
        {
            if (this.loading)
            {
                return;
            }

            this.RedrawBoard();
            this.loading = true;

            this.Game = new Game(GameLevelEnum.Easy);

            this.loading = false;
            this.RedrawBoard();

        }

        private void StartMediumButton_Click(object sender, EventArgs e)
        {
            if (this.loading)
            {
                return;
            }

            this.RedrawBoard();
            this.loading = true;

            this.Game = new Game(GameLevelEnum.Medium);

            this.loading = false;
            this.RedrawBoard();

        }

        private void StartHardButton_Click(object sender, EventArgs e)
        {
            if (this.loading)
            {
                return;
            }

            this.RedrawBoard();
            this.loading = true;

            this.Game = new Game(GameLevelEnum.Hard);

            this.loading = false;
            this.RedrawBoard();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.RedrawTimer();
        }

        private void SolveButton_Click(object sender, EventArgs e)
        {
            if (this.Game != null)
            {
                this.Game.SolveGame();
                this.RedrawBoard();
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            if (this.Game != null)
            {
                this.Game.Clear();
                this.RedrawBoard();
            }
        }

        private void MakeOneStepButton_Click(object sender, EventArgs e)
        {
            if (this.Game != null)
            {
                this.Game.OneStep();
                this.RedrawBoard();
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue >= 48 && e.KeyValue <= 57)
            {
                this.Game.SetCellValue(selectedCellRow!.Value, selectedCellCol!.Value, e.KeyValue - 48);
                this.RedrawBoard();
            }
        }

        private void ShowErrorsButton_Click(object sender, EventArgs e)
        {
            if (this.showErrors)
            {
                this.ShowErrorsButton.Text = "Show Errors";
                this.showErrors = false;
            }
            else
            {
                this.ShowErrorsButton.Text = "Hide Errors";
                this.showErrors = true;
            }
            this.RedrawBoard();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}