using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PolygonCollider2D _collider;
    private MeshFilter _meshFilter;

    private Utility.Meshes.GenerateMesh _meshGenerator;

    private EnemyShipSettingsSO settings;

    public GameObject particlePrefab;

    private bool isDead = false;

    private EnemyType enemyType = EnemyType.SIMPLE;

    public enum EnemyType
    {
        SIMPLE,//comes on screen and stays in one spot shooting projectiles
        FAST,//flies across the screen quickly shooting projectiles out its side
        SURROUND,//flies circles around the player shooting projectiles towards them
        ADVANCED,//flies up and down matching the height of the player and shoots at them
    }

    public void Init(EnemyShipSettingsSO _settings)
    {
        settings = _settings;

        InitMeshDrawingComponents();

        StartCoroutine(ShotTimer(Random.Range(settings.minTimeBetweenShots, settings.maxTimeBetweenShots)));
    }

    private void InitMeshDrawingComponents()
    {
        if (_collider == null) { _collider = GetComponent<PolygonCollider2D>(); }
        if (_meshFilter == null) { _meshFilter = GetComponent<MeshFilter>(); }

        _meshGenerator = new Utility.Meshes.GenerateMesh(_collider.points, _meshFilter);
    }

    float continuousTime = 0;

    private void Update()
    {
        if (!isDead)
        {
            switch (settings.enemyType)
            {
                case EnemyType.SIMPLE:

                    var horizontal = Mathf.Lerp(transform.position.x, settings.simpleShipTargetX, settings.simpleShipSpeed * Time.deltaTime);

                    var sin = SineMovement(settings.simpleShipSpeed, settings.simpleShipMoveDistane, continuousTime).y * Time.deltaTime;

                    transform.position = new Vector3(horizontal, transform.position.y + sin, transform.position.z);
                    break;

                case EnemyType.FAST:
                    var pos = transform.position;

                    pos += new Vector3(-1 * settings.fastShipSpeed, 0, 0) * Time.deltaTime;

                    transform.position = pos;

                    Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);

                    if (PositionOutsideOfScreen(screenPos) && screenPos.x < -100)
                    {
                        Destroy(this.gameObject);
                    }

                    break;

                case EnemyType.SURROUND:
                    break;

                case EnemyType.ADVANCED:
                    var player = GameObject.FindGameObjectWithTag("Player");

                    if (player != null)
                    {
                        var playerY = player.transform.position.y;
                        var playerX = player.transform.position.x;

                        var yDiff = Mathf.Abs(transform.position.y - (float)playerY);

                        var targetY = Mathf.Lerp(transform.position.y, (float)playerY, yDiff * Time.deltaTime);
                        var targetX = Mathf.Lerp(transform.position.x, (float)playerX + settings.distanceFromPlayer, Time.deltaTime);

                        if (PositionOutsideOfScreen(Camera.main.WorldToScreenPoint(new Vector3(targetX, targetY))))
                        {
                            targetX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
                        }

                        transform.position = new Vector3(targetX, targetY, transform.position.z);
                    }

                    break;
            }

            continuousTime += Time.deltaTime;
        }
        else
        {
            float xScale = Mathf.Lerp(transform.localScale.x, 0f, settings.deathSpeed * Time.timeScale);
            float yScale = Mathf.Lerp(transform.localScale.y, 0f, settings.deathSpeed * Time.timeScale);

            transform.localScale = new Vector3(xScale, yScale, 1);

            if (xScale <= 0.1f || yScale <= 0.1f)
            {
                CreateParticleEffect();
                Destroy(this.gameObject);
            }
        }
    }

    public void KillEnemy()
    {
        StopAllCoroutines();
        GetComponent<Collider2D>().enabled = false;
        isDead = true;
    }

    private void CreateParticleEffect()
    {
        var particleObject = Instantiate(particlePrefab);
        particleObject.transform.position = transform.position;
        particleObject.GetComponent<ParticleSystem>().startColor = GetComponent<MeshRenderer>().material.color;
    }

    private void Shoot()
    {
        foreach (var projectileSetting in settings.projectileSettings)
        {
            CreateProjectile(settings.projectilePrefab, projectileSetting);
        }

        StartCoroutine(ShotTimer(Random.Range(settings.minTimeBetweenShots, settings.maxTimeBetweenShots)));
    }

    private void CreateProjectile(GameObject prefab, ProjectileSettingsSO settings)
    {
        var projectileObj = Instantiate(prefab);

        projectileObj.transform.position = transform.position;
        projectileObj.transform.parent = null;

        var projectileScript = projectileObj.GetComponent<Projectile>();

        projectileScript.Init(settings, this.gameObject);
    }

    private IEnumerator ShotTimer(float time)
    {
        yield return new WaitForSeconds(time);

        Shoot();
    }

    private Vector3 SineMovement(float sineFrequency, float sineAmplitude, float sineTime)
    {
        Vector3 pos;

        var sin = Mathf.Sin(sineFrequency * sineTime) * sineAmplitude;

        pos = new Vector3(0, sin, 0);

        return pos;
    }

    private bool PositionOutsideOfScreen(Vector2 position)
    {
        if (position.x > Screen.width || position.x < 0)
        {
            return true;
        }

        if (position.y > Screen.height || position.y < 0)
        {
            return true;
        }

        return false;
    }

    public void ExternalUpdateMesh()
    {
        InitMeshDrawingComponents();
    }
}
