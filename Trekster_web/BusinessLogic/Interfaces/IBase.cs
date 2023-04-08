namespace BusinessLogic.Interfaces
{
    public interface IBase<T>
    {
        IEnumerable<T> GetAll();

        T GetById(int id);

        void Save(T entity);

        void Delete(int id);
    }
}
