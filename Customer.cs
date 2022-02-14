using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;


public class Customer : MonoBehaviour
{
    public float waitTime;
    public float maxWaitTime;
    public TextMeshProUGUI timeText;
    public Image paitenceBar;
    public Gradient waitColor;

    private serviceManager2 served;
    public GameObject plate;
    public GameObject timerStand;
    public bool activateTimer;

    public enum customerState {WalkingUp, BeingServed};
    public customerState currentCustomerState;
    float speed = 2;

    private globalOrder orderAssigned;
    public int whatOrder;
    public GameObject[] reactionBubbles;

    public Transform mySpot;

    public AudioClip Happy, Mad, Appear;
    private AudioSource audioSource;
    public bool customerAtSpotSound;
    public bool customerHappy;
    public bool customerSad;

    void Start()
    {
        served = GameObject.Find("Game Manager").GetComponent<serviceManager2>();
        orderAssigned = GetComponentInChildren<globalOrder>();
        waitTime = 60f;
        maxWaitTime = waitTime;
        activateTimer = false;
        plate.SetActive(false);
        timerStand.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        customerAtSpotSound = false;
        customerHappy = false;
        customerSad = false;
    }

    // Update is called once per frame
    void Update()
    {
        paitenceBar.fillAmount = waitTime / maxWaitTime;
        paitenceBar.color = waitColor.Evaluate(paitenceBar.fillAmount);
        timeText.text = waitTime.ToString("f0");
        transform.position = Vector3.MoveTowards(transform.position, mySpot.position, speed * Time.deltaTime);

        if (activateTimer == true)
        {
            waitTime -= Time.deltaTime;
        }

        if (waitTime <= 1)
        {
            activateTimer = false;
            waitTime = 0;
            CustomerSad();
            StartCoroutine(Rejected());
        }

        if(orderAssigned.orderFinished == true)
        {
            activateTimer = false;
            waitTime = waitTime;
            CustomerHappy();
            StartCoroutine(Served());
        }

        if (transform.position == mySpot.position)
        {
            plate.SetActive(true);
            timerStand.SetActive(true);
            activateTimer = true;
            orderAssigned.orderRequests[orderAssigned.randomAssigned].gameObject.SetActive(true);
            currentCustomerState = customerState.BeingServed;
            RingTheBell();
       }

    
        currentCustomerState = customerState.WalkingUp;

        if(GameManager.gameEnded == true)
        {
            Destroy(this.gameObject);
        }
        
    }

    void RingTheBell()
    {
        if (!customerAtSpotSound)
        {
            audioSource.PlayOneShot(Appear);
            customerAtSpotSound = true;
        }
    }

    void CustomerHappy()
    {
        if (!customerHappy)
        {
            audioSource.PlayOneShot(Happy);
            customerHappy = true;
        }
    }

    void CustomerSad()
    {
        if (!customerSad)
        {
            audioSource.PlayOneShot(Mad);
            customerSad = true;
        }
    }

    IEnumerator Served()
    {

        //GameObject happy = Instantiate(reactionParticles[1].gameObject, whereToReact.transform.position, whereToReact.transform.rotation);
        //Destroy(happy, .3f);
        reactionBubbles[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        served.CustomerServed(gameObject);
        GameManager.amountCustomersServed += 1;
        pattyScore.AddMoney(15);
        for (int i = 0; i < served.orderSpots.Length; i++)
        {
            if(served.orderSpots[i] == mySpot)
            {
                served.possibleOrderSpots.Add(served.orderSpots[i]);
            }
        }

        Destroy(gameObject);
    }

    IEnumerator Rejected()
    {

        //GameObject mad = Instantiate(reactionParticles[0].gameObject, whereToReact.transform.position, whereToReact.transform.rotation);
        //Destroy(mad, .3f); 
        reactionBubbles[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        served.CustomerRejected(gameObject);
        served.madCustomer();
        for (int i = 0; i < served.orderSpots.Length; i++)
        {
            if (served.orderSpots[i] == mySpot)
            {
                served.possibleOrderSpots.Add(served.orderSpots[i]);
            }
        }

        Destroy(gameObject);
    }

        
  }

