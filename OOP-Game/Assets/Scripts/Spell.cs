using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private float spellSpeed = 20f;
    [SerializeField] private float spellRange = 10f;


    private int internalSpellDamage;
    private Vector3 startPosition;
    public int spellDamage
    {
        set
        {
            if(value > 0)
            {
                internalSpellDamage = value;
            }
        }

        get
        {
            return internalSpellDamage;
        }
    }

    private void Start()
    {
        startPosition = transform.position;   
    }

    private void Update()
    {
        CheckRange();
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(spellSpeed * transform.forward * Time.deltaTime, Space.World);
    }

    private void CheckRange()
    {
        if(Vector3.Distance(startPosition, transform.position) > spellRange)
        {
            Destroy(this.gameObject);
        }
    }
}
