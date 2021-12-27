using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class StartingLearning : Agent
{
    //private GameObject exit;
    private Vector2 initialPos;
    private Rigidbody2D playerRigid;
    private PlayerMovement playerMove;
    private int horInt, verInt, numCoins, lastHealth, collFactor;
    public GameObject currentTarget;
    private float prevDist;

    public Tile tileToPlace;
    private int blockPos;
    public Tilemap groundMap;

    public GameObject[] progressCoins;

    public override void Initialize()
    {
        //exit = GameObject.FindGameObjectWithTag("Goal");
        initialPos = transform.position;
        playerRigid = GetComponent<Rigidbody2D>();
        playerMove = GetComponent<PlayerMovement>();
        blockPos = 0;
        lastHealth = 0;
        collFactor = 0;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = initialPos;
        GetComponent<SpriteRenderer>().flipX = false;
        GetComponent<PlayerController>().health = 3;
        GetComponent<SpriteRenderer>().color = Color.green;
        prevDist = Vector3.Distance(transform.localPosition, currentTarget.transform.localPosition);
        numCoins = 0;

        for (int i = 0; i < progressCoins.Length; i++)
        {
            progressCoins[i].SetActive(true);
        }


        for (int i = 0; i < 3; i++)
        {
            Vector3Int cellPos = new Vector3Int((-37 + i), -3, 0);
            if (groundMap.HasTile(cellPos))
            {
                groundMap.SetTile(cellPos, null);
            }
        }

        blockPos = Random.Range(0, 4);

        switch (blockPos)
        {

            case 1:
                groundMap.SetTile(new Vector3Int(-37, -3, 0), tileToPlace);
                break;

            case 2:
                groundMap.SetTile(new Vector3Int(-36, -3, 0), tileToPlace);
                break;

            case 3:
                groundMap.SetTile(new Vector3Int(-35, -3, 0), tileToPlace);
                break;

            default:
                break;

        }
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(GetComponent<PlayerController>().health);
        sensor.AddObservation(currentTarget.transform.localPosition);
        sensor.AddObservation(Vector3.Distance(transform.localPosition, currentTarget.transform.localPosition));
        sensor.AddObservation(System.Convert.ToInt32(playerMove.pressedRun));

        //sensor.AddObservation(System.Convert.ToInt32(Input.GetButton("Run")));
    }

    //public override void Heuristic(in ActionBuffers actionsOut)
    //{
    //    ActionSegment<float> continuousActionsOut = actionsOut.ContinuousActions;
    //    continuousActionsOut[0] = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
    //    continuousActionsOut[1] = System.Convert.ToInt32(Input.GetButton("Jump"));
    //}

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discActionsOut = actionsOut.DiscreteActions;
        discActionsOut[0] = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        discActionsOut[1] = System.Convert.ToInt32(Input.GetButton("Jump"));
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 controlSignal = Vector3.zero;
        //controlSignal.x = actionBuffers.ContinuousActions[0];
        //controlSignal.y = actionBuffers.ContinuousActions[1];

        //playerMove.horizontal = actionBuffers.ContinuousActions[0];
        //playerMove.pressedJump = actionBuffers.ContinuousActions[1] == 1 ? true : false;

        playerMove.horizontal = actionBuffers.DiscreteActions[0] <= 1 ? actionBuffers.DiscreteActions[0] : -1;
        playerMove.pressedJump = actionBuffers.DiscreteActions[1] == 1 ? true : false;
        playerMove.pressedRun = actionBuffers.DiscreteActions[2] == 1 ? true : false;

        if (prevDist > Vector3.Distance(transform.localPosition, currentTarget.transform.localPosition))
        {
            AddReward(1/1000000);
        }

        else {AddReward(-1 / 1000000);}

        //RaycastHit2D leftCast = Physics2D.Raycast(
        //          playerMove.playerBox.bounds.center - new Vector3(playerMove.playerBox.bounds.extents.x, 0)
        //          , Vector2.down, playerMove.playerBox.bounds.extents.y + 0.1f, playerMove.groundLayerMask);

        //RaycastHit2D rightCast = Physics2D.Raycast(
        //    playerMove.playerBox.bounds.center + new Vector3(playerMove.playerBox.bounds.extents.x, 0)
        //    , Vector2.down, playerMove.playerBox.bounds.extents.y + 0.1f, playerMove.groundLayerMask);


        //RaycastHit2D sideHit = Physics2D.Raycast(
        //    playerMove.playerBox.bounds.center,
        //    new Vector2(playerMove.playerXDir, 0), playerMove.playerBox.bounds.extents.x + 0.025f, playerMove.groundLayerMask);

        //if ((leftCast.collider == null && rightCast.collider != null) || (leftCast.collider != null && rightCast.collider == null)
        //    || (sideHit.collider != null))
        //{
        //    Debug.Log("Hanging off edge");
        //    playerMove.pressedJump = true;
        //}


        if (lastHealth > GetComponent<PlayerController>().health)
        {
            AddReward(-0.3f);
        }

        lastHealth = GetComponent<PlayerController>().health;

        if (GetComponent<PlayerController>().health <= 0)
        {
            AddReward(-1f);
            EndEpisode();
        }


        switch (collFactor)
        {
            case 1:
                AddReward(1f);
                EndEpisode();
                break;

            case 2:
                AddReward(0.1f);
                break;

            case 3:
                AddReward(-1f);
                EndEpisode();
                break;

            default:
                break;
        }
        collFactor = 0;

        AddReward(1 / MaxStep);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DeathPlane")
        {
            collFactor = 3;
        }

        else if(collision.gameObject.tag == "Collectable")
        {
            collFactor = 2;
        }

        else if (collision.gameObject.tag == "GetThis")
        {
            collFactor = 1;
            collision.gameObject.SetActive(false);
        }
    }
}