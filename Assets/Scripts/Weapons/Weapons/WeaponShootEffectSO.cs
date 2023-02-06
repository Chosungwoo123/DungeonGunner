using UnityEngine;

[CreateAssetMenu(fileName = "WeaponShootEffect_", menuName = "Scriptable Objects/Weapons/Weapon Shoot Effect")]
public class WeaponShootEffectSO : ScriptableObject
{
    #region Header WEAPON SHOOT EFFECT DETAILS
    [Space(10)]
    [Header("WEAPON SHOOT EFFECT DETAILS")]
    #endregion

    #region Tooltip
    [Tooltip("The color gradient for the shoot effect. This gradient show the color of particles during their lifetime - from left to right ")]
    #endregion
    public Gradient colorGradient;

    #region Tooltip
    [Tooltip("The length of time the particle ststem is emitting particles")]
    #endregion
    public float duration = 0.5f;

    #region Tooltip
    [Tooltip("The start particle size for the particle effect")]
    #endregion
    public float startParticleSize = 0.25f;

    #region Tooltip
    [Tooltip("The start particle speed for the particle effect")]
    #endregion
    public float startParticleSpeed = 3f;

    #region Tooltip
    [Tooltip("The particle lifetime for the particel effect")]
    #endregion
    public float startLifetime = 0.5f;

    #region Tooltip
    [Tooltip("The maximum number of particle to be emitted")]
    #endregion
    public int maxParticleNumber = 100;

    #region Tooltip
    [Tooltip("The number of particle emitted per second. If zero it will just be the burst number")]
    #endregion
    public int emissionRate = 100;

    #region Tooltip
    [Tooltip("How many particles should be emitted in the particle effect burst")]
    #endregion
    public int burstParticleNumber = 20;

    #region Tooltip
    [Tooltip("The gravity on the particle - a small negarive number will make them float up")]
    #endregion
    public float effectGravity = -0.01f;

    #region Tooltip
    [Tooltip("The sprite for the particle effect. If none is specified then the default particle sprite will be used")]
    #endregion
    public Sprite sprite;

    #region Tooltip
    [Tooltip("The min velocity for the particle over its lifetime. A random value between min and max will be generation.")]
    #endregion
    public Vector3 velocityOverLifetimeMin;

    #region Tooltip
    [Tooltip("The min velocity for the particle over its lifetime. A random value between min and max will be generation.")]
    #endregion
    public Vector3 velocityOverLifetimeMix;

    #region Tooltip
    [Tooltip("weaponShootEffectPrefab contains the particle system for the shoot effect - and is configured by the weaponShootEffectSO")]
    #endregion
    public GameObject weaponShootEffectPrefab;

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(duration), duration, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(startParticleSize), startParticleSize, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(startParticleSpeed), startParticleSpeed, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(startLifetime), startLifetime, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(maxParticleNumber), maxParticleNumber, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(emissionRate), emissionRate, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(burstParticleNumber), burstParticleNumber, true);
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponShootEffectPrefab), weaponShootEffectPrefab);
    }
#endif
    #endregion
}