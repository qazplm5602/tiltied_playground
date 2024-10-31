using UnityEngine;

public class GroundObjFinder : MonoBehaviour, IGroundCompo
{
    private Ground _ground;
    
    public void Initialize(Ground ground)
    {
        _ground = ground;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MassHaveObj massHaveObj))
            _ground._onGroundObj.Add(massHaveObj);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out MassHaveObj massHaveObj))
            _ground._onGroundObj.Remove(massHaveObj);
    }
}
