using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCar : MonoBehaviour
{
    private bool IsGo;
    public float MoveSpeed = 6;
    private Rigidbody rb;
    private AudioSource source;
    private float TargetY;
    private bool Inited;
    public float YSpeed = 3;
    private float disapearTimer;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        source = SoundSystem.Instance.InitAudioSource(gameObject,SoundSystem.Instance.SoundMixer);
        
        TargetY = transform.position.y;
        Inited = true;
        
    }
    private void FixedUpdate()
    {
        if (IsGo == true)
        {
        
            rb.velocity = new Vector3(MoveSpeed,0, 0);
        }
    }
    private void Update()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 5, 0), new Vector3(0, -1, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10, 1 << 3)&&Inited == true)
        {
            TargetY = hit.point.y;
        }
        transform.position = Vector3.Lerp(transform.position,new Vector3(transform.position.x, TargetY, transform.position.z),YSpeed *Time.deltaTime) ;
        if(IsGo == true)
        disapearTimer += Time.deltaTime;
        if (disapearTimer > 6)
        {
            gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            IsGo = true;
            gameObject.tag = "Untagged";
        }
            

        if (other.gameObject.CompareTag("Enemy") && IsGo == true && other.GetComponent<Enemy>().IsBoss == false)
        {
            Enemy enmey = other.GetComponent<Enemy>();
            enmey.Hurt(1000);
            source.PlayOneShot(ResourceSystem.Instance.GetEnemy(enmey.type).DeathSounds[0]);
            enmey.OnDisAppear();
        }
    }
}
