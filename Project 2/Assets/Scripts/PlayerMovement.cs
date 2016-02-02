﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    string player;

    Transform cPosition;
    Transform hold;
    public int blockSize = 1;
    public string facing = "up";
    public int xLoc;
    public int yLoc;
    bool isValid = true;
    MapTile tile;
	// Use this for initialization
	void Start ()
    {
	    if (gameObject.tag == "Player1")
        {
            player = "p1";
            tile = Map.Instance.PlayerStartTiles[1];
            xLoc = tile.X;
            yLoc = tile.Y;
        }
        else
        {
            player = "p2";
            tile = Map.Instance.PlayerStartTiles[0];
            xLoc = tile.X;
            yLoc = tile.Y;
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
                if (facing == "up" && yLoc < Map.Instance.TileMap[0].Count-1)
                {
                    cPosition.position = new Vector3(cPosition.position.x, cPosition.position.y + blockSize, cPosition.position.z);
                    yLoc += 1;
                    Map.Instance.GetTile(xLoc, yLoc).Explore();
                }
                else
                {
                    cPosition.rotation = Quaternion.Euler(0, 0, 0);
                    facing = "up";
                }
                
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (facing == "left" && xLoc > 0)
                {
                    cPosition.position = new Vector3(cPosition.position.x - blockSize, cPosition.position.y, cPosition.position.z);
                    xLoc -= 1;
                    Map.Instance.GetTile(xLoc, yLoc).Explore();
                }
                else
                {
                    cPosition.rotation = Quaternion.Euler(0, 0, 90);
                    facing = "left";
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (facing == "down" && yLoc >0)
                {
                    cPosition.position = new Vector3(cPosition.position.x, cPosition.position.y - blockSize, cPosition.position.z);
                    yLoc -= 1;
                    Map.Instance.GetTile(xLoc, yLoc).Explore();
                }
                else
                {
                    cPosition.rotation = Quaternion.Euler(0, 0, 180);
                    facing = "down";
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (facing == "right" && xLoc < Map.Instance.TileMap.Count - 1)
                {
                    cPosition.position = new Vector3(cPosition.position.x + blockSize, cPosition.position.y, cPosition.position.z);
                    xLoc += 1;
                    Map.Instance.GetTile(xLoc, yLoc).Explore();
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
            Debug.Log("p2");
            if (Input.GetKeyDown(KeyCode.UpArrow) && hold.transform == cPosition)
            {
                if (facing == "up" && yLoc < Map.Instance.TileMap[0].Count - 1)
                {
                    cPosition.position = new Vector3(cPosition.position.x, cPosition.position.y + blockSize, cPosition.position.z);
                    yLoc += 1;
                    Map.Instance.GetTile(xLoc, yLoc).Explore();
                }
                else
                {
                    cPosition.rotation = Quaternion.Euler(0, 0, 0);
                    facing = "up";
                }

            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (facing == "left" && xLoc > 0)
                {
                    cPosition.position = new Vector3(cPosition.position.x - blockSize, cPosition.position.y, cPosition.position.z);
                    xLoc -= 1;
                    Map.Instance.GetTile(xLoc, yLoc).Explore();
                }
                else
                {
                    cPosition.rotation = Quaternion.Euler(0, 0, 90);
                    facing = "left";
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (facing == "down" && yLoc >0)
                {
                    cPosition.position = new Vector3(cPosition.position.x, cPosition.position.y - blockSize, cPosition.position.z);
                    yLoc -= 1;
                    Map.Instance.GetTile(xLoc, yLoc).Explore();
                }
                else
                {
                    cPosition.rotation = Quaternion.Euler(0, 0, 180);
                    facing = "down";
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (facing == "right" && xLoc < Map.Instance.TileMap.Count - 1)
                {
                    cPosition.position = new Vector3(cPosition.position.x + blockSize, cPosition.position.y, cPosition.position.z);
                    xLoc += 1;
                    Map.Instance.GetTile(xLoc, yLoc).Explore();
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
