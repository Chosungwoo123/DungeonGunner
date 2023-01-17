using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[DisallowMultipleComponent]
public class WeaponReloadedEvent : MonoBehaviour
{
    public event Action<WeaponReloadedEvent, WeaponReloadedEventArgs> OnWeaponReloaded;

    public void CallWeaponReloadEvent(Weapon weapon)
    {
        OnWeaponReloaded?.Invoke(this, new WeaponReloadedEventArgs() { weapon = weapon });
    }
}

public class WeaponReloadedEventArgs : EventArgs
{
    public Weapon weapon;
}