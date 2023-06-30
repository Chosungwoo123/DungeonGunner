using UnityEngine;

[System.Serializable]
public class RoomEnemySpawnParameters
{
    #region Tooltip

    [Tooltip("Defines the dungeon level for this room with regard to how many enemies in total should be spawned")]

    #endregion
    public DungeonLevelSO dungeonLevel;

    #region Tooltip

    [Tooltip(
        "The minimum number of enemies to spawn in this room for this dungeon level. The actual number will be a random value between the minimum values and maximum values")]

    #endregion
    public int minTotalEnemiesToSpawn;

    #region Tootip

    [Tooltip(
        "The maximum number of enemies to spawn in this room for this dungeon level. The actual number will be a random value between the minimum and maximum values.")]

    #endregion
    public int maxTotalEnemiesToSpawn;

    #region Tooltip

    [Tooltip(
        "The minimum number of concurrent enemies to spawn in this room for this dungeon level. The actual number will be a random value between the minimum and maximum values.")]

    #endregion
    public int minConcurrentEnemies;
    
    #region Tooltip

    [Tooltip(
        "The maximum number of concurrent enemies to spawn in this room for this dungeon level. The actual number will be a random value between the minimum and maximum values.")]

    #endregion
    public int maxConcurrentEnemies;

    #region Tooltip

    [Tooltip(
        "The minimum spawn interval in seconds for enemies in this room for this dungeon level. The actual number will be a random value between the minimum and maximum values.")]

    #endregion
    public int minSpawnInterval;
    
    #region Tooltip

    [Tooltip(
        "The maximum spawn interval in seconds for enemies in this room for this dungeon level. The actual number will be a random value between the minimum and maximum values.")]

    #endregion
    public int maxSpawnInterval;
}