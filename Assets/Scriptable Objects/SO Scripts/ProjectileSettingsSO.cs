using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSettingsSO", menuName = "Game/ScriptableObjects/Projectiles/Projectile Settings")]
public class ProjectileSettingsSO : ScriptableObject
{
    public Vector2 scale = new Vector2(1, 1);
    public Vector2 direction = Vector2.right;
    public float speed;
    public bool useSine = false;
    public float sineAmplitude = 1;
    public float sineFrequency = 1;
}
