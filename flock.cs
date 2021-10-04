using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flock : MonoBehaviour
{
    public globalFlock myManager;
    public float speed = 2.0f;
    float rotationSpeed = 4.0f;
    Vector3 averageHeading;
    Vector3 averagePosition;
    float neighbourDistance = 2.5f;

    bool turning = false;

    public float speedMult = 1;


    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(5.0f,10);
    }
    //Avoid collisions
    void OnTriggerEnter(Collider other)
    {
        if(!turning)
        {
            myManager.goalPos = this.transform.position - other.gameObject.transform.position;
        }
        turning = true;
    }
    void OnTriggerExit(Collider other)
    {
        turning = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        //create a bounding box to the fish to swim in

        Bounds b = new Bounds(myManager.transform.position, myManager.swimLimits * 10);
        if(!b.Contains(transform.position))
        {
            turning = true;
        }
        else
        {
            turning = false;
        }
       if(turning)
        {
            Vector3 direction = myManager.transform.position -transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                 Quaternion.LookRotation(direction),
                                                 rotationSpeed * Time.deltaTime);

            speed = Random.RandomRange(5.0f,10) *speedMult;
        }
        else
        {

            if (Random.Range(0, 5) < 1)
                ApplyRules();
        }
        transform.Translate(-Time.deltaTime * speed * speedMult,0, 0);
    }
    void ApplyRules()
    {
        GameObject[] gos;
        gos = myManager.allFish;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gspeed = 0.01f;

        Vector3 goalPos = myManager.goalPos;

        float dist;

        int groupSize = 0;
        foreach (GameObject go in gos)
        {
            if(go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if(dist <= neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;



                    if(dist< 1.5f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    flock anotherFlock = go.GetComponent<flock>();
                    gspeed = gspeed + anotherFlock.speed;
                }
            }
        }
        if(groupSize > 0)
        {
            vcentre = vcentre / groupSize + (goalPos - this.transform.position);
            speed = gspeed / groupSize * speedMult;

            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                    Quaternion.LookRotation(direction),
                                                    rotationSpeed * Time.deltaTime);
        }
    }
}
