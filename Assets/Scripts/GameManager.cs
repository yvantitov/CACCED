    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Player player = new Player();
    public Battle currentBattle = new Battle(10, 10, 10, 50, 20, 20, false, false, false);

    public GameObject mapNodePrefab;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello World");
        GameObject a = Instantiate(mapNodePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject b = Instantiate(mapNodePrefab, new Vector3(3, 2, 0), Quaternion.identity);
        GameObject c = Instantiate(mapNodePrefab, new Vector3(3, 0, 0), Quaternion.identity);
        GameObject d = Instantiate(mapNodePrefab, new Vector3(-1, -1, 0), Quaternion.identity);
        a.GetComponent<MapNode>().Link(b.GetComponent<MapNode>());
        a.GetComponent <MapNode>().Link(c.GetComponent<MapNode>());
        d.GetComponent<MapNode>().Link(a.GetComponent<MapNode>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
