using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBreath : MonoBehaviour
{
    [SerializeField]private GameObject Particles;
    [SerializeField]private float Speed;

    private void Update()
    {
        transform.Translate(-Speed * Time.deltaTime, 0, 0);

        if (transform.position.x < 0)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grid"))
        {
            SoundSystem.Instance.Play2Dsound("Treat");
            Grid grid = other.GetComponent<Grid>();
            grid.Transfer2();
            GameObject particle = ObjectPool.Instance.GetObject(Particles);
            particle.transform.position = grid.transform.position;
            WorldManager.UpdateWorldInfo();
        }
    }
}
