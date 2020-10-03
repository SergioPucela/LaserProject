using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    public float updateFrequency = 0.1f;
    public int laserDistance;
    public string bounceTag;
    public string splitTag;
    public string doorHandlerTag;
    public int maxBounce;
    public int maxSplit;
    private float timer = 0;
    private LineRenderer mLineRenderer;

    private ObjectPooler objectPooler;
    private GameObject currentLaser;

    private MirrorRotation currentMirror;
    private Color doorHandlerColor;
    private DoorHandler doorHandlerScript;

    public LayerMask ignoreLayers;

    private Queue<GameObject> lasers = new Queue<GameObject>();

    // Use this for initialization
    public void Start()
    {
        timer = 0;
        objectPooler = ObjectPooler.Instance;
        DrawLaser();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer >= updateFrequency)
        {
            timer = 0;
            while (lasers.Count != 0)
            {
                lasers.Dequeue().SetActive(false);
            }
            DrawLaser();
        }
        timer += Time.fixedDeltaTime;
    }

    private void DrawLaser() //Instantiate various LineRenderers
    {
        bool loopActive = true; //Is the reflecting loop active?

        Vector3 laserDirection = transform.forward; //direction of the next laser

        Vector3 lastLaserPosition = transform.localPosition; //origin of the next laser

        Color nextColor = Color.white; //color of the next laser
        Color currentColor = Color.white; //color of the current laser

        RaycastHit hit;

        while (loopActive)
        {
            if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance, ignoreLayers))
            {
                currentColor = nextColor;
                createNewLaser(hit.point, lastLaserPosition, laserDirection, nextColor);               
                nextColor = hit.transform.gameObject.GetComponent<Renderer>().material.color;

                if (hit.transform.gameObject.CompareTag(bounceTag))
                {
                    currentMirror = hit.transform.gameObject.GetComponent<MirrorRotation>();

                    if (currentMirror != null && currentMirror.isFacingMirror(laserDistance, ignoreLayers)) //Two lasers face each other
                    {
                        mLineRenderer.startColor = currentColor;
                        mLineRenderer.endColor = nextColor;
                        loopActive = false;
                    }

                    lastLaserPosition = hit.point;
                    laserDirection = Vector3.Reflect(laserDirection, hit.normal);
                    nextColor = mixColors(currentColor, nextColor);
                }
                else if (hit.transform.gameObject.CompareTag(doorHandlerTag)) //Check if puzzle is solved
                {
                    checkPuzzleSolved(hit, currentColor);
                    loopActive = false;
                }
                else
                {
                    mLineRenderer.SetPosition(1, hit.point);
                    lastLaserPosition = transform.position;
                    if(doorHandlerScript != null)
                    {
                        doorHandlerScript.activateAnimation = false;
                    }

                    loopActive = false;
                }
            }
            else
            {
                createNewLaser(lastLaserPosition + laserDirection.normalized * laserDistance, lastLaserPosition, laserDirection, nextColor);
                if (doorHandlerScript != null)
                {
                    doorHandlerScript.activateAnimation = false;
                }

                loopActive = false;
            }
        }
    }

    private void createNewLaser(Vector3 point, Vector3 position, Vector3 rotation, Color color)
    {
        currentLaser = objectPooler.SpawnFromPool("newLaser", position, Quaternion.LookRotation(rotation));
        mLineRenderer = currentLaser.GetComponent<LineRenderer>();

        lasers.Enqueue(currentLaser);

        mLineRenderer.startWidth = .07f;
        mLineRenderer.endWidth = .07f;
        mLineRenderer.SetPosition(0, position);
        mLineRenderer.SetPosition(1, point);

        mLineRenderer.startColor = color;
        mLineRenderer.endColor = color;
    }

    private void checkPuzzleSolved(RaycastHit hit, Color compareColor)
    {
        doorHandlerColor = hit.transform.gameObject.GetComponent<Renderer>().material.color;
        doorHandlerScript = hit.transform.gameObject.GetComponent<DoorHandler>();

        if (doorHandlerColor == compareColor)
        {
            doorHandlerScript.activateAnimation = true;
        }
    }

    private Color mixColors(Color color1, Color color2)
    {
        Color finalColor = Color.white;

        if(color1 == color2) //Same color
        {
            finalColor = color1;
        }
        else if(color1 == Color.white) //White + any = any
        {
            finalColor = color2;
        }
        else if(color1 == Color.red) //Red + blue = magenta     Red + green = yellow
        {
            if(color2 == Color.blue)
            {
                finalColor = Color.magenta;
            }
            else if(color2 == Color.green)
            {
                finalColor = Color.yellow;
            }
        }
        else if(color1 == Color.blue) //Blue + red = magenta    Blue + green = cyan
        {
            if(color2 == Color.red)
            {
                finalColor = Color.magenta;
            }
            else if(color2 == Color.green)
            {
                finalColor = Color.cyan;
            }
        }
        else if(color1 == Color.green) //Green + blue = cyan     Green + red = yellow
        {
            if(color2 == Color.blue)
            {
                finalColor = Color.cyan;
            }
            else if(color2 == Color.red)
            {
                finalColor = Color.yellow;
            }
        }
        else if(color1 == Color.magenta) //Magena + yellow = red     Magenta + cyan = blue
        {
            if(color2 == Color.yellow)
            {
                finalColor = Color.red;
            }
            else if(color2 == Color.cyan)
            {
                finalColor = Color.blue;
            }
        }
        else if(color1 == Color.yellow) //Yellow + cyan = green      Yellow + magenta = red
        {
            if(color2 == Color.cyan)
            {
                finalColor = Color.green;
            }
            else if(color2 == Color.magenta)
            {
                finalColor = Color.red;
            }
        }
        else if(color1 == Color.cyan) //Cyan + magenta = blue       Cyan + yellow = green
        {
            if(color2 == Color.magenta)
            {
                finalColor = Color.blue;
            }
            else if(color2 == Color.yellow)
            {
                finalColor = Color.green;
            }
        }

        return finalColor;
    }

}