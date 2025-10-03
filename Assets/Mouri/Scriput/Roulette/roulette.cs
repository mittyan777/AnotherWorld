using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roulette : MonoBehaviour
{
    [SerializeField]int power = 0;
    bool powerslot = true;

    [SerializeField] Text powerslot_text;

    [SerializeField] Player_mitubosi player;

    [SerializeField] GameObject slotUI;

    [SerializeField] float tienn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (powerslot)
        {
            power = Random.Range(1, 100);

        }

        powerslot_text.text=power.ToString();
    }
    private void FixedUpdate()
    {
       
        if (powerslot == true) { power = Random.Range(1, 100); }
       
        powerslot_text.text = ($"{power}");
    }
    public void stopslot()
    {
        powerslot = false;

        StartCoroutine(CloseSlotAfterDelay(tienn));
    }

    IEnumerator CloseSlotAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        slotUI.SetActive(false);
        player.Slotstop();
    }

    public void StartSlot()
    {
        power = 0;
        powerslot = true;
        powerslot_text.text = "0";
        slotUI.SetActive(true);
    }

}
