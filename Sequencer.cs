using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Sequencer : MonoBehaviour
{

    /// <summary>
    /// Sample for Practice
    /// </summary>
    /// 

    [System.Serializable]
    public class SquareBlinks ///Class to hold all of our information for each square
    {
        public GameObject square;
        public Material squareColor;
        public AudioSource play;
        public Text text;
    }

    public SquareBlinks[] squares; ///Calling our class info to an array
    public Material defaultColor;
    public float timeBtwSequence; //When player completes sequence 
    public bool spawnActive;

    public List<SquareBlinks> possibleSquareSequence = new List<SquareBlinks>(); //To hold all our squares in a list of possible hits for user
    public List<SquareBlinks> activeSequence = new List<SquareBlinks>(); //List to run the sequence and will continue to grow by one.

    private void Awake() 
    {
        for (int i = 0; i < squares.Length; i++) //Putting all our square information from an array to our possible sequence lists when the scene starts
        {
            possibleSquareSequence.Add(squares[i]);
        }
    }
    
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => spawnActive); // When space (debug) or timer hits zero

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) ///Debug Mode
        {
            spawnActive = true;
            StartCoroutine(runSequence());
        }
    }

    IEnumerator runSequence() //While spawnActive is true, the another Sequence is added.
    {
        yield return null;
        
        while (spawnActive)
        {
            yield return addNextSequence();
        }
    }

    IEnumerator addNextSequence()
    {
        
        yield return new WaitForSeconds(0.5f);
        spawnActive = false;
        var newSquare = possibleSquareSequence[Random.Range(0, squares.Length)];
        activeSequence.Add(newSquare);

        foreach (var squareObject in activeSequence)
        {
            yield return new WaitForSeconds(0.5f);
            print("Play this");
            squareObject.play.Play();
            squareObject.square.GetComponent<MeshRenderer>().material = squareObject.squareColor;
            yield return new WaitForSeconds(1f);
            squareObject.square.GetComponent<MeshRenderer>().material = defaultColor;
        }

        
    }

    
}
