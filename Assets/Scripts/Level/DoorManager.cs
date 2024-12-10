using System.Collections;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private Transform doorTransform;
    [SerializeField] private float doorOpenDistance = 4f;
    [SerializeField] private float doorUpdateTime = 0.01f;
    [SerializeField] private ItemInteractionManager itemInteractionManager;

    public enum DoorOpenDirection { Left, Up }
    [SerializeField] private DoorOpenDirection openDirection = DoorOpenDirection.Left; // Default is Left

    private Vector3 baseLocalPosition = Vector3.zero;
    private Vector3 desiredLocalPosition;

    private bool canClose = true;

    private Coroutine openRoutine;
    private Coroutine closeRoutine;

    private Collider triggerCollider;

    private void Awake()
    {
        gameObject.TryGetComponent(out triggerCollider);

        desiredLocalPosition = openDirection == DoorOpenDirection.Left
            ? new Vector3(doorOpenDistance, 0, 0)
            : new Vector3(0, doorOpenDistance, 0);

        if (itemInteractionManager != null)
        {
            canClose = false;
            itemInteractionManager.OnItemDestroyed += EnableCollider;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerCollider.enabled)
        {
            HandleOpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (canClose && triggerCollider.enabled)
        {
            HandleCloseDoor();
        }
    }

    public void HandleOpenDoor()
    {
        if (closeRoutine != null)
        {
            StopCoroutine(closeRoutine);
        }
        openRoutine = StartCoroutine(OpenDoor());
    }

    public void HandleCloseDoor()
    {
        if (openRoutine != null)
        {
            StopCoroutine(openRoutine);
        }
        closeRoutine = StartCoroutine(CloseDoor());
    }

    IEnumerator OpenDoor()
    {
        bool isOpening = true;
        while (isOpening)
        {
            Vector3 positionStep = Vector3.Lerp(baseLocalPosition, desiredLocalPosition, doorUpdateTime);
            doorTransform.localPosition += positionStep;

            if ((openDirection == DoorOpenDirection.Left && doorTransform.localPosition.x >= doorOpenDistance) ||
                (openDirection == DoorOpenDirection.Up && doorTransform.localPosition.y >= doorOpenDistance))
            {
                isOpening = false;
            }
            yield return new WaitForSeconds(doorUpdateTime);
        }
    }

    private void EnableCollider()
    {
        triggerCollider.enabled = true;
    }

    IEnumerator CloseDoor()
    {
        bool isClosing = true;
        while (isClosing)
        {
            Vector3 positionStep = Vector3.Lerp(baseLocalPosition, desiredLocalPosition, doorUpdateTime);
            doorTransform.localPosition -= positionStep;

            if ((openDirection == DoorOpenDirection.Left && doorTransform.localPosition.x <= baseLocalPosition.x) ||
                (openDirection == DoorOpenDirection.Up && doorTransform.localPosition.y <= baseLocalPosition.y))
            {
                isClosing = false;
            }
            yield return new WaitForSeconds(doorUpdateTime);
        }
    }
}
