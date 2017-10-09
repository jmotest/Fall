using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

	public static PlayerMovement playInst;

	public float moveSpeed = 3f;

	private Rigidbody2D theRigidbody;
	private Animator anim;
	private AudioSource aud;
	public int lives;
	public Text lifeText;
	public GameObject acorn;
	public GameObject bird;
	public GameObject spawnBird;
	public GameObject spawnAcorn;
	public Event birdTrigger;
	public Event acornTrigger;
	public int levelNo = 0;

	Player player = new Player ();

	Subject subject = new Subject();


	// Use this for initialization
	void Start () {
		theRigidbody = GetComponent<Rigidbody2D> ();
		theRigidbody.AddForce (new Vector2 (0, -200f));

		anim = GetComponent<Animator> ();
		aud = GetComponent<AudioSource> ();

		lives = 3;
		lifeText = GameObject.Find ("LivesText").GetComponent<Text>();
		Enemy enemy1 = new Enemy (spawnBird);
		Enemy enemy2 = new Enemy (spawnAcorn);

		player.AddObserver (enemy1);
		player.AddObserver (enemy2);
		Scene current = SceneManager.GetActiveScene();
		string sceneName = current.name;
		if (sceneName == "Fall1") {
			levelNo++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		float inputX = Input.GetAxis ("Horizontal");
		Debug.Log (inputX);
		theRigidbody.velocity = new Vector2 (inputX * moveSpeed, theRigidbody.velocity.y);
	}

	// Controls collision based events
	void OnTriggerEnter2D(Collider2D other) {
		//Death Code
		if (other.gameObject.layer == LayerMask.NameToLayer ("Death")) {
			if (lives > 1) {
				lives--;
				lifeText.text = "Hits: " + lives;
				aud.Play();
				Debug.Log ("Hit");
			} else {
				SceneManager.LoadScene("TitleScreen");
			}
		}
		//Level Loading
		if (other.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
			if (levelNo == 0) {
				SceneManager.LoadScene ("Fall1");
			} else {
				SceneManager.LoadScene ("TitleScreen");
			}
		}

		//Trigger First Projectile
		if (other.gameObject.layer == LayerMask.NameToLayer ("TriggerAcorn")) {
			//player.Notify (1, acorn);
			GameObject acornInstance = Instantiate(acorn, new Vector3(spawnAcorn.transform.position.x, spawnAcorn.transform.position.y + 10, spawnAcorn.transform.position.z), spawnAcorn.transform.rotation) as GameObject;
		}

		//Trigger Second Projectile
		if (other.gameObject.layer == LayerMask.NameToLayer ("TriggerBird")) {
			//player.Notify (2, bird);
			GameObject birdInstance = Instantiate(bird, new Vector3(spawnBird.transform.position.x + 30, spawnBird.transform.position.y - 20, spawnBird.transform.position.z), spawnBird.transform.rotation) as GameObject;
			birdInstance.GetComponent<Rigidbody2D>().AddForce (new Vector2 (-1000f, 1000f));
		}
	}

	void Inst(){
		playInst = this;
	}


}

//Observer Class Template
public abstract class Observer : PlayerMovement {
	public abstract void OnNotify(int e, GameObject p);
};

//Subject Class Template
public class Subject {
	List<Observer> observers = new List<Observer>();

	public void Notify(int e, GameObject p)
	{
		Debug.Log ("Notify: e=" + e);
		for (int i = 0; i < observers.Count; i++)
		{
			observers[i].OnNotify(e, p);
		}
	}

	public void AddObserver(Observer observer)
	{
		observers.Add (observer);
	}

	public void RemoveObserver(Observer observer)
	{
		observers.Remove (observer);
	}
};

//Enemy Observer Class for receiving trigger cues
public class Enemy : Observer {

	GameObject enemy;



	public Enemy(GameObject enemy) {
		this.enemy = enemy;
	}

	public static Enemy EnemyInst;

	void Inst(){
		EnemyInst = this;
	}

	public override void OnNotify (int e, GameObject p)
	{
		Debug.Log ("EnemyNotify: e=" + e);
		if (e == 2) {
			GameObject birdInstance = Instantiate(p, new Vector3(spawnBird.transform.position.x + 30, spawnBird.transform.position.y - 20, spawnBird.transform.position.z), spawnBird.transform.rotation) as GameObject;
			birdInstance.GetComponent<Rigidbody2D>().AddForce (new Vector2 (-1000f, 1000f));
		}

		if (e == 1) {
			GameObject acornInstance = Instantiate(p, new Vector3(spawnAcorn.transform.position.x, spawnAcorn.transform.position.y + 10, spawnAcorn.transform.position.z), spawnAcorn.transform.rotation) as GameObject;
		}
	}
};

//Subject Player Class for notifying observers
public class Player : Subject {

	public static Player PlayerInst;

	void Inst(){
		PlayerInst = this;
	}
		
};

