namespace Warframebot.Storage
{
    public class DataStore
    {
        private readonly IDataStorage _storage;
       
        public DataStore(IDataStorage storage)
        {
            _storage = storage;
           
        }
    }
}