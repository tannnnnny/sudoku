using BL.Enums;

namespace BL
{
    public class Board
    {
        private Random random = new Random();
        public List<List<Cell>> Cells { get; private set; } = new List<List<Cell>>();
        public List<List<Cell>> InitialCells { get; private set; } = new List<List<Cell>>();
        public List<List<Cell>> ResolvedCells { get; private set; } = new List<List<Cell>>();

        public Board(GameLevelEnum level)
        {
            for (int i = 0; i < 9; i++)
            {
                this.Cells.Add(new List<Cell>());
                for(int j = 0; j < 9; j++)
                {
                    this.Cells[i].Add(new Cell());
                }
            }

            this.GenerateBoard(level);
        }

        public void GoToInitil()
        {
            this.Cells = this.GetClone(this.InitialCells);
        }

        public void SolveBoard()
        {
            this.SolveBoard(this.Cells);
        }

        public bool MakeOneStep()
        {
            return this.SolveBoardOneStep(this.Cells);
        }

        public void SetCellValue(int row, int col, int value)
        {
            if (!IsInitialCell(row, col))
            {
                this.Cells[row][col] = new Cell(value);
            }
        }

        public bool IsInitialCell(int row, int col)
        {
            return this.InitialCells[row][col].Value != 0;
        }

        public bool IsCorrectCell(int row, int col)
        {
            return this.Cells[row][col].Value == this.ResolvedCells[row][col].Value;
        }

        public List<List<int>> GetBlueCells(int row, int col)
        {
            var result = new List<List<int>>();
            for (int i = 0; i < 9; i++)
            {
                result.Add(new List<int> { row, i });
                result.Add(new List<int> { i, col });
            }

            var startRow = (int)Math.Floor(row / 3.0) * 3;
            var startCol = (int)Math.Floor(col / 3.0) * 3;

            for (int i = startRow; i < startRow + 3; i++)
            {
                for (int j = startCol; j < startCol + 3; j++)
                {
                    result.Add(new List<int> { i, j });
                }
            }
            return result;
        }

