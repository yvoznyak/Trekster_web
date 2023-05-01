namespace Trekster_web.ControllerServices.Interfaces
{
    public interface IProfitsControllerService
    {
        string GetSummary();

        List<string> GetProfitsByCategory();
    }
}
