using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettingsSO", menuName = "Game/ScriptableObjects/Player/Player Settings")]
public class PlayerSettingsSO : ScriptableObject
{
    public float moveSpeed = 1;
    public float acceleration = 1;
    public float shotCost = 0.1f;
    public GameObject projectilePrefab = null;
    public ProjectileSettingsSO projectileSettings = null;
}
