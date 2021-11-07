namespace QC.States
{
    public abstract class TurnState
    {
        public Cell PlayerPosition { get; set; }
        public Cell OpponentPosition { get; protected set; }

        public int WinIndex { get; protected set; }
        public int LoseIndex { get; protected set; }

        public int WallCount { get; set; }
        protected int OpponentWallCount { get; set; }

        protected bool IsBot { get; }

        protected TurnState(bool isBot, TurnState prevState = null)
        {
            IsBot = isBot;
            if (prevState != null)
            {
                PlayerPosition = prevState.OpponentPosition;
                OpponentPosition = prevState.PlayerPosition;
                WinIndex = prevState.LoseIndex;
                LoseIndex = prevState.WinIndex;
                WallCount = prevState.OpponentWallCount;
                OpponentWallCount = prevState.WallCount;
            }

            if (PlayerPosition == null || OpponentPosition == null) Init();
        }
        
        public void MakeMove()
        {
            if (IsBot) GameManager.MakeMove();
            else Input.ReadInput();
        }

        protected abstract void Init();

        public abstract void ChangeTurn();

        public abstract TurnState Copy();
    }
}