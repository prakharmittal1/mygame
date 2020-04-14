using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDollarGestureRecognizer;

public class Recognizer : MonoBehaviour
{
    public Vector3 hiddenPosition;
    public GameObject player;
    public Rigidbody playerRigidBody;
    public Texture movementHalf;
    public Texture shootingHalf;
    public float movementSense;
    public float jumpSense;
    public GameObject bullet;
    public GameObject chargedShot;
    public GameObject throwObject;
    public GameObject movementDownGraphic;
    public GameObject movementDragGraphic;
    public GameObject shootingDownGraphic;
    public GameObject shootingDragGraphic;
    public GUIStyle labelStyle;

    private GestureData shootingGestureData;
    private GestureData movementGestureData;
    private List<GestureData> gestureList = new List<GestureData>();
    private List<PDollarGestureRecognizer.Point> gesturePoints = new List<PDollarGestureRecognizer.Point>();
    private List<Gesture> trainingSet = new List<Gesture>();


    void Start()
    {
        shootingGestureData = new GestureData {
            downPoint = new Vector3(0, 0, 0),
            dragPoint = new Vector3(0, 0, 0),
            index = -1
        };

        movementGestureData = new GestureData {
            downPoint = new Vector3(0, 0, 0),
            dragPoint = new Vector3(0, 0, 0),
            index = -1
        };

        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
		foreach (TextAsset gestureXml in gesturesXml)
		    trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
    }

