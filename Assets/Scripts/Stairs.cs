using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    private GameObject player;
    private Player playerScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerScript = GameObject.Find("Player").GetComponent<Player>();
    }


    private void OnPointerEnter()
    {
        Debug.Log("Enter");

        if (transform.name == "Stairs2")
            playerScript.ShowMessage("Press A to go downstairs");
        else if (transform.name == "Stairs1")
            playerScript.ShowMessage("Press A to go upstairs");
        else if (transform.name == "Stairs3")
            playerScript.ShowMessage("Press A to go upstairs");
        else
            playerScript.ShowMessage("Press A to go downstairs");
    }

    private void OnPointerExit()
    {
        playerScript.HideInfoPanel();
    }

    private void OnPointerPressed()
    {
        if (transform.name == "Stairs2")
            player.transform.position = new Vector3(35, 7.1f, 18);
        else if (transform.name == "Stairs1")
            player.transform.position = new Vector3(14, 21.75f, 15);
        else if (transform.name == "Stairs3")
            player.transform.position = new Vector3(35, 7.1f, 3);
        else
            player.transform.position = new Vector3(32, -8.15f, -20);
        
        
        playerScript.HideInfoPanel();
    }
}
