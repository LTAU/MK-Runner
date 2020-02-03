using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Background : MonoBehaviour
{
    //Simple script to scroll the background in time with the game
    private Renderer rend;
    private Vector2 offset;
    private float amount;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        amount += (GameManager.singleton.currentGameSpeed*Time.deltaTime)/100;
        amount = Mathf.Repeat(amount, 1);
        rend.material.SetTextureOffset("_BaseMap", new Vector2(amount,0));
    }
}
