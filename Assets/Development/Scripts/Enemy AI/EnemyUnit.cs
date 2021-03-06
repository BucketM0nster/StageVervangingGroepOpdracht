﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 60;
    [SerializeField] private EnemyData enemyData;

    public int Health { get; private set; }
    public float Speed { get; private set; }
    public float GoldReward { get; private set; }
    public int AttackDamage { get; private set; }

    public event Action OnDeath;

    public Transform[] wayPoints;
    private int waypointIndex;

    public void Initialize( EnemyData e ) {
        this.Health = e.health;
        this.Speed = e.speed;
        this.GoldReward = e.goldReward;
        this.AttackDamage = e.attackDamage;
    }

    private void Start() {
        //wayPoints = FindObjectOfType<WaypointManager>();

        // The values can be decided here but we need to figure out what type of enemy unit we are first
        Initialize(enemyData);
    }

    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[waypointIndex].position, Speed * Time.deltaTime);

        // Need to test the rotation more
        Quaternion dir = Quaternion.LookRotation(wayPoints[waypointIndex].position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, dir, rotateSpeed * Time.deltaTime);

        //Vector3 dir = WayPointManager.waypoints[waypointIndex].position - transform.position;
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, Mathf.Atan2(dir.x, dir.y) / Mathf.PI * 180, 0), 0.1f);

        if ( Input.GetKeyDown(KeyCode.E) )
            TakeDamage(1);

        if ( slowDebuffActive ) {
            slowDebuffTimer += Time.deltaTime;

            if ( slowDebuffTimer > slowDebuffTime ) {
                Speed += slowDownSpeed;
                slowDebuffTimer = 0f;
                slowDebuffActive = false;
            }
        }

        // Test
        if ( Input.GetKeyDown(KeyCode.S) )
            SlowDown(1.5f, 4f);

        if ( Vector3.Distance(transform.position, wayPoints[waypointIndex].position) < .1f )
            if ( waypointIndex < wayPoints.Length - 1 ) {
                waypointIndex++;
            } else {
                Death();
                GameController.MainTowerHP -= AttackDamage;
                // Do damage to the main structure
            }
    }

    public void TakeDamage( int damage ) {
        Health -= damage;

        if ( Health < 1 )
            Death();
    }

    //bool takeDamageOTActive = false;
    //int takeDamageOT;
    //float takeDamageOTTime;
    //float takeDamageOTTimer = 0;
    //public void TakeDamageOverTime( int totalDamage, float time ) {
    //    takeDamageOT = totalDamage;
    //    takeDamageOTTime = time;

    //    takeDamageOTActive = true;
    //}

    bool slowDebuffActive = false;
    float slowDownSpeed;
    float slowDebuffTime;
    float slowDebuffTimer = 0;
    public void SlowDown( float speedDebuff, float time ) {
        Speed -= speedDebuff;
        slowDownSpeed = speedDebuff;
        slowDebuffTime = time;

        slowDebuffActive = true;
    }

    private void Death() {
        Destroy(gameObject);

        if ( OnDeath != null )
            OnDeath();
    }
}
