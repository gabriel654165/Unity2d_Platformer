using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class History : MonoBehaviour
{
    [Header("Components")]
    public PLayer hero;

    [Header("Time")]
    public float timeRemaining = 20;

    [Header("History")]
    public Text history;
    private int lecture = 0;
    private string[] text = {
        "Hello !", "My name is Alexa, I'm the artificial inteligence of your starship.", "It seems that we arrive near to our destination : ", "The exoplanet 25468-id5551", "You are probably going to take the control of the starship, I'm not programmed to land proprely.", "Let's go on the control's room", 
        "We have a techincal problem...", "The reactors control dosen't response and we are going to enter in the atmosphere of the exoplanet 25468-id5551", "You need to repaer the controller manually in order to take the control !!", "Go see at level -1 of the starship for some tools",
        "Well it's perfect come to the conttrol pannel !",
        "Oh no we have another problem, the artificial gravity doesn't work well we are to fast !!",
        "WARNING : The starship is to heavy the reactors cannot stabilise the landing we are in a freefall", "You survival chances are 0.5%", "Good Bye"
    };
    private bool secPartHist = false;
    private bool thirdPartHist = false;

    [Header("Events")]
    public bool turnOffGravity = false;

    // Start is called before the first frame update
    void Start()
    {
        history.text = "Hello !";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (lecture >= 5) {
                if (hero.isCollidingEvent1) {
                    secPartHist = true;
                }
            }
            if (lecture >= 9) {
                if (hero.isCollidingEvent2) {
                   thirdPartHist = true;
                }
            }
            if (lecture < 5) {
                lecture += text[lecture] == "" ? 0 : 1;
                history.text = text[lecture];
            }
            if (lecture >= 5 && secPartHist && lecture < 9) {
                lecture += text[lecture] == "" ? 0 : 1;
                history.text = text[lecture];
            }
            if (lecture >= 9 && thirdPartHist && lecture < 11) {
                lecture += text[lecture] == "" ? 0 : 1;
                history.text = text[lecture];
            }
            if (lecture >= 11 && lecture <= 14) {
                lecture += text[lecture] == "" ? 0 : 1;
                history.text = text[lecture];
            }
        }
        if (lecture >= 11) {
                turnOffGravity = true;
                if (timeRemaining > 0)
                    timeRemaining -= Time.deltaTime;
                if (timeRemaining <= 0) {
                    SceneManager.LoadScene("exo_planet");
                }
        }
    }
}
//mettre un decompte en mode 3 2 1 end
//le niveau sur l'exoplanete la faire en mode mario faire un long niveau et mettre un timer
