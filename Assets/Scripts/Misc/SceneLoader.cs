using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    [SerializeField]
    public SimpleRandomWalkGenerator simpleRandomWalkGenerator;
    public string sceneName = "Lvl1"; 

    void Start()
    {
        simpleRandomWalkGenerator.RunProceduralGeneration();
        
    }


    
}
