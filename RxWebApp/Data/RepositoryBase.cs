namespace RxWebApp.Data
{
    internal class RepositoryBase
    {
        private readonly IDataContextFactory _dbFactory;

        protected RepositoryBase(IDataContextFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        protected DataContext Db
        {
            get { return _dbFactory.Current; }
        }

    }
}