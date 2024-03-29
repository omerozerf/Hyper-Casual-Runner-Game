using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private float moveSpeed;
    [SerializeField] public bool isActive;
    [SerializeField] public bool isShot;
    [SerializeField] private new Renderer renderer;
    [SerializeField] private Color color;
    [SerializeField] private Transform mainTransform;
    [SerializeField] private PlayerController mainPlayer;
    [SerializeField] private bool isMainPlayer = false;
    [SerializeField] private BulletSpawner bulletSpawner;
    [SerializeField] private Canvas canvas;
    [SerializeField] private PercentCounter percentCounter;
    [SerializeField] private Canvas winScreen;
    [SerializeField] private GameObject spawner;
    [SerializeField] private Animator animator;
    
    public static List<PlayerController> players = new List<PlayerController>();
    
    private void Start()
    {
        
        if (isMainPlayer)
        {
            players.Add(mainPlayer);
            animator.Play("Pistol Run");
        }
        
    }

    private void Update()
    {
        
        WinAnimation();
        if (isActive)
        {
            MoveStraight();
            MoveHorizontal();
        }
        
        if (isShot)
        {
            renderer.material.color = color;
        }
        
    }

    private void WinAnimation()
    {
        if (transform.position.z > 282)
        {
            animator.Play("Victory");
            isActive = false;
            winScreen.gameObject.SetActive(true);
            Destroy(spawner);

            foreach (var VARIABLE in players)
            {
                Destroy(VARIABLE.spawner);
            }
        }
    }
    
    
    private void MoveHorizontal()
    {
        
        // var horizontalDirection = Input.GetAxis("Horizontal");

        var horizontalInput = joystick.Horizontal;
        var verticalInput = joystick.Vertical;
        
        transform.Translate(Vector3.right * (horizontalInput * moveSpeed * Time.deltaTime));
    }

    private void MoveStraight()
    {
        transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Greenzone") || other.CompareTag("Redzone"))
        {
            other.gameObject.SetActive(false);
        }
        */
        

        if (other.CompareTag("Greenzone"))
        {
            bulletSpawner.spawnSpeed += bulletSpawner.spawnSpeed / 3;
        }
        if (other.CompareTag("Redzone"))
        {
            bulletSpawner.spawnSpeed -= bulletSpawner.spawnSpeed / 3;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController p) && p.isShot && isMainPlayer)
        {
            p.isActive = true;
            var position1 = players[^1].transform.position;
            var position = new Vector3(position1.x - Random.Range(-3f, 3f), 0, position1.z - 1f);
            players.Add(p);
            collision.gameObject.transform.position = position;
            p.animator.Play("Pistol Run");
            p.canvas.gameObject.SetActive(false);
            // p.isMainPlayer = true;
            Debug.Log("aktifleşti");
            Debug.Log(p, p);
        }

        if (collision.gameObject.TryGetComponent(out PlayerController playerController) && !playerController.isShot)
        {
            
            playerController.animator.Play("Standing Death Left 02");
            playerController.isActive = false;
            players[^1].animator.Play("Standing Death Left 02");
            players[^1].isActive = false;
            Destroy(players[^1].gameObject, 2f);
            players.RemoveAt(players.Count-1);
            Debug.Log("öldü");
            Debug.Log(playerController, playerController);

            foreach (var variable in players)
            {
                variable.animator.Play("Standing Death Left 02");
                variable.isActive = false;
                Destroy(variable.gameObject, 2f);
                
            }
        }
    }

    /*private Position CreateArmy()
    {
        int lineCounter = 0;

        if (lineCounter == 0)
        {
            lineCounter++;
            
        }

        if (lineCounter == 1)
        {
            lineCounter++;
        }

        if (lineCounter == 3)
        {
            lineCounter++;
        }
    }*/
}
