using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyShipSettingsSO", menuName = "Game/ScriptableObjects/Enemies/Enemy Settings")]
public class EnemyShipSettingsSO : ScriptableObject
{
    [Header("Ship Type"), Space]
    public EnemyController.EnemyType enemyType = EnemyController.EnemyType.SIMPLE;
    [Min(0.1f)] public float deathSpeed = 0.1f;

    [Header("Simple Ship Settings"), Space]
    public float simpleShipTargetX = 5;
    public float simpleShipSpeed = 1;
    public float simpleShipMoveDistane = 1;

    [Header("Projectile Settings"), Space]
    public float minTimeBetweenShots = 1;
    public float maxTimeBetweenShots = 2;
    public GameObject projectilePrefab;
    public ProjectileSettingsSO projectileSettings;
}
