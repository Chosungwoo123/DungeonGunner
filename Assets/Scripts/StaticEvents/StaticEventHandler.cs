using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class StaticEventHandler
{
    // Room changed event
    public static event Action<RoomChangeEventArgs> OnRoomChanged;

    public static void CallRoomChangedEvent(Room room)
    {
        OnRoomChanged?.Invoke(new RoomChangeEventArgs() { room = room });
    }
}

public class RoomChangeEventArgs : EventArgs
{
    public Room room;
}