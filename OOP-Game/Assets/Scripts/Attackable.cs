using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Attackable
{

    void TakeDamage(int damage);
    void Die();

    void CreateHealthBar();
}