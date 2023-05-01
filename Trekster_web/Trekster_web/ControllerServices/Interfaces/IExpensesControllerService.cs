namespace Trekster_web.ControllerServices.Interfaces
{
    public interface IExpensesControllerService
    {
        string GetSummary();

        List<string> GetExpencesByCategory();
    }
}
