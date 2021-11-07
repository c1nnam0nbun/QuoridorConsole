namespace QC
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            GameManager.Init();
            while (true)
            {
                if (GameManager.TurnState == null) Input.ReadInput();
                else GameManager.TurnState.MakeMove();
            }
        }
    }
}