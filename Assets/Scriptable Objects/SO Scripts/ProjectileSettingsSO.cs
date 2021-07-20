using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSettingsSO", menuName = "Game/ScriptableObjects/Projectiles/Projectile Settings")]
public class ProjectileSettingsSO : ScriptableObject
{
    [Header("Projectile Visual Settings")]
    public Sprite projectileSprite = null;
    public Color projectileColor = Color.white;
    public float trailLength = 0.5f;
    public Vector2 scale = new Vector2(1, 1);

    [Header("Projectile Movement Settings")]
    [Min(-1), ] public Vector2 direction = Vector2.right;
    public float speed;

    [Header("Projectile Type Settings")]
    public Projectile.ProjectileType projectileType = Projectile.ProjectileType.STRAIGHT;

    [Header("Sine Wave Settings")]
    public float sineAmplitude = 1;
    public float sineFrequency = 1;

    [Header("Circular Setttings")]
    public float circleRadius = 1;
    public bool circleDirection = true;
    public float circleTime = 0.1f;
}
