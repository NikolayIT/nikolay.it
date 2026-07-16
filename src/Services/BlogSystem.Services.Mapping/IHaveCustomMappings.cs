namespace BlogSystem.Services.Mapping
{
    using Mapster;

    public interface IHaveCustomMappings
    {
        void CreateMappings(TypeAdapterConfig configuration);
    }
}
