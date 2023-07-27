using UnityEngine;

public class CarAi : MonoBehaviour
{
    public float moveSpeed = 5f;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.tag == "Border")
        {
            Vector3 currentPosition = transform.position;

            Vector3 newPosition = new Vector3(currentPosition.x, -currentPosition.y, -currentPosition.z);

            transform.position = newPosition;

        }
    }
}
