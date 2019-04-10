namespace MAL_UWP_Nightmare
{
    /// <summary>
    /// Dropped due to the massive data consumption and amount of
    /// requests to realize; 
    /// This violates the 2/second limit of Jikan or other API limits.
    /// </summary>
    public class Related
    {
        public readonly long id;
        public readonly string type;
        public readonly string relation;
        public readonly string title;
        private IPage page;

        public Related(long id, string type, string title, string relation)
        {
            this.id = id;
            this.type = type;
            this.title = title;
            this.relation = relation;
        }

        public void setPage(IPage page)
        {
            this.page = page;
        }

        public IPage GetPage()
        {
            return page;
        }
    }
}