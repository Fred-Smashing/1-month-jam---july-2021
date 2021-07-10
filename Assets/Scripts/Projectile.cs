using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    ProjectileSettingsSO settings;

    public void Init(ProjectileSettingsSO projectileSettings)
    {
        settings = projectileSettings;
        transform.localScale = projectileSettings.scale;
    }

    private void Update()
    {
        if (settings.useSine)
        {
            transform.position += new Vector3(settings.direction.x * settings.speed,
                                            Mathf.Sin(settings.sineFrequency * Time.time) * settings.sineAmplitude,
                                            0) * Time.deltaTime;
        }
        else
        {
            transform.position += new Vector3(settings.direction.x * settings.speed,
                                            settings.direction.y * settings.speed,
                                            0) * Time.deltaTime;
        }

        DestroyOffScreen();
    }

    private void DestroyOffScreen()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPos.x > Screen.width + 100 || screenPos.x < -100)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");
    }
}
