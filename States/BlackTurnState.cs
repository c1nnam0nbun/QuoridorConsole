namespace QC.States
{
    public class BlackTurnState : TurnState
    {
        protected sealed override void Init()
        {
            PlayerPosition = GameManager.GetCellByLocation("e1");
            OpponentPosition = GameManager.GetCellByLocation("e9");
            WinIndex = 8;
            LoseIndex = 0;
            WallCount = 10;
            OpponentWallCount = 10;
        }

        public override void ChangeTurn()
        {
            GameManager.TurnState = new WhiteTurnState(!IsBot, this);
        }

        public override TurnState Copy()
        {
            BlackTurnState newState = new BlackTurnState(IsBot)
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

        public BlackTurnState(bool isBot, TurnState prevState = null) : base(isBot, prevState) {}
    }
}