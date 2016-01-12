using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
    public enum Control { PLAYER_ONE, PLAYER_TWO, AI, NULL};
    public enum Side { LEFT, RIGHT };
    public enum Quadrant { TOPRIGHT, TOPLEFT, BOTTOMLEFT, BOTTOMRIGHT }; 

    public int Player1Score = 0;
    public int Player2Score = 0;
    public Quadrant PuckQuadrant;

    public Puck GamePuck;
    public Goal Player1Goal;
    public Goal Player2Goal;
    public Mallet Player1Mallet;
    public Mallet Player2Mallet;
    public BoxCollider2D Player1Wall;
    public BoxCollider2D Player2Wall;

    public Text Player1ScoreText;
    public Text Player2ScoreText;

    public int p1scorestreak = 0;
    public int p2scorestreak = 0;

    public static GameManager Instance;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

	void Start () {
        Player1Mallet.Side = Side.LEFT;
        Player2Mallet.Side = Side.RIGHT;
	}
	
	void Update () {
        PuckQuadrant = GetPuckQuadrant();

        Player1ScoreText.text = Player1Score.ToString();
        Player2ScoreText.text = Player2Score.ToString();
    }

    public Quadrant GetPuckQuadrant() {
        if (GamePuck.GetComponent<Rigidbody2D>().position.x >= transform.position.x) {
            if (GamePuck.GetComponent<Rigidbody2D>().position.y >= transform.position.y) {
                return Quadrant.TOPRIGHT;
            }
            else {
                return Quadrant.BOTTOMRIGHT;
            }
        }
        else {
            if (GamePuck.GetComponent<Rigidbody2D>().position.y >= transform.position.y) {
                return Quadrant.TOPLEFT;
            }
            else {
                return Quadrant.BOTTOMLEFT;
            }
        }
    }

    public Mallet GetCorrespondingMallet(Goal g) {
        if (g == Player1Goal) return Player1Mallet;
        if (g == Player2Goal) return Player2Mallet;
        return null;
    }

    public Goal GetCorrespondingGoal(Mallet m) {
        if (m == Player1Mallet) return Player1Goal;
        if (m == Player2Mallet) return Player2Goal;
        return null;
    }

    public void EventGoalScored(Goal scoredOn) {
        if (scoredOn == Player1Goal)
        {
            Player2Score += 1;
            p1scorestreak = 0;

            int tmp = (int)Player2Goal.transform.localScale.y;
            if (tmp > 1)
            {
                tmp -= 1;
                Player2Goal.transform.localScale = new Vector3(1.5f, tmp, 1);
                Player1Goal.transform.localScale = new Vector3(1.5f, 15, 1);
            }
            int tmp2 = (int)Player2Mallet.transform.localScale.x;
            int tmp3 = (int)Player2Mallet.transform.localScale.y;

            tmp2 += 1;
            tmp3 += 1;
            Player2Mallet.transform.localScale = new Vector3(tmp2, tmp3, 1);
            Player1Mallet.transform.localScale = new Vector3(4, 4, 1);

            p2scorestreak += 1;
            if (p2scorestreak >= 2)
            {
                Player2Mallet.Acceleration = 14;
            }
            else
            {
                Player2Mallet.Acceleration = 7;
                Player2Goal.Acceleration = 10;
            }
        }
        if (scoredOn == Player2Goal)
        {
            Player1Score += 1;
            p2scorestreak = 0;
            int tmp = (int)Player1Goal.transform.localScale.y;
            if (tmp > 1)
            {
                tmp -= 1;
                Player1Goal.transform.localScale = new Vector3(1.5f, tmp, 1);
                Player2Goal.transform.localScale = new Vector3(1.5f, 15, 1);
            }
            int tmp2 = (int)Player1Mallet.transform.localScale.x;
            int tmp3 = (int)Player1Mallet.transform.localScale.y;

            tmp2 += 1;
            tmp3 += 1;
            Player1Mallet.transform.localScale = new Vector3(tmp2, tmp3, 1);
            Player2Mallet.transform.localScale = new Vector3(4, 4, 1);

            p1scorestreak += 1;
            if(p1scorestreak >= 2)
            {
                Player1Mallet.Acceleration = 14;
            }
            else
            {
                Player1Mallet.Acceleration = 7;
                Player1Goal.Acceleration = 10;
            }
        }
    }

    public void EventGoalWallHit()
    {

    }
}
