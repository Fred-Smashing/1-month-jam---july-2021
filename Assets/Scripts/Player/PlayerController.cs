using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSettingsSO playerSettings;
    [SerializeField] private InputHandlerSO inputHandler;

    private PolygonCollider2D _collider;
    private MeshFilter _meshFilter;

    private Utility.Meshes.GenerateMesh _meshGenerator;

    private void Awake()
    {
        _collider = GetComponent<PolygonCollider2D>();
        _meshFilter = GetComponent<MeshFilter>();

        _meshGenerator = new Utility.Meshes.GenerateMesh(_collider.points, _meshFilter);
    }

    Vector2 inputVector;
    private void Update()
    {
        inputHandler.onUpdateInternal();
        inputVector = inputHandler.GetInputVector();

        if (Input.GetButtonDown("Jump"))
        {
            Shoot(playerSettings.projectilePrefab);
        }

        MovePlayer(inputVector, playerSettings.moveSpeed, playerSettings.acceleration, Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.J))
        {
            IncreaseSize(0.1f);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            DecreaseSize(0.1f);
        }
    }

    private void MovePlayer(Vector2 direction, float speed, float acceleration, float deltaTime)
    {
        var transform = GetComponent<Transform>();

        transform.position = Vector2.Lerp(transform.position, (Vector2)transform.position + direction * speed, acceleration * deltaTime);
    }

    private void Shoot(GameObject projectilePrefab)
    {
        DecreaseSize(playerSettings.shotCost);
        
        var projectileObject = Instantiate(projectilePrefab);
        projectileObject.transform.SetParent(null);
        projectileObject.transform.position = transform.position;

        var projectile = projectileObject.GetComponent<Projectile>();
        projectile.Init(playerSettings.projectileSettings);
    }

    public void IncreaseSize(float amount)
    {
        transform.localScale += new Vector3(amount, amount, 0);
    }

    public void DecreaseSize(float amount)
    {
        transform.localScale -= new Vector3(amount, amount, 0);
    }
}
