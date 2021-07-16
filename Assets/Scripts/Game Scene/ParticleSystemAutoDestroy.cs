using UnityEngine;
using System.Collections;

public class ParticleSystemAutoDestroy : MonoBehaviour
{
    private ParticleSystem particelSystem;

    public void Awake()
    {
        particelSystem = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        if (particelSystem)
        {
            if (!particelSystem.IsAlive())
            {
                Destroy(gameObject, 1);
            }
        }
    }
}
