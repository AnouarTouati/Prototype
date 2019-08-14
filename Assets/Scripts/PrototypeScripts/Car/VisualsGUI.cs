using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class VisualsGUI : MonoBehaviour
{
    public GameObject BodyMenu;
    public GameObject SideViewMirrosMenu;
    public GameObject HoodMenu;
    public GameObject RimsMenu;
    private List<GameObject> listOfMenus=new List<GameObject>();
    public Dropdown  Dropdown;
    private void Start()
    {
        listOfMenus.Add(BodyMenu);
        listOfMenus.Add(SideViewMirrosMenu);
        listOfMenus.Add(HoodMenu);
        listOfMenus.Add(RimsMenu);

        ActivateMenu();
    }
    public void ActivateMenu()
    {
        
        for (int i = 0; i < listOfMenus.Count; i++)
        {
            if (i != Dropdown.value)
            {
                listOfMenus[i].SetActive(false);
            }
            else
            {
                listOfMenus[i].SetActive(true);
            }
        }
    }
    /*
  public   void EnableBodyMenu()
    {
        BodyMenu.SetActive(true);
        SideViewMirrosMenu.SetActive(false);
        HoodMenu.SetActive(false);
        RimsMenu.SetActive(false);
    }

 public    void EnableSideViewMirrorsMenu()
    {
        BodyMenu.SetActive(false);
        SideViewMirrosMenu.SetActive(true);
        HoodMenu.SetActive(false);
        RimsMenu.SetActive(false);
    }
public     void EnableHoodMenu()
    {
        BodyMenu.SetActive(false);
        SideViewMirrosMenu.SetActive(false);
        HoodMenu.SetActive(true);
        RimsMenu.SetActive(false);
    }
  public   void EnableRimsMenu()
    {
        BodyMenu.SetActive(false);
        SideViewMirrosMenu.SetActive(false);
        HoodMenu.SetActive(false);
        RimsMenu.SetActive(true);
    }
    */
}
