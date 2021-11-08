using RPG.Control;

namespace InventoryExample.Control
{
    public interface IRaycastable
    {
        bool HandleRaycast(PlayerController callingController);
    }
}