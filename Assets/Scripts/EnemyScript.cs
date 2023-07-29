using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private bool inMenu = false;
    [SerializeField] private int enemyHealth=1;
    public float enemyMovespeed = 5;
    private GameplayScript gameplayScript;
    [SerializeField] private GameObject enemyDeathParticle;

    // Start is called before the first frame update
    void Start()
    {
        if (inMenu == false)
        {
            gameplayScript = GameObject.Find("GameplayManager").GetComponent<GameplayScript>();
            gameplayScript.killAllMonsters += EnemyDie;
        }
        StartCoroutine(EnemyAppearingCor());
    }

    public void TurnInBigSlime()
    {
        enemyHealth = 2;
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = new Vector3(transform.position.x, -2.05f, transform.position.z);
        enemyMovespeed = enemyMovespeed * 0.75f;
    }

    public void EnemyHitted()
    {
        enemyHealth -= gameplayScript.GetPlayerDamage();
        if (enemyHealth <= 0) EnemyDie();
        else Instantiate(enemyDeathParticle, new Vector3(transform.position.x, -2.2f, transform.position.z), Quaternion.Euler(-90, 0, 0));
    }
    private void EnemyDie()
    {
        gameplayScript.killAllMonsters -= EnemyDie;
        Instantiate(enemyDeathParticle, new Vector3(transform.position.x, -2.2f, transform.position.z), Quaternion.Euler(-90, 0, 0));
        GetComponent<CapsuleCollider>().enabled = false;
        gameplayScript.ChangeCountOfEnemiesAlive(-1);
        gameplayScript.ChangeScore(10);
        Destroy(gameObject);
    }

    private IEnumerator EnemyAppearingCor()
    {
        if (inMenu == false)gameplayScript.ChangeCountOfEnemiesAlive(1);
        Vector3 needPos = transform.position;
        transform.position = new Vector3(transform.position.x, -3.4f, transform.position.z);
        Vector3 startPos = transform.position;

        float appearTime = 1.2f;
        float s = 0;
        while (s < 1) 
        {
            transform.position = Vector3.Lerp(startPos, needPos, s*s);
            s += Time.deltaTime/appearTime;
            yield return null;
        }
        transform.position = needPos;

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(MoovingToRandomPoint());
        GetComponentInChildren<Animator>().SetBool("Walking", true);
    }
    private IEnumerator MoovingToRandomPoint()
    {
        while (true)
        {
            transform.eulerAngles = Vector3.zero;
            Vector3 pointToMove = new Vector3(Random.Range(-2f, 2), transform.position.y, Random.Range(-3f, 5.5f));
            transform.LookAt(pointToMove);
            while (Vector3.Distance(transform.position, pointToMove) > 0.5f)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * enemyMovespeed);
                yield return null;
            }
        }
    }

}
