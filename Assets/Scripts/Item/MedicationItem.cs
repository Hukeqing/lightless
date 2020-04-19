namespace Item
{
    public class MedicationItem : Item
    {
        public int value;

        public override void ApplyItem(Player.PlayerControl pc)
        {
            pc.AddHealth(value);
        }
    }
}