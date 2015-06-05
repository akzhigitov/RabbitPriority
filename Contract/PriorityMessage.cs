namespace Contract
{
    public class PriorityMessage
    {
        public PriorityMessage(int number)
        {
            Number = number;
        }

        public int Number { get; private set; }
    }
}
