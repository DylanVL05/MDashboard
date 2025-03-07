namespace MDashboard.Business.Factory
{
    public interface IWidgetApiClient
    {
        Task<string> ObtenerDatosAsync();
    }
}