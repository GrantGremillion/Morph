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
        arrowDamages = new List<int>  { 1, 2, 2, 3, 4, 4, 5, 6, 6, 7, 7 };
        arrowSpeeds = new List<float> { 0.7f, 0.7f, 0.8f, 0.8f, 0.8f, 0.8f, 0.9f, 0.9f, 0.9f, 0.9f, 0.9f };
    }

    // Update is called once per frame
    void Update()
    {
         
    }
}
