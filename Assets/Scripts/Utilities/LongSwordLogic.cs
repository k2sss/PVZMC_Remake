using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSwordLogic : MonoBehaviour
{

    private PlayerMoveController moveController;
    private Animator animator;
    private PlayerItemManager playerItemManager;
    [SerializeField]private GameObject LongSwordAttack;
    private void Start()
    {
        animator = GetComponent<Animator>();
        moveController = GetComponent<PlayerMoveController>();
        playerItemManager = GetComponent<PlayerItemManager>();
    }
    public void OnQuitLongSword()
    {
        moveController.GetRigidbody().useGravity = true;
        PlayerMoveController.Instance.EnableAllControl();
        playerItemManager.TurnToNormalAnimatior();
    }
    public void OnUseLongSword()
    {

        if (!moveController._CanJump) return;
        playerItemManager.TurnToLongSwordAnimator();
        moveController.DisAleAllControl();
        moveController.GetRigidbody().velocity = Vector3.zero;
    }
    private void HitLogic()
    {
        Mob target = MyPhysics.BoxRayCheck<Enemy>(transform.position - transform.forward * 3, 3, transform.forward, 5, 8);

        if (target != null)
        {
            animator.SetBool("Hit", true);
            MonoController.Instance.Invoke(1f, () => animator.SetBool("Hit", false));
            moveController.GetRigidbody().useGravity = false;
            target.Hurt(3);
            CameraAction.Instance.StartShake();
        }
        else
        {

            MonoController.Instance.Invoke(0.9f, () =>
            {
                OnQuitLongSword();
            });

        }
    }
    public void OnPlayerisTopPlace()
    {
        moveController.GetRigidbody().useGravity = true;
        moveController.EnableAllControl();
        animator.SetBool("IsKeyDown", true);
        MonoController.Instance.Invoke(0.5f, () =>
        {
            OnPlayerAttack();


        });

    }
    public void OnPlayerAttack()
    {
        CameraAction.Instance.StartShake();
        animator.SetBool("IsKeyDown", false);
        moveController.DisAleAllControl();
        moveController.GetRigidbody().velocity = Vector3.zero;

        GameObject a = Instantiate(LongSwordAttack);
        a.transform.position = transform.position + transform.forward * 2f;
        MonoController.Instance.Invoke(0.1f, () => a.GetComponent<LongSwordAttack>().StartAttack());
    }
}
