using System;
using Game.Hero;
using Pathfinding;
using UnityEngine;

namespace Game.Logic.Cop
{
    public class CopView : MonoBehaviour
    {
        public CharacterController _controller;
        public Seeker _seeker;
        private HeroMove _player;
        private Path path;
        private bool reachedEndOfPath;
        public float speed = 2;

        public float nextWaypointDistance = 3;

        private int currentWaypoint = 0;
        private float _timer = 3f;
        public float _pathfindingDelay = 2f;
        public Animator _animator;

        public void Start()
        {
            _player = FindObjectOfType<HeroMove>();
            if (!_animator)
            {
                _animator = GetComponentInChildren<Animator>();
            }

            FindPath();
        }

        private void FindPath()
        {
            path = _seeker.StartPath(transform.position, _player.transform.position, OnPathComplete);
        }

        private void OnPathComplete(Path p)
        {
            _timer = _pathfindingDelay;
            currentWaypoint = 0;
        }

        public void Update()
        {
            if (path == null)
            {
                // We have no path to follow yet, so don't do anything
                _animator.SetBool("Run", false);
                return;
            }

            // Check in a loop if we are close enough to the current waypoint to switch to the next one.
            // We do this in a loop because many waypoints might be close to each other and we may reach
            // several of them in the same frame.
            reachedEndOfPath = false;
            // The distance to the next waypoint in the path
            float distanceToWaypoint;
            while (true)
            {
                // If you want maximum performance you can check the squared distance instead to get rid of a
                // square root calculation. But that is outside the scope of this tutorial.
                distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
                if (distanceToWaypoint < nextWaypointDistance)
                {
                    // Check if there is another waypoint or if we have reached the end of the path
                    if (currentWaypoint + 1 < path.vectorPath.Count)
                        currentWaypoint++;
                    else
                    {
                        // Set a status variable to indicate that the agent has reached the end of the path.
                        // You can use this to trigger some special code if your game requires that.
                        reachedEndOfPath = true;
                        break;
                    }
                }
                else
                    break;
            }

            // Slow down smoothly upon approaching the end of the path
            // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
            var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

            // Direction to the next waypoint
            // Normalize it so that it has a length of 1 world unit
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            // Multiply the direction by our desired speed to get a velocity
            Vector3 velocity = dir * speed; // * speedFactor;
            _animator.SetBool("Run", true);
            // Move the agent using the CharacterController component
            // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
            transform.LookAt(transform.position + dir, Vector3.up);
            _controller.SimpleMove(velocity);


            if (_timer <= 0)
            {
                FindPath();
            }

            _timer -= Time.deltaTime;
        }

        public void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Player"))
            {
                _player.ReturnToJail();
            }
        }
    }
}