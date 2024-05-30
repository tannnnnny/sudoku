namespace BL
{
    public class Cell
    {
        public int Value { get; private set; } = 0;

        public Cell() : this(0) { }

        public Cell(int value)
        {
            this.Value = value;
        }
    }
}