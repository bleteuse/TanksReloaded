using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;   //energie de début       
    public Slider m_Slider;     //référence au Slider                   
    public Image m_FillImage;  // Nous cherchons à modifier la couleur de l'image qui doit être renseignée dans cet attribut                    
	public Color m_FullHealthColor = Color.green; //Couleur lorsque l'énergie est égale à 100   
    public Color m_ZeroHealthColor = Color.red;  //Couleur lorsque l'énergie est égale à 0   
    public GameObject m_ExplosionPrefab; //référence au Prefab d'explosion
    
    
    private AudioSource m_ExplosionAudio; //reference à l'audio source du TankExplosion          
    private ParticleSystem m_ExplosionParticles; //reference au système de particule   
    private float m_CurrentHealth; //reference à l'état d'énergie actuel 
    private bool m_Dead;   // Tank est mort ou pas ?         


    private void Awake()
    {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>(); //Instantier le systeme de particule du prefab
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>(); //réference au composant audio du système de particules.

        m_ExplosionParticles.gameObject.SetActive(false); //Désactiver le GameObject pour le moment
    }


    private void OnEnable() //Lorsque le Tank est activé
    {
        m_CurrentHealth = m_StartingHealth; //Initialiser le niveau d'énergie à 100
        m_Dead = false; //Le Tank n'est pas mort

        SetHealthUI(); //Ajuster la valeur et la couleur du Slider
    }
    

    public void TakeDamage(float amount) //L'argument amount peut être variable. Par exemple si l'obus est trop prêt alors le dégat est plus conséquent que
	//si l'obus est plus loin.
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
		m_CurrentHealth-= amount; //On met à jour l'énergie du Tank mais on est pas entrain de déssiner
		SetHealthUI (); //On met à jour le dessin du Slider
		if (m_CurrentHealth <= 0f && !m_Dead) { //Si l'énergie est inférieure ou égale à 0 et on n'est pas déjà mort
			OnDeath ();
		}
    }


    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
		m_Slider.value = m_CurrentHealth;
		m_FillImage.color = Color.Lerp (m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth); //le troisième argument est une sorte
		//de pourcentage qui reflète le pourcentage de niveau d'enérgie qu'on possède.
    }


    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
		m_Dead = true;
		m_ExplosionParticles.transform.position = transform.position; //Placer le GameObject des particles à la position du Tank
		m_ExplosionParticles.gameObject.SetActive (true);//Activer le gameObject parce qu'il a été désactivé au début

		m_ExplosionParticles.Play (); //jouer le mouvement des particules
		m_ExplosionAudio.Play (); //jouer le son de l'explosion

		gameObject.SetActive(false); //désactiver le Tank.
    }
}