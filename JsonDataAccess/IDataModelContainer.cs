namespace JsonDataAccess
{
    interface IDataModelContainer<T> where T : DataModel
    {
        T Model { get; set; }
    }
}
