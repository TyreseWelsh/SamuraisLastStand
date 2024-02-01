using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNode : MonoBehaviour
{
    SphereCollider sphereCollider;
    GravityAttractor attractor;

    [SerializeField] GameObject node;

    // Start is called before the first frame update
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        attractor = GetComponentInChildren<GravityAttractor>();

        SpawnNodes();
    }

    private void FixedUpdate()
    {

    }

    void SpawnNodes()
    {
        for (int i = 0; i < 15; i++)
        {
            Vector3 directionToPoint = Random.onUnitSphere;

            RaycastHit hit;

            if (!Physics.SphereCast(transform.position, 5, directionToPoint, out hit, gameObject.transform.localScale.x / 2, 1 << 2))
            {
                float angleX = Random.Range(0, 360);
                float angleY = Random.Range(0, 360);
                float angleZ = Random.Range(0, 360);
                GameObject currentNode = Instantiate(node, transform.position + (directionToPoint * (gameObject.transform.localScale.x / 2)), Quaternion.Euler(angleX, angleY, angleZ));
            }
        }
    }
}