namespace MAL_UWP_Nightmare
{
    public interface IObserver
    {
        IPage NotifyMe(SearchResult res);
        void NotifyMe(IPage p);
    }
}