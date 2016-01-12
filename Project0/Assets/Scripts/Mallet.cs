using UnityEngine;
using System.Collections;

public class Mallet : MonoBehaviour {
    public GameManager.Control Control;
    public GameManager.Side Side;
    public float Acceleration = 7;

	void Start () {
	
	}
	
	void Update () {
	    if (Control == GameManager.Control.PLAYER_ONE) {
            Vector2 move = new Vector2(Input.GetAxisRaw("P1MalletHorizontal"), Input.GetAxisRaw("P1MalletVertical")).normalized * Acceleration;
            GetComponent<Rigidbody2D>().AddForce(move, ForceMode2D.Force);
        }
        else if (Control == GameManager.Control.PLAYER_TWO) {
            Vector2 move = new Vector2(Input.GetAxisRaw("P2MalletHorizontal"), Input.GetAxisRaw("P2MalletVertical")).normalized * Acceleration;
            GetComponent<Rigidbody2D>().AddForce(move, ForceMode2D.Force);
        }
        else if (Control == GameManager.Control.AI) {
            if (GameManager.Instance.GamePuck) {
                GameManager.Quadrant q = GameManager.Instance.GetPuckQuadrant();

                if (q == GameManager.Quadrant.TOPRIGHT || q == GameManager.Quadrant.BOTTOMRIGHT) {
                    //transform.position = transform.position + (GameManager.Instance.GamePuck.transform.position - transform.position).normalized * MaxSpeed * Time.deltaTime;
                    Vector3 move = (GameManager.Instance.GamePuck.transform.position - transform.position).normalized * Acceleration;
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(move.x, move.y));

                }
                else {
                    Goal g = GameManager.Instance.GetCorrespondingGoal(this);
                    if (Side == GameManager.Side.RIGHT) {
                        Vector3 move = ((g.transform.position - new Vector3(2, 0, 0)) - transform.position).normalized * Acceleration;
                        GetComponent<Rigidbody2D>().AddForce(new Vector2(move.x, move.y));
                    }
                }
            }
        }
	}
    public void SetPlayerOne()
    {
        Control = GameManager.Control.PLAYER_ONE;
    }
    public void SetPlayerTwo()
    {
        Control = GameManager.Control.PLAYER_TWO;
    }
    public void SetAI()
    {
        Control = GameManager.Control.AI;
    }
}
