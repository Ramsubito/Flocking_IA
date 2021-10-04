using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalFlock : MonoBehaviour
{
    public globalFlock myFlock;
    public GameObject fishPrefab;
   // public static float tankSize = 5;

    static int numFish = 120;
    public GameObject[] allFish = new GameObject[numFish];

    public Vector3 swimLimits = new Vector3(5, 5, 5);

    public Vector3 goalPos;

    public void FishSpeed(float speedMult)
    {
        Debug.Log(speedMult);
        for(int i = 0; i< numFish; i++)
        {
            allFish[i].GetComponent<flock>().speedMult = speedMult;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5F);
        Gizmos.DrawCube(transform.position, new Vector3(swimLimits.x * 2, swimLimits.y * 2, swimLimits.z * 2));
        Gizmos.color = new Color(0, 1, 0, 1);
        Gizmos.DrawSphere(goalPos, 0.1f);
    }
    // Start is called before the first frame update
    void Start()
    {
        myFlock = this;
        goalPos = this.transform.position;
        RenderSettings.fogColor = Camera.main.backgroundColor;
        RenderSettings.fogDensity = 0.012F;
        RenderSettings.fog = true;
        for(int i = 0; i < numFish; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                                Random.Range(-swimLimits.y, swimLimits.y),
                                                                Random.Range(-swimLimits.z, swimLimits.z));
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<flock>().myManager = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0,10000)<50)
        {
            goalPos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                     Random.Range(-swimLimits.y, swimLimits.y),
                                     Random.Range(-swimLimits.z, swimLimits.z));
        }
    }
}
