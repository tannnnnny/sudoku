using BL.Enums;

namespace BL
{
    public class Game
    {
        public GameLevelEnum Level { get; private set; }
        public DateTime StartAt { get; private set; }
        public Board Board { get; private set; }

        public Game(GameLevelEnum level)
        {
            this.Level = level;
            this.StartAt = DateTime.Now;
            this.Board = new Board(level);
        }

        public void SolveGame()
        {
            this.Board.SolveBoard();
        }

        public void Clear()
        {
            this.Board.GoToInitil();
        }

        public void OneStep()
        {
            this.Board.MakeOneStep();
        }

        public bool IsInitialCell(int row, int col)
        {
            return this.Board.IsInitialCell(row, col);
        }

        public bool IsCorrectCell(int row, int col)
        {
            return this.Board.IsCorrectCell(row, col);
        }

        public List<List<int>> GetBlueCells(int row, int col)
        {
            return this.Board.GetBlueCells(row, col);
        }

        public List<List<int>> GetYellowCells(int row, int col)
        {
            return this.Board.GetYellowCells(row, col);
        }

        public void SetCellValue(int row, int col, int value)
        {
            this.Board.SetCellValue(row, col, value);
        }
    }
}
