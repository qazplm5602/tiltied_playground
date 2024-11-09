[System.Serializable]
public class Slot
{
    public Item item;

    public bool IsEmpty()
    {
        return item == null;
    }

    public void ClearSlot()
    {
        item = null;
    }
}
