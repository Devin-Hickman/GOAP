using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Player : MonoBehaviour
{

    NPC npcFocus = null;
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    private void Start()
    {
        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);

            if(results.Count == 0)
            {
                GameObject g = GameObjectClickedOn();
                if (g != null && g.GetComponent<NPC>() != null) 
                {
                    if (g.GetComponent<NPC>() != npcFocus && npcFocus != null)
                    {
                        npcFocus.ClearUI();
                    }

                    npcFocus = g.GetComponent<NPC>();
                    npcFocus.SetUI();
                }
                else
                {
                    if (npcFocus != null)
                    {
                        npcFocus.ClearUI();
                    }
                    npcFocus = null;
                }
            }
        }
    }

    private GameObject GameObjectClickedOn()
    {
        GameObject clickedObject = null;

        RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit2D.collider != null)
        {
            clickedObject = hit2D.collider.gameObject;
        }
        return clickedObject;
    }
}
