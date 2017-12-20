using UnityEngine;

public class ballThrow : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform ballSpawn;


    public void Fire()
    {
		Debug.Log("FIRE FIRE FIRE");
        // Create the Bullet from the Bullet Prefab
        var ball = (GameObject)Instantiate(
            ballPrefab,
            ballSpawn.position,
            ballSpawn.rotation);

        // Add velocity to the bullet
        ball.GetComponent<Rigidbody>().velocity = ball.transform.forward * 6;

        // Destroy the bullet after 2 seconds
        Destroy(ball, 2.0f);        
    }

}