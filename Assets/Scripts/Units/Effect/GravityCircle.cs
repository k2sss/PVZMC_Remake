using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCircle : MonoBehaviour
{
    // Start is called before the first frame update
    public float effectTime = 0.3f;
    public float pushStrength = 3;
    public AudioClip bonk;
   

    void Start()
    {
        MonoController.Instance.Invoke(effectTime, () => Destroy(gameObject));



    }
    public void Grab()
    {
        SoundSystem.Instance.Play2Dsound(bonk,0.5f);
        GameObject[] allEnemys = GameObject.FindGameObjectsWithTag("Enemy");;
        foreach (GameObject go in allEnemys)
        {
            if(Mathf.Abs(go.transform.position.x - transform.position.x) < 3f&&
                Mathf.Abs(go.transform.position.z - transform.position.z) < 3f)
            {
                Enemy e = go.GetComponent<Enemy>();
                e.Hurt(5);
                e.Vertigo(0.5f);
                e.AddForce((transform.position - e.transform.position).normalized * pushStrength);

            }
        }





    }
}
