using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSettingsSO", menuName = "Game/ScriptableObjects/Projectiles/Projectile Settings")]
public class ProjectileSettingsSO : ScriptableObject
{
    public Color projectileColor = Color.white;
    public float trailLength = 0.5f;
    public Vector2 scale = new Vector2(1, 1);
    public Vector2 direction = Vector2.right;
    public float speed;
    public bool useSine = false;
    public float sineAmplitude = 1;
    public float sineFrequency = 1;
    public bool preserveMomentum = false;
}
