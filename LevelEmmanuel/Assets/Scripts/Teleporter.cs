using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform teleLocation;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            CharacterController controller = other.GetComponent<CharacterController>();
            controller.enabled = false;
            other.transform.position = teleLocation.position;
            controller.enabled = true;
            GameController.instance.ChangeLevel();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
        Gizmos.DrawWireCube(teleLocation.position, teleLocation.localScale);
        Gizmos.color = Color.magenta;
    }
}
