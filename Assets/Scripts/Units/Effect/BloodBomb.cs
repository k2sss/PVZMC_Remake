using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBomb : MonoBehaviour
{

    [SerializeField]private Vector3 targetPos;
    [SerializeField] private Vector3 OriginPos;
    [SerializeField] private Vector3 betweenPos;
    [SerializeField] private GameObject bloodExplode;
    public Vector3 RotVector3;
    private float t;
    public float Speed = 1;
    public void SetPos(Vector3 OriginPos,Vector3 targetPos)
    {
        t = 0;
        this.targetPos = targetPos;
        this.OriginPos = OriginPos;
        this.betweenPos = OriginPos + (targetPos - OriginPos) / 2 + Vector3.up * 7;
    }

    private void Update()
    {

        transform.Rotate(RotVector3 * Time.deltaTime);
        t += Time.deltaTime * Speed;
        Vector3 a = (1 - t) * (1 - t) * OriginPos;
        Vector3 b = 2 * t * (1 - t) * betweenPos;
        Vector3 c = t * t * targetPos;
        Vector3 d = a + b + c;
        transform.position = d;
    }
    private void OnTriggerEnter(Collider other)
    {
        int layer = other.gameObject.layer;
        if (layer == 3 || layer == 9)
        {
            GameObject explodeBox = ObjectPool.Instance.GetObject(bloodExplode);
            BlockBox box = explodeBox.GetComponent<BlockBox>();
            SoundSystem.Instance.Play2Dsound("Explode");
            CameraAction.Instance.StartShake();
            box.InitDamage(2, 10, 3);
          
            MonoController.Instance.Invoke(0.1f, () => WorldManager.UpdateWorldInfo());
            explodeBox.transform.position = transform.position;
            ObjectPool.Instance.PushObject(gameObject);
            //±¬Õ¨
        }
       
    }
}
