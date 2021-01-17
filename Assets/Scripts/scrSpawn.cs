using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrSpawn : MonoBehaviour
{
    //Get object game controller
    public GameObject gameControl;

    //Bools set by options screen
    public bool spawnerActive;
    public bool spawnBombs = true;

    //Adjustable variables
    [SerializeField]
    float bananaTimer;
    [SerializeField]
    float bombTimer;
    [SerializeField]
    GameObject Banana;
    [SerializeField]
    GameObject Bomb;

    // Update is called once per frame
    void Update() {
        if (spawnerActive == true) {
            //Spawn Bananas
            bananaTimer += Time.deltaTime;
            bombTimer += Time.deltaTime;
            if (bananaTimer >= 2) {
                for (int i = Random.Range(0, 3); i < 3; i++) {
                    var banana = Instantiate(Banana, new Vector3(Random.Range(-9, 10), 50, Random.Range(-9, 10)), Quaternion.identity);
                    banana.GetComponent<scrBanana>().gameControler = gameControl;
                    bananaTimer = 0;
                }
            }
            //Spawn Bombs if allowed
            if (bombTimer >= 8 && spawnBombs == true) {
                var bomb = Instantiate(Bomb, new Vector3(Random.Range(-9, 10), 50, Random.Range(-9, 10)), Quaternion.identity);
                bombTimer = 0;
            }
        }
    }        
}
