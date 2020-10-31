// using System.Collections.Generic;
// using AutoMapper;
// using ItHappened.Api.Models.Requests;
// using ItHappened.Domain;
// using ItHappened.Domain.Statistics;
// using NUnit.Framework;
//
// namespace ItHappened.UnitTests
// {
//     public class DomainToRequest
//     {
//         [Test]
//         public void DomainToRequest_ISingleFactToConcreteFact()
//         {
//             var facts = new ISingleTrackerFact[]
//             {
//                 new AverageScaleTrackerFact("Scale", "description1", 1.0, 1.1, "unit1"),
//                 new AverageRatingTrackerFact("Rating", "description2", 2.1, 2.2)
//             };
//             
//             
//             var config = new MapperConfiguration(
//                 cfg =>
//                 {
//                     //map TrackerRequest => TrackerCustomizationSettings
//                     cfg.CreateMap<TrackerRequest, TrackerCustomizationSettings>()
//                         .ConstructUsing(x =>
//                             new TrackerCustomizationSettings(
//                                 x.CustomizationSettings.IsPhotoRequired,
//                                 x.CustomizationSettings.IsScaleRequired,
//                                 x.CustomizationSettings.ScaleMeasurementUnit,
//                                 x.CustomizationSettings.IsRatingRequired,
//                                 x.CustomizationSettings.IsGeotagRequired,
//                                 x.CustomizationSettings.IsCommentRequired,
//                                 x.CustomizationSettings.AreCustomizationsOptional
//                             ));
//                 });
//             //act
//             var trackerCustomizationSettings = mapper.Map<TrackerRequest, TrackerCustomizationSettings>(trackerRequest);
//
//
//             //assert
//         
//         }
//         
//       
//
//     }
// }

//TODO fix