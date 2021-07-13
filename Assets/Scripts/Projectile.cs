using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    ProjectileSettingsSO settings;

    public void Init(ProjectileSettingsSO projectileSettings)
    {
        settings = projectileSettings;

        transform.localScale = settings.scale;

        GetComponent<SpriteRenderer>().color = settings.projectileColor;

        var trail = GetComponent<TrailRenderer>();

        trail.startWidth = settings.scale.x;

        trail.startColor = settings.projectileColor;
        trail.endColor = settings.projectileColor;
    }

    float time = 0;
    private void Update()
    {
        time += Time.deltaTime;

        if (settings.useSine)
        {
            var sine = Mathf.Sin(settings.sineFrequency * time) * settings.sineAmplitude;

            transform.position += new Vector3(settings.direction.x * settings.speed,
                                            sine,
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
        if (screenPos.x > Screen.width + 200 || screenPos.x < -200)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");
    }
}
