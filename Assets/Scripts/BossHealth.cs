using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class BossHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public AudioClip deathClip;
	private float falling;
	private Rigidbody rb;


    Animator anim;
    AudioSource playerAudio;
    Boss_KI bossKI;
	//GameObject blood;
    //PlayerShooting playerShooting;
    bool isDead;

	public GameObject InfoText;

	void Start() 
	{
		//blood = GameObject.FindGameObjectWithTag ("Blood");

	}
    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
       	bossKI = GetComponent <Boss_KI> ();

		rb = GetComponent<Rigidbody> ();

		//blood.SetActive (false);
        currentHealth = startingHealth;

		falling = 0f;
    }


    void Update ()
    {


		//Falling to the side 'animation' until 90 degrees
		if (isDead) {
			
			if (falling < 90f) {
				Vector3 oldRot = transform.rotation.eulerAngles;
				transform.rotation = Quaternion.Euler (oldRot.x, oldRot.y, falling);

				falling += 1.5f;

				//little boost from 65 degrees (exponential falling growth)

				if (falling > 60f) {
					falling += 5f;
				}
			} 
				
		}
    }


    public void TakeDamage (int amount)
    {

        currentHealth -= amount;

		//blood.SetActive (true);

        //healthSlider.value = currentHealth;

        //playerAudio.Play ();

        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }

	//STIMMT NOCH NICHT

	void OnCollisionStay(Collision other)
	{
		//if (other.gameObject == player && attacking == true) 
		//{
		//Vector3 oldRot = transform.rotation.eulerAngles;
		//Quaternion tempRot = Quaternion.Euler (0, oldRot.y, 0);
		//transform.rotation = Quaternion.Slerp (transform.rotation, tempRot, Time.deltaTime*2);
		//}
	}


    void Death ()
    {
        isDead = true;

        //playerAudio.clip = deathClip;
        //playerAudio.Play ();

		rb.isKinematic = true;

		rb.freezeRotation = true;

		anim.enabled = false;

		bossKI.enabled = false;
		InfoText.SetActive (true);

		Text tempText = InfoText.GetComponent<Text> ();

		tempText.text = "YOU WON DUUUDE";



    }
		
}
