namespace QC.Structures
{
    public class Move
    {
        public readonly string Action;
        public readonly Cell TargetCell;
        public readonly AnchorPoint WallPoint;
        public readonly string WallDirection;

        public Move(string action, Cell targetCell, AnchorPoint wallPoint, string wallDirection)
        {
            Action = action;
            TargetCell = targetCell;
            WallPoint = wallPoint;
            WallDirection = wallDirection;
        }
        
        public Move(string action, Cell targetCell) : this(action, targetCell, null, null) {}
        
        public Move(string action, AnchorPoint wallPoint, string wallDirection) : this(action, null, wallPoint, wallDirection) {}

        public override string ToString()
        {
            return TargetCell != null ? $"{Action} {TargetCell.Location}" : $"{Action} {WallPoint.Location}{WallDirection}";
        }

        public string GetToken()
        {
            return TargetCell != null ? $"{TargetCell.Location}" : $"{WallPoint.Location}{WallDirection}";
        }
    }
}