using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;   //Nous devons savoir quel joueur on va manipuler pour activer ses boutons correspondants. Par exemple Fire1 pour Player1...     
    public Rigidbody m_Shell; // reférence à l'obus qu'on va instancier.          
    public Transform m_FireTransform; // réference à l'endroit à partir duquel on va tirer l'obus   
    public Slider m_AimSlider; //référence au Slider pour l'étirer et le rétrécir           
    public AudioSource m_ShootingAudio; // réference à l'audio qui va être joué lors du tir  
    public AudioClip m_ChargingClip; //un audio clip pour charger     
    public AudioClip m_FireClip;// un audio clip pour tirer         
    public float m_MinLaunchForce = 15f; //Correspondent aux valeurs du Min Max du Slider
    public float m_MaxLaunchForce = 30f; 
    public float m_MaxChargeTime = 0.75f; //La temps maximal de chargement

    
    private string m_FireButton; // Le bouton de tir qui va être FireX. X=1,2,3... le numéro du joueur.         
    private float m_CurrentLaunchForce; //La valeur actuelle du chargement  
    private float m_ChargeSpeed; //la vitesse de transition entre min et max         
    private bool m_Fired; // un bouléen qui renseigne si l'obus a été tiré ou pas              


    private void OnEnable() //initialisation des valeurs
    {
        m_CurrentLaunchForce = m_MinLaunchForce; 
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber; //Calculer la valeur de Fire1 pour joueur1, Fire2 pour joueur2 ....

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime; //Calculer la vitesse = distance/temps
    }
    

    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
		m_AimSlider.value = m_MinLaunchForce; //on remet la valeur du slider à son minimum
		if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired) { //nous avons chargé mais pas encore tiré.
			m_CurrentLaunchForce = m_MaxLaunchForce; //On bloque la valeur maximale du tir sur la valeur max prédeterminée. 
			Fire (); //On tire
		} else if (Input.GetButtonDown (m_FireButton)) { //Vérifier si on a cliqué sur le bouton de tir pour la première fois
			m_Fired = false; //on n'a pas encore tiré
			m_CurrentLaunchForce = m_MinLaunchForce; //on commence par la valeur min
			m_ShootingAudio.clip = m_ChargingClip; //jouer le son du tir
			m_ShootingAudio.Play ();

		} else if (Input.GetButton (m_FireButton) && !m_Fired) { //Vérifier si le bouton est enfoncé mais on n'a pas encore tiré
			m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime; //incrémenter la force du tir mais on n'a pas encore mis à jour le Slider
			m_AimSlider.value = m_CurrentLaunchForce; //Mettre à jour la valeur du Slider.

		} else if (Input.GetButtonUp (m_FireButton) && !m_Fired) { //vérifier si on a relaché le bouton de tir mais on n'a pas encore tiré
			Fire(); //On tire 
		}
    }


    private void Fire() //On va instancier l'obus et le lancer suivant la valeur de tir qu'on determinée
    {
        // Instantiate and launch the shell.
		m_Fired = true ; //on a tiré
		Rigidbody shellInsatnce = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;//instancier l'obus à la position de l'objet de
		//tir ainsi qu'à sa rotation. L'objet concerné est le FireTransform. Le résultat de la fonction Instantiate est un objet on le convertit en Rigibody.
		shellInsatnce.velocity = m_CurrentLaunchForce * m_FireTransform.forward; //La vitesse de l'obus est égale à la force du tir multipliée par le vecteur
		//avant du GameObject FireTransform.

		m_ShootingAudio.clip = m_FireClip; //jouer le son du tir
		m_ShootingAudio.Play ();

		m_CurrentLaunchForce = m_MinLaunchForce; //remettre à la valeur initiale la valeur de chargement
    }
}