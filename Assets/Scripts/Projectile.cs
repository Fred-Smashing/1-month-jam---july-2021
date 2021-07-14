using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;

    private ProjectileSettingsSO settings;

    private Collider2D _collider;

    public enum ProjectileType
    {
        STRAIGHT,
        SINE,
        CIRCULAR
    }

    public void Init(ProjectileSettingsSO projectileSettings)
    {
        settings = projectileSettings;

        transform.localScale = settings.scale;

        SetSprite();
        SetUpTrail();

        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private void SetSprite()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (settings.projectileSprite != null) { spriteRenderer.sprite = settings.projectileSprite; }
        spriteRenderer.color = settings.projectileColor;
    }

    private void SetUpTrail()
    {
        var trail = GetComponent<TrailRenderer>();
        trail.startWidth = settings.scale.x;
        trail.startColor = settings.projectileColor;
        trail.endColor = settings.projectileColor;
        trail.time = settings.trailLength;
    }

    float continuousTime = 0;
    private void Update()
    {
        switch (settings.projectileType)
        {
            case ProjectileType.STRAIGHT:
                StraightProjectileMovement(settings.direction,
                                            settings.speed,
                                            Time.deltaTime);
                break;

            case ProjectileType.SINE:
                SineProjectileMovement(settings.direction,
                                        settings.speed,
                                        Time.deltaTime,
                                        settings.sineFrequency,
                                        settings.sineAmplitude,
                                        continuousTime);
                break;

            case ProjectileType.CIRCULAR:
                break;
        }

        DestroyOffScreen();
        continuousTime += Time.deltaTime;
    }

    private void StraightProjectileMovement(Vector2 dir, float speed, float deltaTime)
    {
        transform.position += new Vector3(dir.x * speed, dir.y * speed, 0) * deltaTime;
    }

    private void SineProjectileMovement(Vector2 dir, float speed, float deltaTime, float sineFrequency, float sineAmplitude, float sineTime)
    {
        var sine = Mathf.Sin(sineFrequency * sineTime) * sineAmplitude;

        transform.position = new Vector3(transform.position.x, sine, transform.position.z);

        StraightProjectileMovement(dir, speed, deltaTime);
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

        CreateParticelEffect();

        if (collision.CompareTag("Player"))
        {
            collision?.GetComponent<PlayerController>().KillPlayer();
        }

        Destroy(this.gameObject);
    }

    private void CreateParticelEffect()
    {
        var particleObject = Instantiate(particlePrefab);
        particleObject.transform.position = transform.position;
        particleObject.GetComponent<ParticleSystem>().startColor = settings.projectileColor;
    }
}
