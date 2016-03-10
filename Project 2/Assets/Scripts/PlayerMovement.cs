using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    string player;
    public Text p1RespawnTime, p2RespawnTime;
    Transform cPosition;
    Transform hold;
    public int Score = 0;
    public int blockSize = 1;
    public string facing = "up";
    public int xLoc;
    public int yLoc;
    bool isValid = true;
    MapTile tile;
    public bool dead = false;
    public float respawnTimer = 3;
    public float currentTimer = 0;

    private SpriteRenderer _spriteRenderer = null;
    private Animator _animator = null;
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
        
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Move();
	}

    public void Move()
    {
        if (dead || GameManager.Instance.IsGameOver) return;

        if (player == "p1")
        {
            if (Input.GetKeyDown(KeyCode.W) && hold.transform == cPosition)
            {
                if (facing == "up" && yLoc < Map.Instance.TileMap[0].Count-1)
                {
                    cPosition.position = new Vector3(cPosition.position.x, cPosition.position.y + blockSize, cPosition.position.z);
                    yLoc += 1;
                    Map.Instance.GetTile(xLoc, yLoc).Explore(this);
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
                    Map.Instance.GetTile(xLoc, yLoc).Explore(this);
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
                    Map.Instance.GetTile(xLoc, yLoc).Explore(this);
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
                    Map.Instance.GetTile(xLoc, yLoc).Explore(this);
                }
                else
                {
                    cPosition.rotation = Quaternion.Euler(0, 0, 270);
                    facing = "right";
                }
            }

            if (Input.GetKeyUp(KeyCode.LeftShift)) {
                MapTile faceTile = null;
                switch(facing) {
                    case "up":
                        faceTile = Map.Instance.GetTile(xLoc, yLoc + 1);
                        break;
                    case "down":
                        faceTile = Map.Instance.GetTile(xLoc, yLoc - 1);
                        break;
                    case "right":
                        faceTile = Map.Instance.GetTile(xLoc + 1, yLoc);
                        break;
                    case "left":
                        faceTile = Map.Instance.GetTile(xLoc - 1, yLoc);
                        break;
                }
                if (faceTile) {
                    faceTile.Mark(true);
                }
            }
        }

        if (player == "p2")
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && hold.transform == cPosition)
            {
                if (facing == "up" && yLoc < Map.Instance.TileMap[0].Count - 1)
                {
                    cPosition.position = new Vector3(cPosition.position.x, cPosition.position.y + blockSize, cPosition.position.z);
                    yLoc += 1;
                    Map.Instance.GetTile(xLoc, yLoc).Explore(this);
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
                    Map.Instance.GetTile(xLoc, yLoc).Explore(this);
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
                    Map.Instance.GetTile(xLoc, yLoc).Explore(this);
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
                    Map.Instance.GetTile(xLoc, yLoc).Explore(this);

                }
                else
                {
                    cPosition.rotation = Quaternion.Euler(0, 0, 270);
                    facing = "right";
                }
            }

            if (Input.GetKeyUp(KeyCode.RightShift)) {
                MapTile faceTile = null;
                switch (facing) {
                    case "up":
                        faceTile = Map.Instance.GetTile(xLoc, yLoc + 1);
                        break;
                    case "down":
                        faceTile = Map.Instance.GetTile(xLoc, yLoc - 1);
                        break;
                    case "right":
                        faceTile = Map.Instance.GetTile(xLoc + 1, yLoc);
                        break;
                    case "left":
                        faceTile = Map.Instance.GetTile(xLoc - 1, yLoc);
                        break;
                }
                if (faceTile) {
                    faceTile.Mark(false);
                }
            }
        }
    }

    public void Kill() {
        dead = true;
        _spriteRenderer.enabled = false;
        _animator.enabled = false;
        currentTimer = respawnTimer;
        respawnTimer = respawnTimer * 1.5f;

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn() {
        while (currentTimer > 0) {
            currentTimer -= Time.deltaTime;
            if (gameObject.tag == "Player1")
            {
                p1RespawnTime.text = "Respawn: " + (int)currentTimer;
            }
            if (gameObject.tag == "Player2")
            {
                p2RespawnTime.text = "Respawn: " + (int)currentTimer;
            }
            yield return null;
        }

        dead = false;

        if (gameObject.tag == "Player1") {
            player = "p1";
            tile = Map.Instance.PlayerStartTiles[1];
            xLoc = tile.X;
            yLoc = tile.Y;
        }
        else {
            player = "p2";
            tile = Map.Instance.PlayerStartTiles[0];
            xLoc = tile.X;
            yLoc = tile.Y;
        }

        facing = "right";
        cPosition.rotation = Quaternion.Euler(0, 0, 270);
        cPosition.position = tile.transform.position;
        _spriteRenderer.enabled = true;
        _animator.enabled = true;
    }
}
