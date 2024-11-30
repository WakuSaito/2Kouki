using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(GameObject _player)
    {
        //�r���[�|�[�g���W�̃��C���΂�
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        RaycastHit hit = new RaycastHit();
        Debug.Log("a");

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("a");

            //�A�C�e���܂ł̋����𒲂ׂ�
            float distance = Vector3.Distance(hit.transform.position, _player.transform.position);
            Debug.Log(distance);
            //�������͈͓��Ȃ�
            if (distance <= 5.0f)
            {
                Debug.Log("b");

                Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 5);

                GameObject hit_obj = hit.collider.gameObject;
                Debug.Log(hit_obj);
                if (hit_obj.tag == "Body")
                {
                    hit_obj.GetComponentInParent<ZombieManager>().DamageBody(hit.point,2);
                }
                if (hit_obj.tag == "Head")
                {
                    hit_obj.GetComponentInParent<ZombieManager>().DamageHead(2);
                }
            }
        }
    }
}
