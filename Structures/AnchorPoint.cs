using System;

namespace QC.Structures
{
    public class AnchorPoint
    {
        public Cell[] AdjacentCells { get; }

        public string Location { get; }
        public Point Index { get; }

        public AnchorPoint(Point index, string location)
        {
            Index = index;
            Location = location;
            AdjacentCells = new Cell[4];
        }

        public void AddAdjacentCells(params Cell[] cells)
        {
            if (cells.Length != AdjacentCells.Length)
                throw new Exception("Number of adjacent cells must be 4.");
            AdjacentCells[0] = cells[0];
            AdjacentCells[1] = cells[1];
            AdjacentCells[2] = cells[2];
            AdjacentCells[3] = cells[3];
        }
    }
}
