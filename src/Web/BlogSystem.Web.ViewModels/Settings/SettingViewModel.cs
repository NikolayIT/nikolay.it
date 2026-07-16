namespace BlogSystem.Web.ViewModels.Settings
{
    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;

    using Mapster;

    public class SettingViewModel : BlogSystem.Services.Mapping.IMapFrom<Setting>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string NameAndValue { get; set; }

        public void CreateMappings(TypeAdapterConfig configuration)
        {
            configuration.NewConfig<Setting, SettingViewModel>()
                .Map(dest => dest.NameAndValue, src => src.Name + " = " + src.Value);
        }
    }
}
