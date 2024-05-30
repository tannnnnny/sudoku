namespace Sudoku.Components
{
    public class CellButton : Button
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public CellButton(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }
    }
}
