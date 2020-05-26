using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerController : MonoBehaviour
{

    public bool isAlive = true;
    public bool isInShip = false;

    [Header("Vitals")]
    public float health = 100f;
    public float healthMax = 100f;
    public float oxygen = 75f;
    public float oxygenMax = 100f;
    public float food = 50f;
    public float foodMax = 100f;

    [Header("Inventory")]
    //private Inventory inventory;
    //[SerializeField] private UI_Inventory uiInventory;
    //private Item activeItem;
    public InventoryObject inventory;
    public MouseItem mouseItem = new MouseItem();
    public Transform mainHand;
    public bool hasMainHand = false;

    private bool isLookingAt = false;
    private RaycastHit lookingAt;
    public Animator crosshairAnimator;

    public float interactDelay = 0.5f;
    private float interactDelayInit;

    private int EndPieces = 0;
    public AudioClip endPieceSound;

    // Start is called before the first frame update
    void Start()
    {
        interactDelayInit = interactDelay;
        //Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isInShip)
        {
            GameManager.instance.playerTransform = transform;
            GetRaycastLook();

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                if (Cursor.lockState == CursorLockMode.Confined)
                    Cursor.lockState = CursorLockMode.None;
                else Cursor.lockState = CursorLockMode.Confined;
            }

        }
        else
        {
            GameManager.instance.playerTransform = GameManager.instance.ship.transform;
        }


        //TEST SAVING AND LOADING INVENTORY
        if (Input.GetKeyDown(KeyCode.Z))
        {
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            inventory.Load();
        }

        interactDelay = Mathf.Clamp(interactDelay - Time.deltaTime, -1, interactDelayInit);
    }

    private void FixedUpdate()
    {
        if (!isInShip)
            Interact();
    }


    private void AddPiece()
    {
        EndPieces++;
        AudioSource.PlayClipAtPoint(endPieceSound, transform.position);
        if (EndPieces == 3)
        {
            GameManager.instance.gameVictoryUI.gameObject.SetActive(true);
        }
    }

    private void Interact()
    {
        if (isLookingAt && lookingAt.transform.gameObject != null)
            switch (lookingAt.transform.gameObject.tag)
            {
                case "Item":
                    crosshairAnimator.SetBool("onInteractable", true);

                    if (Input.GetKeyDown(KeyCode.F))
                    {

                        GroundItem item = lookingAt.transform.gameObject.GetComponent<GroundItem>();

                        if (item.item.description.Equals("End Piece"))
                        {
                            AddPiece();
                        }

                        inventory.AddItem(new Item(item.item), item.amount);
                        Destroy(item.gameObject);


                    }

                    break;

                case "Machine":
                    break;

                case "Ship":
                    crosshairAnimator.SetBool("onInteractable", true);

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (interactDelay <= 0f)
                        {
                            Debug.Log("F is pressed");


                            // Get in ship
                            GetComponentInChildren<CameraRotation>().enabled = false;
                            GetComponentInChildren<Camera>().enabled = false;
                            GetComponentInChildren<AudioListener>().enabled = false;
                            transform.GetChild(1).GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                            GetComponent<PlayerMovement>().enabled = false;
                            GetComponent<CapsuleCollider>().enabled = false;
                            mainHand.gameObject.SetActive(false);



                            GameManager.instance.ship.GetComponentInChildren<AudioListener>().enabled = true;
                            GameManager.instance.ship.GetComponentInChildren<Camera>().enabled = true;
                            GameManager.instance.ship.GetComponentInChildren<ShipCamera>().enabled = true;
                            GameManager.instance.ship.enabled = true;

                            isInShip = true;
                            crosshairAnimator.SetBool("onInteractable", false);

                            interactDelay = interactDelayInit;
                        }

                    }




                    break;

                default:
                    crosshairAnimator.SetBool("onInteractable", false);
                    break;
            }
        else crosshairAnimator.SetBool("onInteractable", false);
    }

    private void UseMainHand()
    {
        if (hasMainHand)
        {

        }
    }

    public void Eat(float restoreHealth, float restoreFood)
    {
        health = Mathf.Clamp(health + restoreHealth, 0, healthMax);
        food = Mathf.Clamp(food + restoreFood, 0, foodMax);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            isAlive = false;

            Cursor.lockState = CursorLockMode.None;

            Time.timeScale = 0f;

            // Show canvas with game over
            GameManager.instance.gameOverUI.gameObject.SetActive(true);

        }
    }

    public void GetRaycastLook()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        isLookingAt = Physics.Raycast(ray, out hit, 2f);
        lookingAt = hit;
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Items = new InventorySlot[28]; // Clears all items in the inventory
    }

}