        public List<List<int>> GetYellowCells(int row, int col)
        {
            var result = new List<List<int>>();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (this.Cells[i][j].Value == this.Cells[row][col].Value && this.Cells[row][col].Value != 0)
                    {
                        result.Add(new List<int> { i, j });
                    }
                }
            }
            return result;
        }

        private void GenerateBoard(GameLevelEnum level)
        {
            var cellsToRemove = 0;
            switch (level)
            {
                case GameLevelEnum.Easy:
                    cellsToRemove = 40;
                    break;
                case GameLevelEnum.Medium:
                    cellsToRemove = 50;
                    break;
                case GameLevelEnum.Hard:
                    cellsToRemove = 55;
                    break;
            }

            var counts = 0;
            while (counts < cellsToRemove)
            {
                counts = 0;
                this.MakeBoardEmpty();
                this.GenerateFilledBoard();

                this.ResolvedCells = this.GetClone(this.Cells);

                while (this.RemoveCellIfPossible() && counts < cellsToRemove)
                {
                    counts++;
                };

                this.InitialCells = this.GetClone(this.Cells);
            }

        }

        private void MakeBoardEmpty()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    this.Cells[i][j] = new Cell();
                }
            }
        }

        private void GenerateFilledBoard()
        {
            while (true)
            {
                this.GenerateFilledBoardPrivate();
                if (this.IsBoardFullfilled(this.Cells))
                {
                    break;
                }
            }
        }

        private void GenerateFilledBoardPrivate()
        {
            var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for (int i = 0; i < 81; i++)
            {
                var row = (int)Math.Floor(i / 9.0);
                var col = i % 9;
                if (this.Cells[row][col].Value == 0)
                {
                    this.ShuffleArray(list);
                    for (int j = 0; j < 9; j++)
                    {
                        if (this.Cells[row].Any(x => x.Value == list[j]))
                        {
                            continue;
                        }

                        var column = new List<int>();
                        for (int k = 0; k < 9; k++)
                        {
                            column.Add(this.Cells[k][col].Value);
                        }

                        if (column.Contains(list[j]))
                        {
                            continue;
                        }

                        var square = this.GetSquare(row, col);
                        if (square.Contains(list[j]))
                        {
                            continue;
                        }

                        this.Cells[row][col] = new Cell(list[j]);
                    }

                    if (this.Cells[row][col].Value == 0)
                    {
                        for (int ii = 0; ii < 9; ii++)
                        {
                            for (int jj = 0; jj < 9; jj++)
                            {
                                this.Cells[ii][jj] = new Cell();
                            }
                        }
                        break;
                    }
                }
            }
        }

        private bool RemoveCellIfPossible()
        {
            var list = Enumerable.Range(0, 81).ToList();
            this.ShuffleArray(list);
            foreach (var number in list)
            {
                var row = (int)Math.Floor(number / 9.0);
                var col = number % 9;
                if (this.Cells[row][col].Value != 0)
                {
                    var removedValue = this.Cells[row][col].Value;
                    this.Cells[row][col] = new Cell();

                    if (this.IsBoardSolveable())
                    {
                        return true;
                    }
                    else
                    {
                        this.Cells[row][col] = new Cell(removedValue);
                    }
                }

            }

            return false;
        }

        private List<List<Cell>> GetClone(List<List<Cell>> cells)
        {
            var clone = new List<List<Cell>>();
            for (int i = 0; i < cells.Count; i++)
            {
                clone.Add(new List<Cell>());
                for (int j = 0; j < cells[i].Count; j++)
                {
                    clone[i].Add(new Cell(cells[i][j].Value));
                }
            }

            return clone;
        }

        private bool IsBoardSolveable()
        {
            var clone = this.GetClone(this.Cells);

            return this.SolveBoard(clone);
        }


        private bool SolveBoardOneStep(List<List<Cell>> cells)
        {
            var result = false;
            var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for (int i = 0; i < 81; i++)
            {
                var row = (int)Math.Floor(i / 9.0);
                var col = i % 9;
                if (cells[row][col].Value == 0)
                {
                    var value = new Cell();
                    for (int j = 0; j < 9; j++)
                    {
                        if (cells[row].Any(x => x.Value == list[j]))
                        {
                            continue;
                        }

                        var column = new List<int>();
                        for (int k = 0; k < 9; k++)
                        {
                            column.Add(cells[k][col].Value);
                        }

                        if (column.Contains(list[j]))
                        {
                            continue;
                        }

                        var square = this.GetSquare(row, col, cells);
                        if (square.Contains(list[j]))
                        {
                            continue;
                        }

                        var rowsToContain = new int[3];
                        var columnsToContain = new int[3];
                        var squareNumber = this.GetSquareNumber(row, col);
                        if (squareNumber == 1)
                        {
                            Array.Copy(new int[] { 0, 1, 2 }.ToArray(), rowsToContain, 3);
                            Array.Copy(new int[] { 0, 1, 2 }.ToArray(), columnsToContain, 3);
                        }
                        else if (squareNumber == 2)
                        {
                            Array.Copy(new int[] { 0, 1, 2 }.ToArray(), rowsToContain, 3);
                            Array.Copy(new int[] { 3, 4, 5 }.ToArray(), columnsToContain, 3);
                        }
                        else if (squareNumber == 3)
                        {
                            Array.Copy(new int[] { 0, 1, 2 }.ToArray(), rowsToContain, 3);
                            Array.Copy(new int[] { 6, 7, 8 }.ToArray(), columnsToContain, 3);
                        }
                        else if (squareNumber == 4)
                        {
                            Array.Copy(new int[] { 3, 4, 5 }.ToArray(), rowsToContain, 3);
                            Array.Copy(new int[] { 0, 1, 2 }.ToArray(), columnsToContain, 3);
                        }
                        else if (squareNumber == 5)
                        {
                            Array.Copy(new int[] { 3, 4, 5 }.ToArray(), rowsToContain, 3);
                            Array.Copy(new int[] { 3, 4, 5 }.ToArray(), columnsToContain, 3);
                        }
                        else if (squareNumber == 6)
                        {
                            Array.Copy(new int[] { 3, 4, 5 }.ToArray(), rowsToContain, 3);
                            Array.Copy(new int[] { 6, 7, 8 }.ToArray(), columnsToContain, 3);
                        }
                        else if (squareNumber == 7)
                        {
                            Array.Copy(new int[] { 6, 7, 8 }.ToArray(), rowsToContain, 3);
                            Array.Copy(new int[] { 0, 1, 2 }.ToArray(), columnsToContain, 3);
                        }
                        else if (squareNumber == 8)
                        {
                            Array.Copy(new int[] { 6, 7, 8 }.ToArray(), rowsToContain, 3);
                            Array.Copy(new int[] { 3, 4, 5 }.ToArray(), columnsToContain, 3);
                        }
                        else
                        {
                            Array.Copy(new int[] { 6, 7, 8 }.ToArray(), rowsToContain, 3);
                            Array.Copy(new int[] { 6, 7, 8 }.ToArray(), columnsToContain, 3);
                        }

                        var skip = false;
                        foreach (int r in rowsToContain)
                        {
                            foreach (int c in columnsToContain)
                            {
                                if (r == row && c == col)
                                {
                                    continue;
                                }

                                column = new List<int>();
                                for (int k = 0; k < 9; k++)
                                {
                                    column.Add(cells[k][c].Value);
                                }

                                if (cells[r][c].Value == 0 && !(cells[r].Any(x => x.Value == list[j]) ||
                                    column.Contains(list[j])))
                                {
                                    skip = true;
                                    break;
                                }
                            }
                            if (skip)
                            {
                                break;
                            }
                        }

                        if (skip)
                        {
                            continue;
                        }

                        value = new Cell(list[j]);
                        break;
                    }

                    if (value.Value != 0)
                    {
                        cells[row][col] = value;
                        return true;
                    }
                }
            }

            if (this.IsBoardFullfilled(cells))
            {
                return true;
            }
            else if (!result)
            {
                return false;
            }
            else if (this.SolveBoard(cells))
            {
                return true;
            }
            return false;
        }

        private bool SolveBoard(List<List<Cell>> cells)
        {
            var result = false;
            var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for (int i = 0; i < 81; i++)
            {
                var row = (int)Math.Floor(i / 9.0);
                var col = i % 9;
                if (cells[row][col].Value == 0)
                {
                    var value = new Cell();
                    for (int j = 0; j < 9; j++)
                    {
                        if (cells[row].Any(x => x.Value == list[j]))
                        {
                            continue;
                        }

                        var column = new List<int>();
                        for (int k = 0; k < 9; k++)
                        {
                            column.Add(cells[k][col].Value);
                        }

                        if (column.Contains(list[j]))
                        {
                            continue;
                        }

                        var square = this.GetSquare(row, col, cells);
                        if (square.Contains(list[j]))
                        {
                            continue;
                        }

                        var rowsToContain = new int[3];
                        var columnsToContain = new int[3];
                        var squareNumber = this.GetSquareNumber(row, col);
                        if (squareNumber == 1)
                        {
                            Array.Copy(new int[] { 0, 1, 2 }.ToArray(), rowsToContain, 3);
                            Array.Copy(new int[] { 0, 1, 2 }.ToArray(), columnsToContain, 3);
                        }
                        else if (squareNumber == 2)
                        {
                            Array.Copy(new int[] { 0, 1, 2 }.ToArray(), rowsToContain, 3);
                            Array.Copy(new int[] { 3, 4, 5 }.ToArray(), columnsToContain, 3);
                        }
                        else if (squareNumber == 3)
                        {
                            Array.Copy(new int[] { 0, 1, 2 }.ToArray(), rowsToContain, 3);
                            Array.Copy(new int[] { 6, 7, 8 }.ToArray(), columnsToContain, 3);
                        }
                        else if (squareNumber == 4)
                        {
                            Array.Copy(new int[] { 3, 4, 5 }.ToArray(), rowsToContain, 3);
                            Array.Copy(new int[] { 0, 1, 2 }.ToArray(), columnsToContain, 3);
                        }
                        else if (squareNumber == 5)
                        {
                            Array.Copy(new int[] { 3, 4, 5 }.ToArray(), rowsToContain, 3);
                            Array.Copy(new int[] { 3, 4, 5 }.ToArray(), columnsToContain, 3);
                        }
                        else if (squareNumber == 6)
                        {
                            Array.Copy(new int[] { 3, 4, 5 }.ToArray(), rowsToContain, 3);
                            Array.Copy(new int[] { 6, 7, 8 }.ToArray(), columnsToContain, 3);
                        }
                        else if (squareNumber == 7)
                        {
                            Array.Copy(new int[] { 6, 7, 8 }.ToArray(), rowsToContain, 3);
                            Array.Copy(new int[] { 0, 1, 2 }.ToArray(), columnsToContain, 3);
                        }
                        else if (squareNumber == 8)
                        {
                            Array.Copy(new int[] { 6, 7, 8 }.ToArray(), rowsToContain, 3);
                            Array.Copy(new int[] { 3, 4, 5 }.ToArray(), columnsToContain, 3);
                        }
                        else
                        {
                            Array.Copy(new int[] { 6, 7, 8 }.ToArray(), rowsToContain, 3);
                            Array.Copy(new int[] { 6, 7, 8 }.ToArray(), columnsToContain, 3);
                        }

                        var skip = false;
                        foreach(int r in rowsToContain)
                        {
                            foreach(int c in columnsToContain)
                            {
                                if (r == row && c == col)
                                {
                                    continue;
                                }

                                column = new List<int>();
                                for (int k = 0; k < 9; k++)
                                {
                                    column.Add(cells[k][c].Value);
                                }

                                if (cells[r][c].Value == 0 && !(cells[r].Any(x => x.Value == list[j]) ||
                                    column.Contains(list[j])))
                                {
                                    skip = true;
                                    break;
                                }
                            }
                            if (skip)
                            {
                                break;
                            }
                        }

                        if (skip)
                        {
                            continue;
                        }


                        value = new Cell(list[j]);
                        break;
                    }

                    if (value.Value != 0)
                    {
                        cells[row][col] = value;
                        result = true;
                        break;
                    }
                }
            }

            if (this.IsBoardFullfilled(cells))
            {
                return true;
            }
            else if (!result)
            {
                return false;
            }
            else if (this.SolveBoard(cells))
            {
                return true;
            }
            return false;
        }

        private bool IsBoardFullfilled(List<List<Cell>> cells)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                for(int j = 0; j < cells[i].Count; j++)
                {
                    if (cells[i][j].Value == 0)
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }

        private void ShuffleArray(List<int> array)
        {
            int n = array.Count;
            while (n > 1)
            {
                int k = this.random.Next(n--);
                var temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        private List<int> GetSquare(int row, int col)
        {
            var square = new List<int>();
            if (row < 3)
            {
                if (col < 3)
                {
                    square.Add(this.Cells[0][0].Value);
                    square.Add(this.Cells[0][1].Value);
                    square.Add(this.Cells[0][2].Value);
                    square.Add(this.Cells[1][0].Value);
                    square.Add(this.Cells[1][1].Value);
                    square.Add(this.Cells[1][2].Value);
                    square.Add(this.Cells[2][0].Value);
                    square.Add(this.Cells[2][1].Value);
                    square.Add(this.Cells[2][2].Value);
                }
                else if (col < 6)
                {
                    square.Add(this.Cells[0][3].Value);
                    square.Add(this.Cells[0][4].Value);
                    square.Add(this.Cells[0][5].Value);
                    square.Add(this.Cells[1][3].Value);
                    square.Add(this.Cells[1][4].Value);
                    square.Add(this.Cells[1][5].Value);
                    square.Add(this.Cells[2][3].Value);
                    square.Add(this.Cells[2][4].Value);
                    square.Add(this.Cells[2][5].Value);
                }
                else
                {
                    square.Add(this.Cells[0][6].Value);
                    square.Add(this.Cells[0][7].Value);
                    square.Add(this.Cells[0][8].Value);
                    square.Add(this.Cells[1][6].Value);
                    square.Add(this.Cells[1][7].Value);
                    square.Add(this.Cells[1][8].Value);
                    square.Add(this.Cells[2][6].Value);
                    square.Add(this.Cells[2][7].Value);
                    square.Add(this.Cells[2][8].Value);
                }
            }
            else if (row < 6)
            {
                if (col < 3)
                {
                    square.Add(this.Cells[3][0].Value);
                    square.Add(this.Cells[3][1].Value);
                    square.Add(this.Cells[3][2].Value);
                    square.Add(this.Cells[4][0].Value);
                    square.Add(this.Cells[4][1].Value);
                    square.Add(this.Cells[4][2].Value);
                    square.Add(this.Cells[5][0].Value);
                    square.Add(this.Cells[5][1].Value);
                    square.Add(this.Cells[5][2].Value);
                }
                else if (col < 6)
                {
                    square.Add(this.Cells[3][3].Value);
                    square.Add(this.Cells[3][4].Value);
                    square.Add(this.Cells[3][5].Value);
                    square.Add(this.Cells[4][3].Value);
                    square.Add(this.Cells[4][4].Value);
                    square.Add(this.Cells[4][5].Value);
                    square.Add(this.Cells[5][3].Value);
                    square.Add(this.Cells[5][4].Value);
                    square.Add(this.Cells[5][5].Value);
                }
                else
                {
                    square.Add(this.Cells[3][6].Value);
                    square.Add(this.Cells[3][7].Value);
                    square.Add(this.Cells[3][8].Value);
                    square.Add(this.Cells[4][6].Value);
                    square.Add(this.Cells[4][7].Value);
                    square.Add(this.Cells[4][8].Value);
                    square.Add(this.Cells[5][6].Value);
                    square.Add(this.Cells[5][7].Value);
                    square.Add(this.Cells[5][8].Value);
                }
            }
            else
            {
                if (col < 3)
                {
                    square.Add(this.Cells[6][0].Value);
                    square.Add(this.Cells[6][1].Value);
                    square.Add(this.Cells[6][2].Value);
                    square.Add(this.Cells[7][0].Value);
                    square.Add(this.Cells[7][1].Value);
                    square.Add(this.Cells[7][2].Value);
                    square.Add(this.Cells[8][0].Value);
                    square.Add(this.Cells[8][1].Value);
                    square.Add(this.Cells[8][2].Value);
                }
                else if (col < 6)
                {
                    square.Add(this.Cells[6][3].Value);
                    square.Add(this.Cells[6][4].Value);
                    square.Add(this.Cells[6][5].Value);
                    square.Add(this.Cells[7][3].Value);
                    square.Add(this.Cells[7][4].Value);
                    square.Add(this.Cells[7][5].Value);
                    square.Add(this.Cells[8][3].Value);
                    square.Add(this.Cells[8][4].Value);
                    square.Add(this.Cells[8][5].Value);
                }
                else
                {
                    square.Add(this.Cells[6][6].Value);
                    square.Add(this.Cells[6][7].Value);
                    square.Add(this.Cells[6][8].Value);
                    square.Add(this.Cells[7][6].Value);
                    square.Add(this.Cells[7][7].Value);
                    square.Add(this.Cells[7][8].Value);
                    square.Add(this.Cells[8][6].Value);
                    square.Add(this.Cells[8][7].Value);
                    square.Add(this.Cells[8][8].Value);
                }
            }

            return square;
        }

        private int GetSquareNumber(int row, int col)
        {
            if (row < 3)
            {
                if (col < 3)
                {
                    return 1;
                }
                else if (col < 6)
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
            }
            else if (row < 6)
            {
                if (col < 3)
                {
                    return 4;
                }
                else if (col < 6)
                {
                    return 5;
                }
                else
                {
                    return 6;
                }
            }
            else
            {
                if (col < 3)
                {
                    return 7;
                }
                else if (col < 6)
                {
                    return 8;
                }
                else
                {
                    return 9;
                }
            }
        }

        private List<int> GetSquare(int row, int col, List<List<Cell>> cells)
        {
            var square = new List<int>();
            if (row < 3)
            {
                if (col < 3)
                {
                    square.Add(cells[0][0].Value);
                    square.Add(cells[0][1].Value);
                    square.Add(cells[0][2].Value);
                    square.Add(cells[1][0].Value);
                    square.Add(cells[1][1].Value);
                    square.Add(cells[1][2].Value);
                    square.Add(cells[2][0].Value);
                    square.Add(cells[2][1].Value);
                    square.Add(cells[2][2].Value);
                }
                else if (col < 6)
                {
                    square.Add(cells[0][3].Value);
                    square.Add(cells[0][4].Value);
                    square.Add(cells[0][5].Value);
                    square.Add(cells[1][3].Value);
                    square.Add(cells[1][4].Value);
                    square.Add(cells[1][5].Value);
                    square.Add(cells[2][3].Value);
                    square.Add(cells[2][4].Value);
                    square.Add(cells[2][5].Value);
                }
                else
                {
                    square.Add(cells[0][6].Value);
                    square.Add(cells[0][7].Value);
                    square.Add(cells[0][8].Value);
                    square.Add(cells[1][6].Value);
                    square.Add(cells[1][7].Value);
                    square.Add(cells[1][8].Value);
                    square.Add(cells[2][6].Value);
                    square.Add(cells[2][7].Value);
                    square.Add(cells[2][8].Value);
                }
            }
            else if (row < 6)
            {
                if (col < 3)
                {
                    square.Add(cells[3][0].Value);
                    square.Add(cells[3][1].Value);
                    square.Add(cells[3][2].Value);
                    square.Add(cells[4][0].Value);
                    square.Add(cells[4][1].Value);
                    square.Add(cells[4][2].Value);
                    square.Add(cells[5][0].Value);
                    square.Add(cells[5][1].Value);
                    square.Add(cells[5][2].Value);
                }
                else if (col < 6)
                {
                    square.Add(cells[3][3].Value);
                    square.Add(cells[3][4].Value);
                    square.Add(cells[3][5].Value);
                    square.Add(cells[4][3].Value);
                    square.Add(cells[4][4].Value);
                    square.Add(cells[4][5].Value);
                    square.Add(cells[5][3].Value);
                    square.Add(cells[5][4].Value);
                    square.Add(cells[5][5].Value);
                }
                else
                {
                    square.Add(cells[3][6].Value);
                    square.Add(cells[3][7].Value);
                    square.Add(cells[3][8].Value);
                    square.Add(cells[4][6].Value);
                    square.Add(cells[4][7].Value);
                    square.Add(cells[4][8].Value);
                    square.Add(cells[5][6].Value);
                    square.Add(cells[5][7].Value);
                    square.Add(cells[5][8].Value);
                }
            }
            else
            {
                if (col < 3)
                {
                    square.Add(cells[6][0].Value);
                    square.Add(cells[6][1].Value);
                    square.Add(cells[6][2].Value);
                    square.Add(cells[7][0].Value);
                    square.Add(cells[7][1].Value);
                    square.Add(cells[7][2].Value);
                    square.Add(cells[8][0].Value);
                    square.Add(cells[8][1].Value);
                    square.Add(cells[8][2].Value);
                }
                else if (col < 6)
                {
                    square.Add(cells[6][3].Value);
                    square.Add(cells[6][4].Value);
                    square.Add(cells[6][5].Value);
                    square.Add(cells[7][3].Value);
                    square.Add(cells[7][4].Value);
                    square.Add(cells[7][5].Value);
                    square.Add(cells[8][3].Value);
                    square.Add(cells[8][4].Value);
                    square.Add(cells[8][5].Value);
                }
                else
                {
                    square.Add(cells[6][6].Value);
                    square.Add(cells[6][7].Value);
                    square.Add(cells[6][8].Value);
                    square.Add(cells[7][6].Value);
                    square.Add(cells[7][7].Value);
                    square.Add(cells[7][8].Value);
                    square.Add(cells[8][6].Value);
                    square.Add(cells[8][7].Value);
                    square.Add(cells[8][8].Value);
                }
            }

            return square;
        }

        private void GenerateExample()
        {
            this.Cells[0][0] = new Cell(8);
            this.Cells[0][6] = new Cell(1);
            this.Cells[0][7] = new Cell(5);
            this.Cells[1][0] = new Cell(7);
            this.Cells[1][3] = new Cell(8);
            //this.Cells[1][7] = new Cell(9);
            this.Cells[2][1] = new Cell(9);
            this.Cells[2][4] = new Cell(5);
            this.Cells[2][6] = new Cell(2);
            this.Cells[2][8] = new Cell(4);

            this.Cells[3][0] = new Cell(3);
            this.Cells[3][3] = new Cell(2);
            this.Cells[3][4] = new Cell(6);
            this.Cells[3][5] = new Cell(9);
            this.Cells[3][7] = new Cell(1);
            this.Cells[4][3] = new Cell(5);
            this.Cells[4][8] = new Cell(3);
            this.Cells[5][2] = new Cell(1);
            this.Cells[5][6] = new Cell(9);
            this.Cells[5][7] = new Cell(2);

            this.Cells[6][1] = new Cell(8);
            this.Cells[6][2] = new Cell(4);
            this.Cells[6][4] = new Cell(3);
            this.Cells[6][6] = new Cell(6);
            this.Cells[7][0] = new Cell(2);
            this.Cells[7][3] = new Cell(9);
            this.Cells[7][6] = new Cell(8);
            this.Cells[8][2] = new Cell(3);
            this.Cells[8][4] = new Cell(2);
            this.Cells[8][7] = new Cell(4);
            this.Cells[8][8] = new Cell(9);
        }

        private bool RemoveCellIfPossibleV3()
        {
            var list = Enumerable.Range(0, 81).ToList();
            this.ShuffleArray(list);
            foreach (var number in list)
            {
                var row = (int)Math.Floor(number / 9.0);
                var col = number % 9;
                if (this.Cells[row][col].Value != 0)
                {
                    var removedValue = this.Cells[row][col].Value;
                    this.Cells[row][col] = new Cell();

                    if (this.IsBoardSolveable())
                    {
                        return true;
                    }
                    else
                    {
                        this.Cells[row][col] = new Cell(removedValue);
                    }
                }

            }

            return false;
        }

        private bool RemoveCellIfPossibleV2()
        {
            var list = Enumerable.Range(1, 10).ToList();
            this.ShuffleArray(list);
            foreach (var number in list)
            {
                var rowsWithoutNumber = new List<int>();
                for (int i = 0; i < 9; i++)
                {
                    if (!this.Cells[i].Any(x => x.Value == number))
                    {
                        rowsWithoutNumber.Add(i);
                    }
                }

                var columnsWithoutNumber = new List<int>();
                for (int i = 0; i < 9; i++)
                {
                    var column = new List<int>();
                    for (int k = 0; k < 9; k++)
                    {
                        column.Add(this.Cells[k][i].Value);
                    }

                    if (!column.Contains(number))
                    {
                        columnsWithoutNumber.Add(i);
                    }
                }

                var group = new Dictionary<int, List<int>>();
                for (int i = 0; i < rowsWithoutNumber.Count; i++)
                {
                    for (int j = 0; j < columnsWithoutNumber.Count; j++)
                    {
                        var square = this.GetSquare(rowsWithoutNumber[i], columnsWithoutNumber[j]);
                        var squareNumber = this.GetSquareNumber(rowsWithoutNumber[i], columnsWithoutNumber[j]);
                        if (!square.Contains(number))
                        {
                            if (group.ContainsKey(squareNumber))
                            {
                                group[squareNumber].Add(i);
                                group[squareNumber].Add(j);
                            }
                            else
                            {
                                group.Add(squareNumber, new List<int>() { i, j });
                            }
                        }
                    }
                }

                var toRemove = group.Values.FirstOrDefault(x => x.Count == 2);

                if (toRemove != null)
                {
                    this.Cells[toRemove[0]][toRemove[1]] = new Cell();

                    return true;
                }
            }

            return false;
        }

        private bool IsBoardSolveableV3()
        {
            var clone = this.GetClone(this.Cells);

            return this.SolveBoard(clone);
        }
    }
}
