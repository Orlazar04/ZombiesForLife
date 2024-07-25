namespace ZombieSpace
{
    // Target pickup statuses
    public enum TPStatus
    {
        Safe,           // Target pickup is chilling
        Threatened,     // Zombies approaching target pickup
        Attacked,       // Zombies attacking target pickup
        Destroyed,      // Target pickup destroyed
    }

    // Reasons for losing a level
    public enum DefeatType
    {
        TargetDestroyed,    // Target pickup was destroyed
        PlayerKilled,       // Player was killed
    }
}
