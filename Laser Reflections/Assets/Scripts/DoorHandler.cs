using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    public bool activateAnimation;
    public float timeForActivation;
    public float animationSpeed = 1.5f;

    public float posDoorOpen;
    public float posDoorClose;

    public List<DoorHandler> dependentDoorHandlers = new List<DoorHandler>(); //Door will open if all other handlers here are active

    public bool permaDoorOpen; //If true, door will be permanently active after it opens

    private float timer;

    //public Animator animator;
    public GameObject doorToOpen;

    void Start()
    {
        timer = 0;
    }

    void Update()
    {
        if (activateAnimation)
        {
            if (timer >= timeForActivation)
            {
                if(doorToOpen.transform.position.y < posDoorOpen)
                {
                    if(dependentDoorHandlers.Count > 0)
                    {
                        foreach (DoorHandler doorH in dependentDoorHandlers)
                        {
                            if (!doorH.activateAnimation)
                            {
                                return;
                            }
                        }
                    }
                    doorToOpen.transform.position = Vector3.Lerp(doorToOpen.transform.position, 
                        new Vector3(doorToOpen.transform.position.x, posDoorOpen, doorToOpen.transform.position.z), animationSpeed * Time.deltaTime);
                }
                //animator.SetBool("isDoorOpen", true);
            }
            timer += Time.deltaTime;
        }
        else
        {
            if (!permaDoorOpen)
            {
                if (doorToOpen.transform.position.y > posDoorClose)
                {
                    doorToOpen.transform.position = Vector3.Lerp(doorToOpen.transform.position,
                        new Vector3(doorToOpen.transform.position.x, posDoorClose, doorToOpen.transform.position.z), animationSpeed * Time.deltaTime);
                }
                //animator.SetBool("isDoorOpen", false);
            }
            resetTimer();
        }
    }

    public void resetTimer()
    {
        timer = 0;
    }


}
