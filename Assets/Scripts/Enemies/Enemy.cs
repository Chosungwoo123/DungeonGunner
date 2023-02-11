using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

#region REQUIRE CONPONENTS
[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(PolygonCollider2D))]
#endregion

[DisallowMultipleComponent]
public class Enemy : MonoBehaviour
{
    [HideInInspector] public EnemyDetailsSO enemyDetails;
    [HideInInspector] public SpriteRenderer[] spriteRendererArray;

    private CircleCollider2D circleCollider2D;
    private PolygonCollider2D polygonCollider2D;

    private void Awake()
    {
        // Load components
        circleCollider2D = GetComponent<CircleCollider2D>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        spriteRendererArray = GetComponentsInChildren<SpriteRenderer>();
    }
}