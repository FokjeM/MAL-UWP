namespace MAL_UWP_Nightmare
{
    public interface IObserver
    {
        void NotifyMe(SearchResult res);
        void NotifyMe(IPage p);
        IPage LoadTarget();
    }
}