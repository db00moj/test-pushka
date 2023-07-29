using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayScript : MonoBehaviour
{
    [SerializeField] private int score = 0;
    [SerializeField] private int countOfEnemiesAlive = 0;
    private int enemiesSpawned = 0;

    [SerializeField] private int playerDamage = 1;
    [SerializeField] private float shootingCooldown = 1f;
    [SerializeField] private float monsterSpawnCooldown = 2.2f;
    private float startMonsterCooldown;
    private bool isPlayerAbleToShoot = true;

    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject[] boostPrefab;
    [SerializeField] private GameObject cannonBallPrefab;
    public UnityAction killAllMonsters;

    [Space(20)]

    [SerializeField] private AudioSource audioShot;
    [SerializeField] private AudioSource audioBoost;

    [Space(20)]

    [SerializeField] private Text scoreText;
    [SerializeField] private Text countOfEnemiesAliveText;
    [SerializeField]private GameObject cooldownIndicator;
    public GameObject smokeParticle;
    [SerializeField] private GameObject canvasDefault;
    [SerializeField] private GameObject canvasLose;
    private BoostsInfo boostsInfo;
    private IEnumerator[] boostCor;
    private Coroutine[] startedBoostCor;


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        startMonsterCooldown = monsterSpawnCooldown;
        boostsInfo = GetComponent<BoostsInfo>();
        BoostCorsInitialization();
        StartCoroutine(SpawningMonsters());
        StartCoroutine(SpawningBoosts());
    }

    public void ShootTheBall()
    {
        if (isPlayerAbleToShoot)
        {
            audioShot.Play();
            Ray rayFromMousePos = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit rayhitFromMouse;
            Physics.Raycast(rayFromMousePos, out rayhitFromMouse);

            GameObject newCannonBall = Instantiate(cannonBallPrefab, new Vector3(0, 3.4f, -6), Quaternion.identity);
            newCannonBall.transform.LookAt(rayhitFromMouse.point);
            newCannonBall.GetComponent<Rigidbody>().AddForce(newCannonBall.transform.forward * 40, ForceMode.Impulse);

            StartCoroutine(ShootingCooldownProcess(shootingCooldown));
        }

    }
    private IEnumerator ShootingCooldownProcess(float cooldownValue)
    {
        isPlayerAbleToShoot = false;
        cooldownIndicator.SetActive(true);
        Image cdImage = GameObject.Find("CD_Main").GetComponent<Image>();

        float s = 0;
        while (s < cooldownValue)
        {
            cdImage.fillAmount = s / cooldownValue;
            s += Time.deltaTime;
            yield return null;
        }
        isPlayerAbleToShoot = true;
        cooldownIndicator.SetActive(false);
    }

    private IEnumerator SpawningBoosts()
    {
        while (true)
        {
            float s = 0;
            while (s < 9f+Random.Range(-2f,3f))
            {
                s += Time.deltaTime;
                yield return null; 
            }

            GameObject newBoost = Instantiate(boostPrefab[Random.Range(0, boostPrefab.Length)], new Vector3(Random.Range(-2f, 2f), -2.05f, Random.Range(-3f, 5.5f)), Quaternion.identity);
        }
    }

    private IEnumerator SpawningMonsters()
    {
        while (true)
        {
            float s = 0;
            float thisMonsterSpawnCooldown = Mathf.Clamp(monsterSpawnCooldown + Random.Range(-0.15f, 0.15f), 1.6f, 3);
            while (s < thisMonsterSpawnCooldown)
            {
                if (startedBoostCor[1] == null) s += Time.deltaTime;
                yield return null;
            }

            GameObject newEnemy = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], new Vector3(Random.Range(-2f, 2f), -2.25f, Random.Range(-3f, 5.5f)), Quaternion.identity);
            newEnemy.GetComponent<EnemyScript>().enemyMovespeed += enemiesSpawned * 0.05f;

            if (enemiesSpawned >= 15)
            {
                if (Random.Range(0f, 100f) < 15 + (enemiesSpawned - 15)) newEnemy.GetComponent<EnemyScript>().TurnInBigSlime();
            }

            enemiesSpawned++;
            monsterSpawnCooldown = startMonsterCooldown - (enemiesSpawned / 5) * 0.025f;
        }
    }

    public void ChangeScore(int value)
    {
        score += value;
        scoreText.text = "" + score;
    }

    public void ChangeCountOfEnemiesAlive(int value)
    {
        countOfEnemiesAlive += value;
        countOfEnemiesAliveText.text = countOfEnemiesAlive+"/10";

        if (countOfEnemiesAlive >= 10) PlayerDie();
    }

    public int GetPlayerDamage()
    {
        return playerDamage;
    }

    private void PlayerDie()
    {

        StopAllCoroutines();
        canvasDefault.SetActive(false);
        canvasLose.SetActive(true);

        if (score > PlayerPrefs.GetInt("BestScore")) PlayerPrefs.SetInt("BestScore", score);
        PlayerPrefs.SetInt("LastScore", score);

        GameObject.Find("YourScoreText").GetComponent<Text>().text = "YOUR SCORE: " + score;
        GameObject.Find("BestScoreText").GetComponent<Text>().text = "HIGH SCORE: " + PlayerPrefs.GetInt("BestScore");
    }

    public void LoadScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void StartBoostCor(int boostId)
    {
        if (startedBoostCor[boostId] != null) { StopCoroutine(startedBoostCor[boostId]); BoostCorsInitialization(); }
        startedBoostCor[boostId] = StartCoroutine(boostCor[boostId]);
        audioBoost.Play();
    }
    private void BoostCorsInitialization()
    {
        boostCor = new IEnumerator[4];
        startedBoostCor = new Coroutine[4];
        boostCor[0] = KillBoost();
        boostCor[1] = FreezeBoost();
        boostCor[2] = AttackspeedBoost();
        boostCor[3] = DamageBoost();
    }

    private IEnumerator KillBoost()
    {
        Debug.Log("1");
        boostsInfo.boostIcon[0].sprite = boostsInfo.boostSprite[0];
        float boostTimelength = 0.5f;
        float s = 0;
        while (s < 1)
        {
            boostsInfo.boostTimeline[0].fillAmount = s;
            s += Time.deltaTime / boostTimelength;
            yield return null;
        }
        killAllMonsters.Invoke();
        boostsInfo.boostIcon[0].sprite = boostsInfo.boostSpriteDisabled[0];
        boostsInfo.boostTimeline[0].fillAmount = 0;
        BoostCorsInitialization();
    }

    private IEnumerator FreezeBoost()
    {
        boostsInfo.boostIcon[1].sprite = boostsInfo.boostSprite[1];
        float boostTimelength = 3f;
        float s = 0;
        while (s < 1)
        {
            boostsInfo.boostTimeline[1].fillAmount = s;
            s += Time.deltaTime / boostTimelength;
            yield return null;
        }
        boostsInfo.boostIcon[1].sprite = boostsInfo.boostSpriteDisabled[1];
        boostsInfo.boostTimeline[1].fillAmount = 0;
        BoostCorsInitialization();
    }

    private IEnumerator AttackspeedBoost()
    {
        boostsInfo.boostIcon[2].sprite = boostsInfo.boostSprite[2];
        float boostTimelength = 10;
        float s = 0;
        shootingCooldown = 0.85f;
        while (s < 1)
        {
            boostsInfo.boostTimeline[2].fillAmount = s;
            s += Time.deltaTime / boostTimelength;
            yield return null;
        }
        shootingCooldown = 1.5f;
        boostsInfo.boostIcon[2].sprite = boostsInfo.boostSpriteDisabled[2];
        boostsInfo.boostTimeline[2].fillAmount = 0;
        BoostCorsInitialization();
    }

    private IEnumerator DamageBoost()
    {
        boostsInfo.boostIcon[3].sprite = boostsInfo.boostSprite[3];
        float boostTimelength = 10;
        float s = 0;
        playerDamage = 2;
        while (s < 1)
        {
            boostsInfo.boostTimeline[3].fillAmount = s;
            s += Time.deltaTime / boostTimelength;
            yield return null;
        }
        playerDamage = 1;
        boostsInfo.boostIcon[3].sprite = boostsInfo.boostSpriteDisabled[3];
        boostsInfo.boostTimeline[3].fillAmount = 0;
        BoostCorsInitialization();
    }
}
