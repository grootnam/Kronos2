using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    [SerializeField]
    public float damageRate;

    [SerializeField]
    GameObject hitEffect;

    public float CalculatedDamage()

    {
        float damage = 10f;

        return damage;
    }


    public void HitEffect(Vector3 collisionPos)
        /*
        * ���� ���� �� ����Ʈ�� �߻���Ű�� �Լ�
        * Player.cs �� "OnTriggerEnter"���� ����
        */
    {
        if (hitEffect != null)
        {
            GameObject _hitSwordEffect = GameObject.Instantiate(hitEffect);
            _hitSwordEffect.transform.position = collisionPos;
            _hitSwordEffect.transform.rotation = gameObject.transform.rotation;
            _hitSwordEffect.transform.parent = GameObject.Find("@Effect").transform;

            Destroy(_hitSwordEffect, _hitSwordEffect.GetComponent<ParticleSystem>().main.duration);
        }
        
    }
}
