using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSwordAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Mob> targetList = new List<Mob>();

    public void StartAttack()
    {
        StartCoroutine(AttackCourtine());

    }
    IEnumerator AttackCourtine()
    {
        for(int i = 0;i<7;i++)
        {
            yield return new WaitForSecondsRealtime(0.05f);
            if(targetList.Count != 0)
            SoundSystem.Instance.PlayRandom2Dsound("HIT");
            for(int j = 0;j<targetList.Count;j++)
            {
                if (targetList[j] != null && !targetList[j].IsDead)
                {
                    targetList[j].Hurt(5);
                    
                }
            }
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            targetList.Add(other.GetComponent<Mob>());
        }
    }
}
