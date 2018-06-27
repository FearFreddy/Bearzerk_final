using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    private float startingHealth = 1000;
    private float currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.5f);
	private float falling;
	private Rigidbody rb;


    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
	FollowMom followMom;
    bool isDead;
    bool damaged;

	public GameObject InfoText;

	void Start() 
	{

	}
    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
		followMom = GetComponent <FollowMom> ();

		rb = GetComponent<Rigidbody> ();

        currentHealth = startingHealth;

		falling = 0f;
    }


    void Update ()
    {
		if (this.gameObject.tag == "Player") {

			if (currentHealth < 1000) {
				currentHealth += 0.02f * Time.deltaTime * 30;
				healthSlider.value = currentHealth;
			}

			if (damaged) {
				damageImage.color = flashColour;
			} else {
				damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
			}
			damaged = false;
		}


		//Falling to the side 'animation' until 90 degrees
		if (isDead) {

			InfoText.SetActive (true);

			Text tempText = InfoText.GetComponent<Text> ();

			tempText.text = "GAME OVER \n TRY AGAIN";
			
			if (falling < 90f) {
				Vector3 oldRot = transform.rotation.eulerAngles;
				transform.rotation = Quaternion.Euler (oldRot.x, oldRot.y, falling);

				falling += 2.5f;

				//little boost from 65 degrees (exponential falling growth)

				if (falling > 60f) {
					falling += 5f;
				}
			} 
				
		}
    }


    public void TakeDamage (int amount)
    {
        damaged = true;

        currentHealth -= amount;

		if (this.gameObject.tag == "Player") {
			healthSlider.value = currentHealth;
		}

        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }

	public void Heal(int amount) {

		currentHealth += amount;
		healthSlider.value = currentHealth;
	}


    void Death ()
    {
        isDead = true;

		rb.isKinematic = true;

		rb.freezeRotation = true;

		anim.enabled = false;

		if (this.gameObject.tag == "ChildBear") 
		{
			followMom.enabled = false;
		} 
		else if(this.gameObject.tag == "Player")
		{
			playerAudio.clip = deathClip;
			playerAudio.Play ();

			playerMovement.enabled = false;

			StartCoroutine (setRestart());
		}

    }


    public void RestartLevel ()
    {
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
    }

	IEnumerator setRestart() {
		yield return new WaitForSeconds(3);
		RestartLevel ();
	}
}
