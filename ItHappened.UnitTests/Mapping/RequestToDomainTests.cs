using AutoMapper;
using ItHappened.Api.Models.Requests;
using ItHappened.Domain;
using NUnit.Framework;

namespace ItHappened.UnitTests
{
    public class RequestToDomain
    {
        [Test]
        public void RequestToDomain_TrackerRequestToTrackerCustomizationSettings()
        {
            //arrange
            const string scaleMeasurementUnit = "unit";
            var trackerRequest = new TrackerRequest
            {
                Name = "Name1",
                CustomizationSettings = new CustomizationSettingsRequest
                {
                    ScaleMeasurementUnit = scaleMeasurementUnit,
                    IsPhotoRequired = true,
                    IsScaleRequired = true,
                    IsRatingRequired = true,
                    IsGeotagRequired = true,
                    IsCommentRequired = true,
                    IsCustomizationRequired = true
                }
            };
            var config = new MapperConfiguration(
                cfg =>
                {
                    //map TrackerRequest => TrackerCustomizationSettings
                    cfg.CreateMap<TrackerRequest, TrackerCustomizationSettings>()
                        .ConstructUsing(x =>
                            new TrackerCustomizationSettings(
                                x.CustomizationSettings.IsPhotoRequired,
                                x.CustomizationSettings.IsScaleRequired,
                                x.CustomizationSettings.ScaleMeasurementUnit,
                                x.CustomizationSettings.IsRatingRequired,
                                x.CustomizationSettings.IsGeotagRequired,
                                x.CustomizationSettings.IsCommentRequired,
                                x.CustomizationSettings.IsCustomizationRequired
                            ));
                });
            var mapper = new Mapper(config);

            //act
            var trackerCustomizationSettings = mapper.Map<TrackerRequest, TrackerCustomizationSettings>(trackerRequest);


            //assert
            Assert.AreEqual(
                new TrackerCustomizationSettings(
                    true,
                    true,
                    scaleMeasurementUnit,
                    true,
                    true,
                    true,
                    true),
                trackerCustomizationSettings);
        }
    }
}