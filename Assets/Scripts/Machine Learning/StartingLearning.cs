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
    private int horInt, verInt;
    public GameObject currentTarget;

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
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(GetComponent<PlayerController>().health);
        sensor.AddObservation(currentTarget.transform.localPosition);
    }
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        continuousActionsOut[1] = System.Convert.ToInt32(Input.GetButton("Jump"));
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.y = actionBuffers.ContinuousActions[1];

        playerMove.horizontal = actionBuffers.ContinuousActions[0];
        playerMove.pressedJump = actionBuffers.ContinuousActions[1] == 1 ? true : false;

        if (GetComponent<PlayerController>().health <= 0)
        {
            AddReward(-1f);
            EndEpisode();
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
            AddReward(1f);
            EndEpisode();
        }
    }
}