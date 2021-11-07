namespace QC.States
{
    public class WhiteTurnState : TurnState
    {
        protected sealed override void Init()
        {
            PlayerPosition = GameManager.GetCellByLocation("e9");
            OpponentPosition = GameManager.GetCellByLocation("e1");
            WinIndex = 0;
            LoseIndex = 8;
            WallCount = 10;
            OpponentWallCount = 10;
        }

        public override void ChangeTurn()
        {
            GameManager.TurnState = new BlackTurnState(!IsBot, this);
        }

        public override TurnState Copy()
        {
            WhiteTurnState newState = new WhiteTurnState(IsBot)
            {
                PlayerPosition = PlayerPosition,
                OpponentPosition = OpponentPosition,
                WinIndex = WinIndex,
                LoseIndex = LoseIndex,
                WallCount = WallCount,
                OpponentWallCount = OpponentWallCount
            };
            return newState;
        }

        public WhiteTurnState(bool isBot, TurnState prevState = null) : base(isBot, prevState) {}
    }
}