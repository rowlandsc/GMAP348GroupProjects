using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    string player;

    Transform cPosition;
    Transform hold;
    public int blockSize = 1;
    public string facing = "up";
	// Use this for initialization
	void Start ()
    {
	    if (gameObject.tag == "Player1")
        {
            player = "p1";
        }
        else
        {
            player = "p2";
        }

        
        cPosition = gameObject.transform;
        hold = transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Move();
	}

    public void Move()
    {
        if (player == "p1")
        {
            Debug.Log("p1");
            if (Input.GetKeyDown(KeyCode.W) && hold.transform == cPosition)
            {
                if (facing == "up")
                {
                    cPosition.position = new Vector3(cPosition.position.x, cPosition.position.y + blockSize, cPosition.position.z);
                }
                else
                {
                    cPosition.rotation = Quaternion.Euler(0, 0, 0);
                    facing = "up";
                }
                
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (facing == "left")
                {
                    cPosition.position = new Vector3(cPosition.position.x - blockSize, cPosition.position.y, cPosition.position.z);
                }
                else
                {
                    cPosition.rotation = Quaternion.Euler(0, 0, 90);
                    facing = "left";
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (facing == "down")
                {
                    cPosition.position = new Vector3(cPosition.position.x, cPosition.position.y - blockSize, cPosition.position.z);
                }
                else
                {
                    cPosition.rotation = Quaternion.Euler(0, 0, 180);
                    facing = "down";
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (facing == "right")
                {
                    cPosition.position = new Vector3(cPosition.position.x + blockSize, cPosition.position.y, cPosition.position.z);
                }
                else
                {
                    cPosition.rotation = Quaternion.Euler(0, 0, 270);
                    facing = "right";
                }
            }
        }

        if (player == "p2")
        {
            Debug.Log("p1");
            if (Input.GetKeyDown(KeyCode.UpArrow) && hold.transform == cPosition)
            {
                if (facing == "up")
                {
                    cPosition.position = new Vector3(cPosition.position.x, cPosition.position.y + blockSize, cPosition.position.z);
                }
                else
                {
                    cPosition.rotation = Quaternion.Euler(0, 0, 0);
                    facing = "up";
                }

            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (facing == "left")
                {
                    cPosition.position = new Vector3(cPosition.position.x - blockSize, cPosition.position.y, cPosition.position.z);
                }
                else
                {
                    cPosition.rotation = Quaternion.Euler(0, 0, 90);
                    facing = "left";
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (facing == "down")
                {
                    cPosition.position = new Vector3(cPosition.position.x, cPosition.position.y - blockSize, cPosition.position.z);
                }
                else
                {
                    cPosition.rotation = Quaternion.Euler(0, 0, 180);
                    facing = "down";
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (facing == "right")
                {
                    cPosition.position = new Vector3(cPosition.position.x + blockSize, cPosition.position.y, cPosition.position.z);
                }
                else
                {
                    cPosition.rotation = Quaternion.Euler(0, 0, 270);
                    facing = "right";
                }
            }
        }
    }
}