    void Update()
    {   
        int num = Input.touchCount;
        for(int i = 0; i < num; i++)
        {
            Touch touch = Input.GetTouch(i);
            TouchPhase phase = touch.phase;
            Vector3 temp = touch.position;

            if (phase == TouchPhase.Began)
            {
                if(temp.y + 20 < Screen.height / 2)
                {
                    movementGestureData.downPoint.x = temp.x;
                    movementGestureData.downPoint.y = temp.y;
                    movementGestureData.dragPoint.x = temp.x;
                    movementGestureData.dragPoint.y = temp.y;
                    movementGestureData.gesturePoints.Add(new Point(temp.x,temp.y,1));
                    movementGestureData.index = i;
                }
                else
                {
                    shootingGestureData.downPoint.x = temp.x;
                    shootingGestureData.downPoint.y = temp.y;
                    shootingGestureData.dragPoint.x = temp.x;
                    shootingGestureData.dragPoint.y = temp.y;
                    shootingGestureData.gesturePoints.Add(new Point(temp.x,temp.y,1));
                    shootingGestureData.index = i;
                }
            }
            else if(phase == TouchPhase.Ended)
            {
                //if it was a shooting gesture
                if(i == shootingGestureData.index)
                {
                    // The tap down was on the player: Throw 
                    if(Helper.Distance(shootingGestureData.downPoint.x, shootingGestureData.downPoint.y, Screen.width / 2, Screen.height / 2) < 200)
                    {
                        Point p = shootingGestureData.gesturePoints[shootingGestureData.gesturePoints.Count - 1];
                        float xForce = Helper.GetXForce(p.X, p.Y, Screen.width / 2, Screen.height / 2, 1.5f * Helper.Distance(p.X, p.Y, Screen.width / 2, Screen.height /2));
                        float yForce = Helper.GetYForce(p.X, p.Y, Screen.width / 2, Screen.height / 2, 1.5f * Helper.Distance(p.X, p.Y, Screen.width / 2, Screen.height /2));
                        xForce *= -1.0f;
                        yForce *= -1.0f;
                        var clone = Instantiate(throwObject, new Vector3(player.transform.position.x, player.transform.position.y + 1, 1), new Quaternion(0, 0, 0, 1));
                        clone.GetComponent<Rigidbody>().AddForce(new Vector3(xForce, yForce, 0));
                    }

                    //long hold: charged shot
                    else if(shootingGestureData.gesturePoints.Count > 30)
                    {
                        Point p = shootingGestureData.gesturePoints[shootingGestureData.gesturePoints.Count - 1];
                        float xForce = Helper.GetXForce(p.X, p.Y, Screen.width / 2, Screen.height /2, 600.0f);
                        float yForce = Helper.GetYForce(p.X, p.Y, Screen.width / 2, Screen.height /2, 600.0f);
                        var clone = Instantiate(chargedShot, new Vector3(player.transform.position.x, player.transform.position.y + 1, 1), new Quaternion(0, 0, 0, 1));
                        clone.GetComponent<Rigidbody>().AddForce(new Vector3(xForce, yForce, 0));
                    }
            
                    //quick shot
                    else
                    {
                        if(shootingGestureData.downPoint.x < Screen.width / 2)
                        {
                            var c = Instantiate(bullet, new Vector3(player.transform.position.x - 1, player.transform.position.y, 1), new Quaternion(0, 0, 0, 1));
                            c.GetComponent<Rigidbody>().AddForce(Vector3.left * 400.0f);
                        }
                        else
                        {
                            var c = Instantiate(bullet, new Vector3(player.transform.position.x + 1, player.transform.position.y, 1), new Quaternion(0, 0, 0, 1));
                            c.GetComponent<Rigidbody>().AddForce(Vector3.right * 400.0f);
                        }
                    }

                    //check for multishot (only if they made a gesture)
                    if(Helper.Distance(shootingGestureData.dragPoint.x, shootingGestureData.dragPoint.y, shootingGestureData.downPoint.x, shootingGestureData.downPoint.y) > 0)
                    {
                        Gesture candidate = new Gesture(shootingGestureData.gesturePoints.ToArray());
                        Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
                        if (gestureResult.GestureClass == "D")
                        {
                            var c1 = Instantiate(chargedShot, new Vector3(player.transform.position.x - 1, player.transform.position.y, 1), new Quaternion(0, 0, 0, 1));
                            c1.GetComponent<Rigidbody>().AddForce(Vector3.left * 400.0f);
                            var c2 = Instantiate(chargedShot, new Vector3(player.transform.position.x + 1, player.transform.position.y, 1), new Quaternion(0, 0, 0, 1));
                            c2.GetComponent<Rigidbody>().AddForce(Vector3.right * 400.0f);
                        }
                    }


                    shootingGestureData.index = -1;
                    shootingGestureData.gesturePoints.Clear();
                    if(movementGestureData.index != -1)
                        movementGestureData.index = 0;
                }

                //it was movement
                else if(i == movementGestureData.index)
                {
                    if(Mathf.Abs(movementGestureData.downPoint.y - movementGestureData.dragPoint.y) > Mathf.Abs(movementGestureData.downPoint.x - movementGestureData.dragPoint.x))
                    {
                        float xForce = Helper.GetXForce(movementGestureData.downPoint.x, movementGestureData.downPoint.y, movementGestureData.dragPoint.x, movementGestureData.dragPoint.y, 350.0f);
                        float yForce = Helper.GetYForce(movementGestureData.downPoint.x, movementGestureData.downPoint.y, movementGestureData.dragPoint.x, movementGestureData.dragPoint.y, 350.0f);
                        xForce *= -1.0f;
                        yForce *= -1.0f;
                        playerRigidBody.AddForce(new Vector3(xForce, yForce, 0));
                    }

                    movementGestureData.index = -1;
                    movementGestureData.gesturePoints.Clear();
                    if(shootingGestureData.index != -1)
                        shootingGestureData.index = 0;
                }
                else
                {
                    Debug.Log("index not found");
                }
            }

            /* Drag */
            else
            {
                if(i == shootingGestureData.index)
                {
                    shootingGestureData.gesturePoints.Add(new Point(temp.x, temp.y, shootingGestureData.index));
                    shootingGestureData.dragPoint = new Vector3(temp.x, temp.y, 1);
                }
                else if(i == movementGestureData.index)
                {
                    movementGestureData.gesturePoints.Add(new Point(temp.x, temp.y, shootingGestureData.index));
                    movementGestureData.dragPoint = new Vector3(temp.x, temp.y, 1);

                    float xMag = Mathf.Abs(movementGestureData.dragPoint.x - movementGestureData.downPoint.x);
                    float yMag = Mathf.Abs(movementGestureData.dragPoint.y - movementGestureData.downPoint.y);

                    if (xMag > movementSense && xMag > yMag)
                    {
                        if(movementGestureData.dragPoint.x > movementGestureData.downPoint.x)
                            player.transform.position += new Vector3(0.15f,0,0);
                        else
                            player.transform.position -= new Vector3(0.15f,0,0);
                    }
                }
                else
                {
                    Debug.Log("index not found");
                }
            }
        }

        Vector3 temp1 = Camera.main.ScreenToWorldPoint(shootingGestureData.downPoint);
        temp1.z = 1;
        Vector3 temp2 = Camera.main.ScreenToWorldPoint(shootingGestureData.dragPoint);
        temp2.z = 1;
        Vector3 temp3 = Camera.main.ScreenToWorldPoint(movementGestureData.downPoint);
        temp3.z = 1;
        Vector3 temp4 = Camera.main.ScreenToWorldPoint(movementGestureData.dragPoint);
        temp4.z = 1;

        if(shootingGestureData.index != -1)
        {
            shootingDownGraphic.transform.position = temp1;
            shootingDragGraphic.transform.position = temp2;
        }
        else
        {
            shootingDownGraphic.transform.position = hiddenPosition;
            shootingDragGraphic.transform.position = hiddenPosition;
        }

        if(movementGestureData.index != -1)
        {
            movementDownGraphic.transform.position = temp3;
            movementDragGraphic.transform.position = temp4;
        }
        else
        {
            movementDownGraphic.transform.position = hiddenPosition;
            movementDragGraphic.transform.position = hiddenPosition;
        }
    }
}

class GestureData 
{
    public Vector3 downPoint;
    public Vector3 dragPoint;
    public int index;
    public List<PDollarGestureRecognizer.Point> gesturePoints = new List<PDollarGestureRecognizer.Point>();
}