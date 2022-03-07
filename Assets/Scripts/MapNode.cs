using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public GameObject nodeLink;

    private SpriteRenderer spriteRenderer;
    private List<MapNode> forwardNodes = new List<MapNode>();

    public void Link(MapNode nextNode)
    {
        forwardNodes.Add(nextNode);
        float linkLen = Vector3.Distance(transform.position, nextNode.transform.position);
        Vector3 linkPos = Vector3.Lerp(transform.position, nextNode.transform.position, 0.5f);
        GameObject link = Instantiate(nodeLink, linkPos, Quaternion.identity);
        Transform linkTransform = link.GetComponent<Transform>();
        linkTransform.localScale = new Vector3(0.13f, linkLen);
        linkTransform.up = nextNode.transform.position - transform.position;
        linkTransform.Translate(0, 0, 0.39f);
    }

    void Start()
    {
        Debug.Log("MapNode init");
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.black;
    }

    void OnMouseDown()
    {
        Debug.Log("MapNode clicked");
        if (spriteRenderer.color == Color.black)
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = Color.black;
        }
    }
}
