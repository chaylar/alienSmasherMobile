namespace ShootyMood.Scripts.Managers
{
    public sealed class GameStateManager
    {
        private GameState gameState;

        public void SetState(GameState state)
        {
            gameState = state;
        }

        public GameState GetState()
        {
            return gameState;
        }

        private GameStateManager()
        {
            gameState = GameState.MENU;
        }
        
        private static readonly object stLock = new object();
        private static GameStateManager instance = null;
        public static GameStateManager Instance {
            get {
                lock(stLock) {
                    if (instance == null) {
                        instance = new GameStateManager();
                    }
                    return instance;
                }
            }
        }

        public enum GameState
        {
            MENU,
            PLAY
        }
    }
}