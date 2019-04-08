namespace MAL_UWP_Nightmare
{
    internal interface IObserver
    {
        void NotifyMe(SearchResult res);
        IPage loadTarget();
    }
}