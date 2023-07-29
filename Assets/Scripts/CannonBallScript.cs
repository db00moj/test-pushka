using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "CannonBallDeadZone") Destroy(gameObject);
        if (col.gameObject.tag == "Enemy") col.gameObject.GetComponent<EnemyScript>().EnemyHitted();
        if (col.gameObject.tag == "Boost") 
        {
            GameObject.Find("GameplayManager").GetComponent<GameplayScript>().StartBoostCor(col.gameObject.GetComponent<BoostScript>().boostId);
            Destroy(col.gameObject);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Land") Instantiate(GameObject.Find("GameplayManager").GetComponent<GameplayScript>().smokeParticle, new Vector3(transform.position.x, -2.5f, transform.position.z), Quaternion.Euler(-90, 0, 0));
    }

}
