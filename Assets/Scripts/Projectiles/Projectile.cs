using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;

    private ProjectileSettingsSO settings;

    private Collider2D _collider;

    private GameObject creator;

    public enum ProjectileType
    {
        STRAIGHT,
        SINE,
        CIRCULAR
    }

    public void Init(ProjectileSettingsSO projectileSettings, GameObject _creator)
    {
        creator = _creator;

        settings = projectileSettings;

        transform.localScale = settings.scale;

        SetSprite();
        SetUpTrail();

        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;

        circularSpeed = (2 * Mathf.PI) / settings.circleTime;
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
    public float angle = 0;
    float circularSpeed;
    private void Update()
    {
        switch (settings.projectileType)
        {
            case ProjectileType.STRAIGHT:
                transform.position = StraightProjectileMovement(settings.direction, settings.speed);
                break;

            case ProjectileType.SINE:
                var sOriginPos = StraightProjectileMovement(settings.direction, settings.speed);

                var sineMove = SineProjectileMovement(settings.sineFrequency, settings.sineAmplitude, continuousTime) * Time.deltaTime;

                transform.position = new Vector3(sOriginPos.x, sOriginPos.y + sineMove.y, transform.position.z);
                break;

            case ProjectileType.CIRCULAR:
                var cOriginPos = StraightProjectileMovement(settings.direction, settings.speed);

                if (settings.circleDirection == true)
                {
                    angle += circularSpeed * Time.deltaTime;
                }
                else if (settings.circleDirection == false)
                {
                    angle -= circularSpeed * Time.deltaTime;
                }

                var circularMove = CirularProjectileMovement(cOriginPos, angle, settings.circleRadius);

                transform.position = circularMove;
                break;
        }

        DestroyOffScreen();
        continuousTime += Time.deltaTime;
    }

    private Vector3 StraightProjectileMovement(Vector2 dir, float speed)
    {
        var pos = transform.position;
        pos += new Vector3(dir.x * speed, dir.y * speed, 0) * Time.deltaTime;
        return pos;
    }

    private Vector3 SineProjectileMovement(float sineFrequency, float sineAmplitude, float sineTime)
    {
        Vector3 pos;

        var sin = Mathf.Sin(sineFrequency * sineTime) * sineAmplitude;

        pos = new Vector3(0, sin, 0);

        return pos;
    }

    private Vector3 CirularProjectileMovement(Vector3 originPosition, float _angle, float circleRadius)
    {
        float cos = originPosition.x + circleRadius * (AngleCosine(_angle) * Time.deltaTime);
        float sin = originPosition.y + circleRadius * (AngleSin(_angle) * Time.deltaTime);
        Vector3 pos = new Vector3(cos, sin, 0);

        return pos;
    }

    private float AngleSin(float angle)
    {
        return Mathf.Sin(angle);
    }

    private float AngleCosine(float angle)
    {
        return Mathf.Cos(angle);
    }

    private void DestroyOffScreen()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        if (settings.direction.x > 0 && screenPos.x  > Screen.width + 100)
        {
            Destroy(this.gameObject);
        }
        else if (settings.direction.x < 0 && screenPos.x < -100)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != creator && creator!= null)
        {
            if (collision.CompareTag("Player"))
            {
                collision?.GetComponent<PlayerController>().KillPlayer();
                HitTarget();
            }

            if (creator.CompareTag("Player") && collision.CompareTag("Enemy"))
            {
                collision?.GetComponent<EnemyController>().KillEnemy();
                HitTarget();
            }
        }
    }

    private void HitTarget()
    {
        CreateParticelEffect();
        Destroy(this.gameObject);
    }

    private void CreateParticelEffect()
    {
        var particleObject = Instantiate(particlePrefab);
        particleObject.transform.position = transform.position;
        particleObject.GetComponent<ParticleSystem>().startColor = settings.projectileColor;
    }
}
