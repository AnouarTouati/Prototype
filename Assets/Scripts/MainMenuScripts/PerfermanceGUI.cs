using UnityEngine;
using UnityEngine.UI;

public class PerfermanceGUI : MonoBehaviour {

    public Slider EngineSlider;
    public Slider SuspensionSlider;
    public Slider TiresSlider;
    public Slider BrakesSlider;
    public Text EngineValueText;
    public Text SuspensionValueText;
    public Text TiresValueText;
    public Text BrakesValueText;
    /* 
       ALL of the sliders
       Above are not intercatble they are just to display values
    */
    public GameObject[] DropDownElementsLevels;
  
    public Dropdown Dropdown;
    public int ThatValue;
    
    void Update()
    {
       
        EngineSlider.value = 4;
        EngineValueText.text = "" + 4;

        SuspensionSlider.value = 6;
        SuspensionValueText.text = "" + 6;

        TiresSlider.value = 5;
        TiresValueText.text = "" + 5;

        BrakesSlider.value = 7.5f;
        BrakesValueText.text = "" + 7.5;
        ThatValue = Dropdown.value;
       
    }
    public void DropdownSwitch()
    {
        //i did it with all this statements because i had problem with the for lopp

        for (int i = 0; i < DropDownElementsLevels.Length; i++)
        {
            if (i != Dropdown.value)
            {
                DropDownElementsLevels[i].SetActive(false);
            }
            else
            {
                DropDownElementsLevels[i].SetActive(true);
            }
        }
    }
   /*int index=Random.Range(0,ColorsArray.Length-1);
   
    if(PreviousIndex==index)
        {
            MaxNumberOfTimesValue++;
            if(MaxNumberOfTimesValue>=MaxNumberOfTimes)
            {
        PreviousIndex=index;
        index=Random.Range(0,ColorsArray.Length-1);
                     while(PreviousIndex==index)
                           {
                          index=Random.Range(0,ColorsArray.Length-1);
                            }
              }
        }
else{
 MaxNumberOfTimesValue=1;
}
    GameObject TempGO = instantiate();
    TempGo.GetComponent<Material>().color=ColorArray[index];*/
}
