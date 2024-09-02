using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieNHead : MonoBehaviour
{
    private float timer;
    private bool isCancleCollider;
    private float posY;
    
    void Start()
    {
        Invoke("Create", 6);

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 5&&isCancleCollider == false)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            timer = 0;
            isCancleCollider = true;
            Ray ray = new Ray(transform.position + new Vector3(0, 10, 0), Vector3.down);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,20,1<<3))
            {
                posY = hit.point.y;
            }

        }
    }
    
    public void Create()
    {
       GameObject e = Instantiate(ResourceSystem.Instance.GetEnemy(EnemyType.zombieN).prefab);
        float z = 0;
        z = (int)(transform.position.z/2) *2 + 1;


        e.transform.position = new Vector3(transform.position.x + 1.3f, posY, z);
        MonoController.Instance.Invoke(1, () => Destroy(gameObject));
    }
}
