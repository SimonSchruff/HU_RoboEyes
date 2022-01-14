using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouseArrow : MonoBehaviour
{
    public Sprite ArrowSprite; 
    public Sprite ArrowInactiveSprite; 

    private SpriteRenderer m_sp; 
    private bool m_isHovering; 
    private Vector3 m_mousePos; 

    void Start()
    {
        m_sp = GetComponentInChildren<SpriteRenderer>(); 
        m_sp.sprite = ArrowSprite; 
    }

    void Update()
    {
        if(m_isHovering)
            return; 
            
        // Get Mouse pos 
        m_mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = m_mousePos - transform.position; 

        // Calculates angle between direction and Vector2.right
        float angle = Vector2.SignedAngle(Vector2.right, direction); 
        // Rotate Arrow towards Mouse Pos
        transform.eulerAngles = new Vector3(0,0,angle); 

        

    }

    private void OnMouseEnter()
    {
        m_isHovering = true; 
        transform.eulerAngles = Vector3.zero; 
        SwitchArrowSprite(); 
        
    }

    private void OnMouseExit()
    {
        m_isHovering = false; 
        SwitchArrowSprite(); 
    }

    private void SwitchArrowSprite()
    {
        if(m_sp.sprite == ArrowInactiveSprite)
            m_sp.sprite = ArrowSprite; 
        else
            m_sp.sprite = ArrowInactiveSprite; 
    }
}
