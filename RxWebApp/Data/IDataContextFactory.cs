namespace RxWebApp.Data
{
    public interface IDataContextFactory
    {
        DataContext Current { get; set; }
    }
}