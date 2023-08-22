namespace ShootyMood.Scripts.ShootyGameEvents
{
    public struct EnemySpawned
    {
        
    }
    
    public struct EnemyKilled
    {
        public bool isFriendly;
        public float timeAddition;
    }

    public struct EnemyEscaped
    {

    }

    public struct PlayButtonClickEvent
    {
        
    }

    public struct PlayerKilled
    {
        
    }
    
    public struct PlayerGotDamage
    {
        public int damage;
    }

    public struct GameStarted
    {
        
    }

    public struct AudioOptionChanged
    {

    }
}