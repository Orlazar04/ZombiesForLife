// Author : Olivia Lazar
namespace ZombieSpace
{
    // Operational state of a level
    public enum LevelState
    {
        Active,     // Running
        Paused,     // Temporarily suspended
        Over,       // Permanently finished
    }

    // Reasons for losing a level
    public enum DefeatType
    {
        TargetDestroyed,    // Target pickup was destroyed
        PlayerKilled,       // Player was killed
    }

    // Zombie target that affects movement and attacking
    public enum ZombieTarget
    {
        None,       // Moving randomly
        Player,     // Moves towards player
        Pickup,     // Moves towards pickup
        Ally,       // Moves towards cured human
    }

    // Target pickup state
    public enum PickupState
    {
        Safe,           // Target pickup has no zombies nearby
        Threatened,     // Zombies approaching target pickup
        Attacked,       // Zombies attacking target pickup
        Destroyed,      // Target pickup destroyed
    }    
}
