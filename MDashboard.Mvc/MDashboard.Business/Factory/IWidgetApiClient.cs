namespace MDashboard.Business.Factory
{
    public interface IWidgetApiClient
    {
        string Name { get; set; }
        Task<KeyValuePair<string, object>> ObtenerDatosAsync(string name);
    }
}