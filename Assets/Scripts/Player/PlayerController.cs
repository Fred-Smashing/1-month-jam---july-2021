using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSettingsSO playerSettings;
    [SerializeField] private InputHandlerSO inputHandler;
    [SerializeField] private GameObject particlePrefab;

    private PolygonCollider2D _collider;
    private MeshFilter _meshFilter;

    private RandomOneShotSound oneShotPlayer;

    private Utility.Meshes.GenerateMesh _meshGenerator;

    private Transform shotPosition;

    private bool playerDead = false;
    private bool playerEnabled = false;

    public void SetPlayerEnabled(bool enabled)
    {
        playerEnabled = enabled;
    }

    public void Init()
    {
        InitMeshDrawingComponents();
        shotPosition = GameObject.Find("ShotPosition").transform;
        oneShotPlayer = GetComponent<RandomOneShotSound>();
    }

    private void InitMeshDrawingComponents()
    {
        if (_collider == null) { _collider = GetComponent<PolygonCollider2D>(); }
        if (_meshFilter == null) { _meshFilter = GetComponent<MeshFilter>(); }

        _meshGenerator = new Utility.Meshes.GenerateMesh(_collider.points, _meshFilter);
    }

    Vector2 inputVector;
    float deathSpeed = 0.1f;
    private void Update()
    {
        if (playerEnabled)
        {

            if (!playerDead)
            {
                inputHandler.onUpdateInternal();
                inputVector = inputHandler.GetInputVector();

                if (Input.GetButtonDown("Jump"))
                {
                    Shoot(playerSettings.projectilePrefab);
                }

                MovePlayer(inputVector, playerSettings.moveSpeed, playerSettings.acceleration, Time.deltaTime);
            }
            else
            {
                float timescale = Mathf.Lerp(Time.timeScale, 0.2f, deathSpeed * Time.timeScale);
                float xScale = Mathf.Lerp(transform.localScale.x, 0f, deathSpeed * Time.timeScale);
                float yScale = Mathf.Lerp(transform.localScale.y, 0f, deathSpeed * Time.timeScale);

                Time.timeScale = timescale;
                transform.localScale = new Vector3(xScale, yScale, 1);

                if (xScale <= 0.1f || yScale <= 0.1f)
                {
                    CreateParticleEffect();
                    Destroy(this.gameObject);

                    FindObjectOfType<GameManager>().EndGame();
                }
            }
        }
        else
        {

        }
    }

    private void MovePlayer(Vector2 direction, float speed, float acceleration, float deltaTime)
    {
        var transform = GetComponent<Transform>();

        var move = Vector2.Lerp(transform.position, (Vector2)transform.position + direction * speed, acceleration * deltaTime);

        var screenTopLeft = Camera.main.ScreenToWorldPoint(Vector2.zero);
        var screenBotRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        var halfSize = new Vector2(_collider.bounds.size.x / 2,
                                    _collider.bounds.size.y / 2);

        move.x = Mathf.Clamp(move.x,
                            screenTopLeft.x + halfSize.x,
                            screenBotRight.x - halfSize.x);
        move.y = Mathf.Clamp(move.y,
                            screenTopLeft.y + halfSize.y,
                            screenBotRight.y - halfSize.y);

        transform.position = move;
    }

    private void Shoot(GameObject projectilePrefab)
    {
        var projectileObject = Instantiate(projectilePrefab);
        projectileObject.transform.SetParent(null);
        projectileObject.transform.position = shotPosition.position;

        var projectile = projectileObject.GetComponent<Projectile>();
        projectile.Init(playerSettings.projectileSettings, this.gameObject);

        oneShotPlayer.PlaySound();
    }

    public void KillPlayer()
    {
        playerDead = true;
    }

    private void CreateParticleEffect()
    {
        var particleObject = Instantiate(particlePrefab);
        particleObject.transform.position = transform.position;
        particleObject.GetComponent<ParticleSystem>().startColor = GetComponent<MeshRenderer>().material.color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            KillPlayer();
        }
    }

    public void IncreaseSize(float amount)
    {
        transform.localScale += new Vector3(amount, amount, 0);
    }

    public void DecreaseSize(float amount)
    {
        transform.localScale -= new Vector3(amount, amount, 0);
    }

    public void ExternalUpdateMesh()
    {
        InitMeshDrawingComponents();
    }
}
