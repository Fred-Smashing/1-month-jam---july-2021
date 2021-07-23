using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;

    private ProjectileSettingsSO settings;

    private Collider2D _collider;

    private GameObject creator;

    private GameManager gameManager;

    private Vector2 directedDirection = Vector2.zero;

    private Transform playerTransform;

    public enum ProjectileType
    {
        STRAIGHT,
        SINE,
        CIRCULAR,
        DIRECTED,
        HOMING
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

        gameManager = FindObjectOfType<GameManager>();

        playerTransform = gameManager.player.gameObject.transform;

        if (settings.projectileType == ProjectileType.DIRECTED)
        {
            directedDirection = (playerTransform.position - transform.position).normalized;
        }

        if (settings.projectileType == ProjectileType.HOMING)
        {
            StartCoroutine(homingTimer(settings.homingTime));
        }
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
    Vector3 homingTarget = Vector2.zero;
    bool homing = true;
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

                transform.position = CirularProjectileMovement(cOriginPos, angle, settings.circleRadius);
                break;

            case ProjectileType.DIRECTED:
                transform.position = StraightProjectileMovement(directedDirection, settings.speed);

                if (!gameManager.gameOver)
                {
                    DestroyOffScreenDirected();
                }
                break;

            case ProjectileType.HOMING:
                if (playerTransform != null)
                {
                    var dist = Vector3.Distance(playerTransform.position, transform.position);
                    dist = Mathf.Clamp(dist, 0.5f, 1);

                    homingTarget = Vector3.Lerp(homingTarget, playerTransform.position, (settings.homingSpeed * dist) * Time.deltaTime);

                    if (homing)
                    {
                        directedDirection = (homingTarget - transform.position).normalized;
                    }
                }

                transform.position = StraightProjectileMovement(directedDirection, settings.speed);

                if (!gameManager.gameOver)
                {
                    DestroyOffScreenDirected();
                }
                break;
        }


        if (!gameManager.gameOver)
        {
            DestroyOffScreen();
        }

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

    float bufferDistance = 100;
    private void DestroyOffScreen()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        if (settings.direction.x > 0 && screenPos.x > Screen.width + bufferDistance)
        {
            Destroy(this.gameObject);
        }
        else if (settings.direction.x < 0 && screenPos.x < -bufferDistance)
        {
            Destroy(this.gameObject);
        }

        if (settings.direction.y > 0 && screenPos.y > Screen.height + bufferDistance)
        {
            Destroy(this.gameObject);
        }
        else if (settings.direction.y < 0 && screenPos.y < -bufferDistance)
        {
            Destroy(this.gameObject);
        }
    }

    private void DestroyOffScreenDirected()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        if (directedDirection.x > 0 && screenPos.x > Screen.width + bufferDistance)
        {
            Destroy(this.gameObject);
        }
        else if (directedDirection.x < 0 && screenPos.x < -bufferDistance)
        {
            Destroy(this.gameObject);
        }

        if (directedDirection.y > 0 && screenPos.y > Screen.height + bufferDistance)
        {
            Destroy(this.gameObject);
        }
        else if (directedDirection.y < 0 && screenPos.y < -bufferDistance)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != creator && creator != null && !gameManager.gameOver)
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

    public void HitTarget()
    {
        CreateParticelEffect();
        Destroy(this.gameObject);
    }

    private void CreateParticelEffect()
    {
        var particleObject = Instantiate(particlePrefab);
        particleObject.transform.position = transform.position;
        particleObject.GetComponent<ParticleSystem>().startColor = settings.projectileColor;

        particleObject.GetComponent<AudioSource>().pitch = Random.RandomRange(0.8f, 1.2f);
    }

    private IEnumerator homingTimer(float time)
    {
        yield return new WaitForSeconds(time);

        homing = false;
    }
}
