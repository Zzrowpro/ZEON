using Unity.VisualScripting;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject[] myObstacles;
    public int amount;
    public GameObject [] inWorld;

    void Start()
    {
        //Spawner for obstacles
        for(int i = 0; amount > i; i++)
        {
            int randomIndex = Random.Range(0, myObstacles.Length);

            Vector2 randomSpawnPosition = new Vector2(Random.Range(-12,12), Random.Range(-6,7));

            GameObject Obstacle = Instantiate(myObstacles[randomIndex], randomSpawnPosition,Quaternion.identity );
        }   
    }

    void Update()
    {
        //Spawns more obstacles 
        inWorld = GameObject.FindGameObjectsWithTag("Obstacle");
        
        if(inWorld.Length <= amount)
        {
            int randomIndex = Random.Range(0, myObstacles.Length);  
            Vector2 randomSpawnPosition = new Vector2(Random.Range(-12,12), Random.Range(-6,7));
            Instantiate(myObstacles[randomIndex], randomSpawnPosition, Quaternion.identity);
        }
    } 
}
