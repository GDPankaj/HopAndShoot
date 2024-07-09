using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public void Move();
    public void OnCollided(Collision2D other);
}
