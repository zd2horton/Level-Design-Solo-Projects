using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.SceneManagement;

public class StartingLearning : Agent
{
    private GameObject exit;
    private Vector2 initialPos;
    private Rigidbody2D playerRigid;
    private PlayerMovement playerMove;
    private int horInt, verInt, numCoins;
    public GameObject currentTarget;
    private float prevDist;
    private bool loseState, winState;

    public GameObject[] coins;

    public override void Initialize()
    {
        exit = GameObject.FindGameObjectWithTag("Goal");
        initialPos = transform.position;
        playerRigid = GetComponent<Rigidbody2D>();
        playerMove = GetComponent<PlayerMovement>();
    }

    public override void OnEpisodeBegin()
    {
        transform.position = initialPos;
        GetComponent<SpriteRenderer>().flipX = false;
        GetComponent<PlayerController>().health = 3;
        GetComponent<SpriteRenderer>().color = Color.green;
        prevDist = Vector3.Distance(transform.localPosition, currentTarget.transform.localPosition);
        numCoins = 0;
        winState = false;

        for (int i = 0; i < coins.Length; i++)
        {
            coins[i].SetActive(true);
        }

        loseState = false;
        winState = false;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(GetComponent<PlayerController>().health);
        sensor.AddObservation(currentTarget.transform.localPosition);
        sensor.AddObservation(Vector3.Distance(transform.localPosition, currentTarget.transform.localPosition));
        sensor.AddObservation(System.Convert.ToInt32(playerMove.pressedJump));
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

        //if (prevDist > Vector3.Distance(transform.localPosition, currentTarget.transform.localPosition))
        //{
        //    AddReward(0.0000001f);
        //}

        //else if (prevDist < Vector3.Distance(transform.localPosition, currentTarget.transform.localPosition))
        //{
        //    AddReward(-0.0000001f);
        //}

        if (GetComponent<PlayerController>().health <= 0)
        {
            loseState = true;
        }

        if (loseState == true)
        {
            AddReward(-1f);
            EndEpisode();
        }

        else if (winState == true)
        {
            AddReward(1f);
            //AddReward(0.5f);
            //numCoins++;


            //if (numCoins >= coins.Length)
            //{
                EndEpisode();
            //}

            //winState = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DeathPlane")
        {
            AddReward(-1f);
            EndEpisode();
        }

        else if (collision.gameObject.tag == "GetThis")
        {
            winState = true;
            //collision.gameObject.SetActive(false);

            //if (numCoins >= coins.Length)
            //{
            //    EndEpisode();
            //}
        }
    }
}