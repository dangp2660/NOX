using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    [SerializeField] private Transform lanchPoint;
    [SerializeField] private GameObject prefabs;
    public void firePrefabs()
    {
        GameObject spell = Instantiate(prefabs, lanchPoint.position, lanchPoint.rotation);

        // Lấy hướng của nhân vật (dựa vào scale x)
        float direction = Mathf.Sign(transform.localScale.x);

        // Điều chỉnh scale phép thuật để quay đúng hướng
        Vector3 spellScale = spell.transform.localScale;
        spellScale.x *= direction;
        spell.transform.localScale = spellScale;
        Destroy(spell , 2.5f);
    }

}
