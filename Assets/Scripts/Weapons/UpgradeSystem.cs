using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{

    public List<int> arrowDamages; // Keeps track of the stats added to each bow level
    public List<float> arrowSpeeds;
    public List<float> drawSpeeds;



    // Start is called before the first frame update
    void Start()
    {
        arrowDamages = new List<int>  { 20, 30, 40, 41, 42, 45, 50, 60, 65, 70, 75 };
        arrowSpeeds = new List<float> { 0.7f, 0.7f, 0.8f, 0.8f, 0.8f, 0.8f, 0.9f, 0.9f, 0.9f, 0.9f, 0.9f };
    }

    // Update is called once per frame
    void Update()
    {
         
    }
}
