using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(WeaponFiredEvent))]
[DisallowMultipleComponent]
public class FireWeapon : MonoBehaviour
{
    private float firePreChargeTimer = 0f;
    private float fireRateCoolDownTimer = 0f;
    private ActiveWeapon activeWeapon;
    private FireWeaponEvent fireWeaponEvent;
    private ReloadWeaponEvent reloadWeaponEvent;
    private WeaponFiredEvent weaponFiredEvent;

    private void Awake()
    {
        // Load components
        activeWeapon = GetComponent<ActiveWeapon>();
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        weaponFiredEvent = GetComponent<WeaponFiredEvent>();
    }

    private void OnEnable()
    {
        // Subscribe to fire weapon event.
        fireWeaponEvent.OnFireWeapon += FireWeaponEvent_OnFireWeapon;
    }

    private void OnDisable()
    {
        // Unsubscribe from fire weapon event.
        fireWeaponEvent.OnFireWeapon -= FireWeaponEvent_OnFireWeapon;
    }

    private void Update()
    {
        // Decrease cooldown timer.
        fireRateCoolDownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Handle fire weapon evnet.
    /// </summary>
    private void FireWeaponEvent_OnFireWeapon(FireWeaponEvent fireWeaponEvent, FireWeaponEventArgs fireWeaponEventArgs)
    {
        WeaponFire(fireWeaponEventArgs);
    }

    /// <summary>
    /// Fire weapon,
    /// </summary>
    private void WeaponFire(FireWeaponEventArgs fireWeaponEventArgs)
    {
        // Handle weapon precharge timer.
        WeaponPreCharge(fireWeaponEventArgs);

        // Weapon fire.
        if (fireWeaponEventArgs.fire)
        {
            // Test if weapon is ready to fire.
            if (IsWeaponReadyToFire())
            {
                FireAmmo(fireWeaponEventArgs.aimAngle, fireWeaponEventArgs.weaponAimAngle, fireWeaponEventArgs.weaponAimDirectionVector);

                ResetCoolDownTimer();

                ResetPreChargeTimer();
            }
        }
    }

    /// <summary>
    /// Handle weapon precharge.
    /// </summary>
    private void WeaponPreCharge(FireWeaponEventArgs fireWeaponEventArgs)
    {
        // Weapon precharge.
        if (fireWeaponEventArgs.firePreviousFrame)
        {
            // Decrase precharge timer if fire button held previous frame.
            firePreChargeTimer -= Time.deltaTime;
        }
        else
        {
            // Else reset the precharge timer.
            ResetPreChargeTimer();
        }
    }

    /// <summary>
    /// Returns ture if the weapon is ready to fire, else returns false
    /// </summary>
    private bool IsWeaponReadyToFire()
    {
        // If there is no ammo and weapon doesn't have infinite ammo then return false
        if (activeWeapon.GetCurrentWeapon().weaponRemainingAmmo <= 0 && !activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteAmmo)
        {
            return false;
        }

        // If the weapon is reloading then return false
        if (activeWeapon.GetCurrentWeapon().isWeaponReloading)
        {
            return false;
        }

        // If the wweapon isn't precharge or is cooling down then return false.
        if (firePreChargeTimer > 0f || fireRateCoolDownTimer > 0f)
        {
            return false;
        }

        // If no ammo in the clip and the weapon doesn't have infinite clip capacity then return false
        if (!activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteClipCapacity && activeWeapon.GetCurrentWeapon().weaponClipRemainingAmmo <= 0)
        {
            // Trigger a reload weapon event.
            reloadWeaponEvent.CallReloadWeaponEvent(activeWeapon.GetCurrentWeapon(), 0);

            return false;
        }

        // Weapon is ready to fire - return true
        return true;
    }

    /// <summary>
    /// Set up ammo using an ammo gameobject and components from the object pool.
    /// </summary>
    private void FireAmmo(float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        AmmoDetailsSO currentAmmo = activeWeapon.GetCurrentAmmo();

        if (currentAmmo != null)
        {
            // Fire ammo routine.
            StartCoroutine(FireAmmoRoutine(currentAmmo, aimAngle, weaponAimAngle, weaponAimDirectionVector));
        }
    }

    /// <summary>
    /// Coroutine to spawn multiple ammo per shot if specified in the ammo details
    /// </summary>
    private IEnumerator FireAmmoRoutine(AmmoDetailsSO currentAmmo, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        int ammoCounter = 0;

        // Get random ammo per shot
        int ammoPerShot = Random.Range(currentAmmo.ammoSpawnAmountMin, currentAmmo.ammoSpawnAmountMax + 1);

        // Get random interval between ammo
        float ammoSpawnInterval;

        if (ammoPerShot > 1)
        {
            ammoSpawnInterval = Random.Range(currentAmmo.ammoSpawnIntervalMin, currentAmmo.ammoSpawnIntervalMax);
        }
        else
        {
            ammoSpawnInterval = 0f;
        }

        // Loop for number of ammo per shot
        while (ammoCounter < ammoPerShot)
        {
            ammoCounter++;

            // Get ammo prefab from array
            GameObject ammoPrefab = currentAmmo.ammoPrefabArray[Random.Range(0, currentAmmo.ammoPrefabArray.Length)];

            // Get random speed value
            float ammoSpeed = Random.Range(currentAmmo.ammoSpeedMin, currentAmmo.ammoSpeedMax);

            // Get Gameobject with IFireable components
            IFireable ammo = (IFireable)PoolManager.Instance.ReuseComponent(ammoPrefab, activeWeapon.GetShootPosition(), Quaternion.identity);

            // Initialise Ammo
            ammo.InitialiseAmmo(currentAmmo, aimAngle, weaponAimAngle, ammoSpeed, weaponAimDirectionVector);

            // Wait for ammo per shot timegap
            yield return new WaitForSeconds(ammoSpawnInterval);
        }

        // Reduce ammo clip count if not infinite clip capacity
        if (!activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteClipCapacity)
        {
            activeWeapon.GetCurrentWeapon().weaponClipRemainingAmmo--;
            activeWeapon.GetCurrentWeapon().weaponRemainingAmmo--;
        }

        // Call weapon fired event
        weaponFiredEvent.CallWeaponFiredEvent(activeWeapon.GetCurrentWeapon());

        // Display weapon shoot effect
        WeaponShootEffect(aimAngle);

        // Weapon fired sound effect
        WeaponSoundEffect();

        // Camera Shake
        CameraShake();
    }

    /// <summary>
    /// Reset cooldown timer
    /// </summary>
    private void ResetCoolDownTimer()
    {
        // Reset cooldown timer
        fireRateCoolDownTimer = activeWeapon.GetCurrentWeapon().weaponDetails.weaponFireRate;
    }

    /// <summary>
    /// Reset precharge timer.
    /// </summary>
    private void ResetPreChargeTimer()
    {
        // Reset precharge timer
        firePreChargeTimer = activeWeapon.GetCurrentWeapon().weaponDetails.weaponPrechargeTime;
    }

    /// <summary>
    /// Display the weapon shoot effect
    /// </summary>
    private void WeaponShootEffect(float aimAngle)
    {
        // Process if there is a shoot effect & prefab
        if (activeWeapon.GetCurrentWeapon().weaponDetails.weaponShootEffect != null && activeWeapon.GetCurrentWeapon().weaponDetails.weaponShootEffect.weaponShootEffectPrefab != null)
        {
            // Get weapon shoot effect gameobject from the  pool with particle system component
            WeaponShootEffect weaponShootEffect = (WeaponShootEffect)PoolManager.Instance.ReuseComponent
                (activeWeapon.GetCurrentWeapon().weaponDetails.weaponShootEffect.weaponShootEffectPrefab, activeWeapon.GetShootEffectPosition(), Quaternion.identity);

            // Set shoot effect
            weaponShootEffect.SetShootEffect(activeWeapon.GetCurrentWeapon().weaponDetails.weaponShootEffect, aimAngle);

            // Set gameobject active (the particle system is set to autematically disable the gameobject once finished
            weaponShootEffect.gameObject.SetActive(true);
        }
    }

    private void CameraShake()
    {
        CinemachineShake.Instance.ShakeCamera(activeWeapon.GetCurrentWeapon().weaponDetails.cameraShakeIntensity, activeWeapon.GetCurrentWeapon().weaponDetails.cameraShakeTime);
    }

    /// <summary>
    /// Play weapon shooting sound effect
    /// </summary>
    private void WeaponSoundEffect()
    {
        if (activeWeapon.GetCurrentWeapon().weaponDetails.weaponFiringSoundEffect != null)
        {
            SoundEffectManager.Instance.PlaySoundEffect(activeWeapon.GetCurrentWeapon().weaponDetails.weaponFiringSoundEffect);
        }
    }
}