﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class StartingLearning : Agent
{
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
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(GetComponent<PlayerController>().health);
        sensor.AddObservation(currentTarget.transform.localPosition);
        sensor.AddObservation(Vector3.Distance(transform.localPosition, currentTarget.transform.localPosition));
        sensor.AddObservation(System.Convert.ToInt32(playerMove.pressedRun));
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discActionsOut = actionsOut.DiscreteActions;
        discActionsOut[0] = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        discActionsOut[1] = System.Convert.ToInt32(Input.GetButton("Jump"));
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 controlSignal = Vector3.zero;
        playerMove.horizontal = actionBuffers.DiscreteActions[0] <= 1 ? actionBuffers.DiscreteActions[0] : -1;
        playerMove.pressedJump = actionBuffers.DiscreteActions[1] == 1 ? true : false;
        playerMove.pressedRun = actionBuffers.DiscreteActions[2] == 1 ? true : false;

        if (prevDist > Vector3.Distance(transform.localPosition, currentTarget.transform.localPosition))
        {
            AddReward(1/1000000);
        }
        else {AddReward(-1 / 1000000);}


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