using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnvironment : MonoBehaviour
{
    [SerializeField] Transform checkpointsParent;

    public static GameEnvironment Instance;

    List<Transform> checkpoints = new List<Transform>();
    public List<Transform> Checkpoints => checkpoints;

    void InitializeCheckpoints()
    {
        foreach (Transform checkpoint in checkpointsParent)
            checkpoints.Add(checkpoint);
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this.gameObject);
        }
        InitializeCheckpoints();
    }
}
