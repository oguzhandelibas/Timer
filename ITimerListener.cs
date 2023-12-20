namespace Timer
{
    public interface ITimerListener
    {
        void OnTimerStart(int duration);

        void OnTimerStop();
    }
}
