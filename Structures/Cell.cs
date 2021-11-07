using System.Collections.Generic;
using QC.Structures;

namespace QC
{
    public class Cell
    {
        public Point Index { get; }
        public List<Cell> Neighbours { get; }
        public string Location { get; }

        public Cell(Point index, string location)
        {
            Index = index;
            Location = location;
            Neighbours = new List<Cell>();
        }

        public void AddNeighbour(Cell neighbour)
        {
            Neighbours.Add(neighbour);
        }
        
        public void RemoveNeighbour(Cell neighbour)
        {
            Neighbours.Remove(neighbour);
        }
        
        public void RemoveNeighbourAt(int index)
        {
            Neighbours.RemoveAt(index);
        }

        public bool HasNeighbour(Cell cell)
        {
            return Neighbours.Contains(cell);
        }

        public override string ToString()
        {
            return $"Cell {{Index: {{{Index.X}, {Index.Y}}}, Location: {Location}}}";
        }
    }
}