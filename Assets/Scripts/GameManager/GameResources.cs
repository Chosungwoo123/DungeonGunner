using UnityEngine;

public class GameResources : MonoBehaviour
{
    private static GameResources instance;

    public static GameResources Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<GameResources>("GameResources");
            }
            return instance;
        }
    }

    #region Header DUNGEON

    [Space(10)]
    [Header("DUNGEON")]

    #endregion Header DUNGEON

    #region Tooltip

    [Tooltip("Populate with the dungeon RoomNodeTypeListSO")]

    #endregion Tooltip
    public RoomNodeTypeListSO roomNodeTypeList;
}