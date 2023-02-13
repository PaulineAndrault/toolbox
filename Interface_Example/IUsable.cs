// This interface is implemented on every interactable object of the scene
// The PlayerInteraction class call the Use() method on the interactable objects
// Really helpful because the interactable objects have nothing in common except the need to be Used by the Player.
// Without the IUsable interface, we would have needed a common class on every interactable object.
public interface IUsable
{
    public void Use();
}
