using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour {

	public Rigidbody projectile;
	Vector3 position;
	public int velocity;
	public Transform spawnPoint;
	// Use this for initialization
	void Start () 
	{
		position = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (position != transform.position)
		{
			shootSphere();
			position = transform.position;
		}

		transform.rotation = spawnPoint.rotation;

	}


	public void shootSphere()
	{
		Rigidbody clone;
		clone = Instantiate(projectile, transform.position, transform.rotation) as Rigidbody;
		//Physics.IgnoreCollision(clone.GetComponent<Collider>(), GetComponent<Collider>());
		clone.velocity = transform.TransformDirection(Vector3.forward * velocity);

	}


}
