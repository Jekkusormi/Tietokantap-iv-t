using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombie;
    public float minX = -4;
    public float maxX = 4;
    public float currentTime;
    public float updateStateTime; // Päivitysajankohta sekunteina
    public string gameStateURL = "http://localhost/SurvivalGame/GameState.php"; // Yhteys PHP tiedostoon, joka ottaa yhteyttä tietokantaan ja palauttaa tietoa
    public int id; //SQL rivin numero
    public int nextCheck; // Sekunteina milloin seuraava tarkastus
    public int zombieInterval; // Kuinka usein instansioidaan uusi zombie sekunteina
    public int zombieAmount; // Kuinka monta zombieta tulee tietyn ajan välein
    public float zombieSpawnCounter; // Laskuri, milloin seuraavat zombiet instansioidaan
    public int currentLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.realtimeSinceStartup;

        if(currentTime > updateStateTime)
        {
            Debug.Log("Haetaan uudet infot tietokannasta");
            StartCoroutine(UpdateGameState(updateStateTime, currentLevel));
        }

        if(zombieSpawnCounter < zombieInterval)
        {
            zombieSpawnCounter += Time.deltaTime;
        }
        else
        {
            zombieSpawnCounter = 0;
            SpawnZombie(zombieAmount);
        }

    }

    IEnumerator UpdateGameState(float updateTime, int currentlvl)
    {
        // Otetaan webrequest yhteys php scriptiin
        UnityWebRequest hs_get = UnityWebRequest.Get(gameStateURL+"?nextTime="+updateTime+"&level="+currentlvl);
        // Odotetaan tässä vaiheessa niin kauan kunnes saadaan vastaus php scriptiltä
        yield return hs_get.SendWebRequest();

        if(hs_get.error != null )
        {
            Debug.Log("Virhe otettaessa yhteyttä php scriptiin" + hs_get.error);

        }
        else
        {
            Debug.Log("On yhteys tietokantaan");
            // Otetaan php scriptin luoma tekstinpätkä muuttujaan
            string dataText = hs_get.downloadHandler.text;
            Debug.Log(dataText);
            MatchCollection mc = Regex.Matches(dataText, @"_");
            Debug.Log(mc.Count);
            if(mc.Count > 0 )
            {
                // Datasta löytyy jotain tietoa jota voidaan käyttää
                string[] splitData = Regex.Split(dataText, @"_");
                // Data on eroteltu _ merkin mukaan ja arvot laitettu taulukkoon
                id = int.Parse(splitData[0]);
                nextCheck = int.Parse(splitData[1]);
                zombieInterval = int.Parse(splitData[2]);
                zombieAmount = int.Parse(splitData[3]);
                updateStateTime = nextCheck;
            }
        }

    }

    void SpawnZombie(int zombieAmt)
    {
        float zombieAmount = zombieAmt;
        
        for (int i = 0; i < zombieAmount; i++)
        {
            Vector3 zombiePos = new Vector3(Random.Range(minX, maxX), 1.5f, Random.Range(35, 45));
            Instantiate(zombie, zombiePos, Quaternion.identity);
        }
    }
}
