using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGizoms : MonoBehaviour
{
    public Sprite DefaultGridGizomsSprite;
    public Sprite UnAbleGridGizomsSprite;

    private SpriteRenderer GridGizmosRenderer;
    private void Start()
    {
        GridGizmosRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (PlantManager.Instance.IsSelectedPlant() == true)
        {
            GridGizmosLogic();
        }
        else
        {
            GridGizmosRenderer.enabled = false;
        }
    }
    private void GridGizmosLogic()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20, 1 << 6))
        {   GridGizmosRenderer.enabled = true;
            Grid g = hit.collider.gameObject.GetComponent<Grid>();
            if (g.IsEmpty() == true&&g.IsBlockOccpuied()==false&&g.IsFitForPlant(PlantManager.Instance.SelectedPlant.GetComponent<Plants>().type))
            {
                GridGizmosRenderer.sprite = DefaultGridGizomsSprite;
                transform.position = hit.collider.gameObject.transform.position + new Vector3(0, 0.1f, 0);
            }
            else
            {
                GridGizmosRenderer.sprite = UnAbleGridGizomsSprite;
                transform.position = hit.collider.gameObject.transform.position + new Vector3(0, 0.1f, 0);
            }

        }else
        {
            GridGizmosRenderer.enabled = false;
        }
    }
}
