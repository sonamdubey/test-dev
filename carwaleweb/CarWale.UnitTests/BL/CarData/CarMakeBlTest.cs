using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.ViewModels;
using Carwale.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarWale.UnitTests.BL.CarData
{
    [TestClass]
    public class CarMakeBlTest : CarMakeBase
    {

        public List<ModelSummary> GetUnsortedListHavingLaunchWithHighConfidence()
        {
            List<ModelSummary> modelList = new List<ModelSummary>();
            ModelSummary model = new ModelSummary
            {
                CWConfidence = 5,
                LaunchDate = DateTime.Now.AddDays(7),
                MinPrice = 10,
                ModelId = 1,
                ShowDate = true
            };
            modelList.Add(model);
            model = new ModelSummary
            {
                CWConfidence = 5,
                LaunchDate = DateTime.Now.AddDays(7),
                MinPrice = 11,
                ModelId = 2,
                ShowDate = true
            };
            modelList.Add(model);
            model = new ModelSummary
            {
                CWConfidence = 5,
                LaunchDate = DateTime.Now.AddDays(6),
                MinPrice = 11,
                ModelId = 3,
                ShowDate = true
            };
            modelList.Add(model);
            model = new ModelSummary
            {
                CWConfidence = 5,
                LaunchDate = DateTime.Now.AddDays(3),
                MinPrice = 11,
                ModelId = 4,
                ShowDate = true
            };
            modelList.Add(model);
            return modelList;
        }
        public List<ModelSummary> GetNewModelList()
        {
            DateTime date = new DateTime(2020, 1, 1);
            List<ModelSummary> modelList = new List<ModelSummary> {
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(-2),
                    MinPrice = 11,
                    ModelId = 21,
                    New = true
                },
                new ModelSummary
                {
                    CWConfidence = 3,
                    LaunchDate = date.AddDays(-8),
                    MinPrice = 9,
                    ModelId = 22,
                    New = true
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(-200),
                    MinPrice = 55,
                    ModelId = 23,
                    New = true
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(-500),
                    MinPrice = 15,
                    ModelId = 24,
                    New = true,
                    ShowDate = true
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(0),
                    MinPrice = 12,
                    ModelId = 25,
                    New = true
                }
            };
            return modelList;
        }
        public List<ModelSummary> GetUnsortedUpcomingModelList()
        {
            DateTime date = new DateTime(2020, 1, 1);
            List<ModelSummary> modelList = new List<ModelSummary> {
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(42),
                    MinPrice = 11,
                    ModelId = 1,
                    New = false,
                    ShowDate = false
                },
                new ModelSummary
                {
                    CWConfidence = 3,
                    LaunchDate = date.AddDays(42),
                    MinPrice = 9,
                    ModelId = 7,
                    New = false,
                    ShowDate = false
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(1),
                    MinPrice = 55,
                    ModelId = 10,
                    New = false,
                    ShowDate = true
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(3),
                    MinPrice = 15,
                    ModelId = 8,
                    New = false,
                    ShowDate = true
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(3),
                    MinPrice = 12,
                    ModelId = 9,
                    New = false,
                    ShowDate = true
                },
                new ModelSummary
                {
                    CWConfidence = 3,
                    LaunchDate = date.AddDays(3),
                    MinPrice = 10,
                    ModelId = 19,
                    New = false,
                    ShowDate = false
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(25),
                    MinPrice = 11,
                    ModelId = 20,
                    New = false,
                    ShowDate = true
                }

            };
            return modelList;
        }
        public List<ModelSummary> GetSortedUpcomingModelList()
        {
            DateTime date = new DateTime(2020, 1, 1);
            List<ModelSummary> modelList = new List<ModelSummary> {
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(1),
                    MinPrice = 55,
                    ModelId = 10,
                    New = false,
                    ShowDate = true
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(3),
                    MinPrice = 12,
                    ModelId = 9,
                    New = false,
                    ShowDate = true
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(3),
                    MinPrice = 15,
                    ModelId = 8,
                    New = false,
                    ShowDate = true
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(25),
                    MinPrice = 11,
                    ModelId = 20,
                    New = false,
                    ShowDate = true
                },
                new ModelSummary
                {
                    CWConfidence = 3,
                    LaunchDate = date.AddDays(3),
                    MinPrice = 10,
                    ModelId = 19,
                    New = false,
                    ShowDate = false
                },
                new ModelSummary
                {
                    CWConfidence = 3,
                    LaunchDate = date.AddDays(42),
                    MinPrice = 9,
                    ModelId = 7,
                    New = false,
                    ShowDate = false
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(42),
                    MinPrice = 11,
                    ModelId = 1,
                    New = false,
                    ShowDate = false
                }
            };
            return modelList;
        }
        public List<ModelSummary> GetFinalOrderedList()
        {
            DateTime date = new DateTime(2020, 1, 1);
            List<ModelSummary> modelList = new List<ModelSummary> {
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(-2),
                    MinPrice = 11,
                    ModelId = 21,
                    New = true,
                    ShowDate = true
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(0),
                    MinPrice = 12,
                    ModelId = 25,
                    New = true
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(1),
                    MinPrice = 55,
                    ModelId = 10,
                    New = false,
                    ShowDate = true
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(3),
                    MinPrice = 12,
                    ModelId = 9,
                    New = false,
                    ShowDate = true
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(3),
                    MinPrice = 15,
                    ModelId = 8,
                    New = false,
                    ShowDate = true
                },
                new ModelSummary
                {
                    CWConfidence = 3,
                    LaunchDate = date.AddDays(-8),
                    MinPrice = 9,
                    ModelId = 22,
                    New = true
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(-200),
                    MinPrice = 55,
                    ModelId = 23,
                    New = true
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(-500),
                    MinPrice = 15,
                    ModelId = 24,
                    New = true,
                    ShowDate = true
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(25),
                    MinPrice = 11,
                    ModelId = 20,
                    New = false,
                    ShowDate = true
                },
                new ModelSummary
                {
                    CWConfidence = 3,
                    LaunchDate = date.AddDays(3),
                    MinPrice = 10,
                    ModelId = 19,
                    New = false,
                    ShowDate = false
                },
                new ModelSummary
                {
                    CWConfidence = 3,
                    LaunchDate = date.AddDays(42),
                    MinPrice = 9,
                    ModelId = 7,
                    New = false,
                    ShowDate = false
                },
                new ModelSummary
                {
                    CWConfidence = 5,
                    LaunchDate = date.AddDays(42),
                    MinPrice = 11,
                    ModelId = 1,
                    New = false,
                    ShowDate = false
                }
            };
            return modelList;
        }
        [TestMethod]
        public void TestSortUpcoming()
        {
            List<ModelSummary> modelList = GetUnsortedUpcomingModelList();
            List<ModelSummary> sortedModelList = GetSortedUpcomingModelList();
            carMakesBL.SortUpcomingModel(modelList);
            for (int i = 0; i < modelList.Count; i++)
            {
                Assert.IsTrue(modelList[i].ModelId == sortedModelList[i].ModelId);
            }
        }
        [TestMethod]
        public void TestFilter()
        {
            List<ModelSummary> modelList = new List<ModelSummary> {
                new ModelSummary
                {
                    ModelId = 1,
                    ShowDate = false,
                },
                new ModelSummary
                {
                    ModelId = 2,
                    ShowDate = true,
                },
                new ModelSummary
                {
                    ModelId = 3,
                    ShowDate = true,
                },
                new ModelSummary
                {
                    ModelId = 4,
                    ShowDate = false,
                },
                new ModelSummary
                {
                    ModelId = 5,
                    ShowDate = false,
                }
            };
            List<ModelSummary> orderdModelList = new List<ModelSummary>();
            List<ModelSummary> activeModelList = new List<ModelSummary>();

            carMakesBL.FilterList(modelList, orderdModelList, activeModelList);

            Assert.AreEqual(modelList.Count, orderdModelList.Count + activeModelList.Count);

            CollectionAssert.AllItemsAreNotNull(orderdModelList);
            Assert.AreEqual(2, orderdModelList.Count);
            Assert.AreEqual(3, orderdModelList[1].ModelId);

            CollectionAssert.AllItemsAreNotNull(activeModelList);
            Assert.AreEqual(3, activeModelList.Count);
            Assert.AreEqual(5, activeModelList[2].ModelId);

        }
        [TestMethod]
        public void TestIsDateWithIn31Days()
        {
            bool res = DateTimeUtility.IsDateWithIn31Days(DateTime.Now.AddDays(30), DateTime.Now);
            Assert.IsTrue(res);
            res = DateTimeUtility.IsDateWithIn31Days(DateTime.Now.AddDays(31.01), DateTime.Now);
            Assert.IsFalse(res);
            res = DateTimeUtility.IsDateWithIn31Days(DateTime.Now.AddDays(32), DateTime.Now);
            Assert.IsFalse(res);
            res = DateTimeUtility.IsDateWithIn31Days(DateTime.Now, DateTime.Now);
            Assert.IsTrue(res);
            res = DateTimeUtility.IsDateWithIn31Days(DateTime.Now.AddDays(-1), DateTime.Now);
            Assert.IsFalse(res);
        }
        [TestMethod]
        public void TestShowLaunchLabel()
        {
            bool res = DateTimeUtility.ShowLaunchLabel(DateTime.Now, DateTime.Now);
            Assert.IsTrue(res);
            res = DateTimeUtility.ShowLaunchLabel(DateTime.Now.AddDays(7), DateTime.Now);
            Assert.IsTrue(res);
            res = DateTimeUtility.ShowLaunchLabel(DateTime.Now.AddDays(8), DateTime.Now);
            Assert.IsFalse(res);
            res = DateTimeUtility.ShowLaunchLabel(DateTime.Now.AddDays(-1), DateTime.Now);
            Assert.IsTrue(res);
            res = DateTimeUtility.ShowLaunchLabel(DateTime.Now.AddDays(-7), DateTime.Now);
            Assert.IsTrue(res);
            res = DateTimeUtility.ShowLaunchLabel(DateTime.Now.AddDays(-8), DateTime.Now);
            Assert.IsFalse(res);
        }
        [TestMethod]
        public void TestLaunchWithInWeekWithHighConfidence()
        {
            List<ModelSummary> modelList = GetUnsortedListHavingLaunchWithHighConfidence();
            carMakesBL.SortUpcomingModel(modelList);
            int count = modelList.Count;
            for (int i = 1; i < count; i++)
            {
                if (modelList[i].CWConfidence == modelList[i - 1].CWConfidence)
                {
                    Assert.IsTrue(modelList[i].LaunchDate <= modelList[i].LaunchDate);
                    if (modelList[i].LaunchDate == modelList[i].LaunchDate)
                    {
                        Assert.IsTrue(modelList[i].MinPrice <= modelList[i].MinPrice);
                    }
                }
            }
        }
        [TestMethod]
        public void TestGetActiveModelsWithDetails_WhenDataPresentInCache()
        {
            _modelCacheRepo.Setup(cache => cache.GetActiveModelsByMake(It.IsAny<int>())).Returns(new List<ModelSummary> { new ModelSummary { } });

            carMakesBL.GetActiveModelsWithDetails(1, 3);

            _modelRepo.Verify(db => db.GetActiveModelsByMake(It.IsAny<int>(), false), Times.Never());
            _modelCacheRepo.Verify(cache => cache.SetActiveModelsByMakeInCache(It.IsAny<int>(), It.IsAny<List<ModelSummary>>()), Times.Never());
        }
        [TestMethod]
        public void TestGetActiveModelsWithDetails_WhenDataNotPresentInCache()
        {
            List<ModelSummary> activeModel = GetNewModelList();
            int newModelCount = activeModel.Count;

            var upcomingModels = GetUnsortedUpcomingModelList();
            upcomingModels.ForEach(model => model.ShowDate = false);

            int upcomingModelCount = upcomingModels.Count;
            activeModel.AddRange(upcomingModels);

            _modelCacheRepo.Setup(cache => cache.GetActiveModelsByMake(It.IsAny<int>())).Returns((List<ModelSummary>)null);

            _modelRepo.Setup(db => db.GetActiveModelsByMake(It.IsAny<int>(), false)).Returns(activeModel);

            _prices.Setup(pricebl => pricebl.GetAvailablePriceForModel(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<VersionPrice>>(), It.IsAny<bool>())).Returns(new Carwale.Entity.PriceQuote.PriceOverview { Price = 1000000 });

            var res = carMakesBL.GetActiveModelsWithDetails(1, 3);

            _modelRepo.Verify(db => db.GetActiveModelsByMake(It.IsAny<int>(), false), Times.Once());

            _modelCacheRepo.Verify(cache => cache.SetActiveModelsByMakeInCache(It.IsAny<int>(), It.IsAny<List<ModelSummary>>()), Times.Once());

            _prices.Verify(bl => bl.GetAvailablePriceForModel(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<VersionPrice>>(), It.IsAny<bool>()), Times.Exactly(newModelCount));

            var expectedList = GetFinalOrderedList();
            Assert.IsTrue(expectedList.Count == res.Count);
        }

        [TestMethod]
        public void TestGetMakeImageGallary()
        {
            List<ModelImageCarousal> imageCarouselModels = GetImageCarouselModelList();
            photoBL.GetMakeImageGallary(imageCarouselModels);
            Assert.IsNotNull(imageCarouselModels);
            Assert.AreEqual(imageCarouselModels[0].Images.Count(), 6);
            Assert.AreEqual(imageCarouselModels[1].Images.Count(), 4);
            Assert.AreEqual(imageCarouselModels[2].Images.Count(), 1);
            Assert.AreEqual(imageCarouselModels[3].Images.Count(), 6);
            Assert.AreEqual(imageCarouselModels[4].Images.Count(), 4);
            Assert.AreEqual(imageCarouselModels[5].Images.Count(), 1);
            Assert.AreEqual((uint)7, imageCarouselModels[0].RecordCount);
            Assert.AreEqual("NewURL1", imageCarouselModels[0].Images[0].HostUrl);
            Assert.AreEqual("Tata1 Images Swiper", imageCarouselModels[0].ImageSwiperTitle);
            Assert.AreEqual("Tata1", imageCarouselModels[0].MakeName);
            Assert.AreEqual("Tiago1", imageCarouselModels[0].ModelName);
            Assert.AreEqual("/m/tata1-cars/tiago/images/", imageCarouselModels[0].ModelImagePageUrl);
        }

        public List<ModelImageCarousal> GetImageCarouselModelList()
        {
            return new List<ModelImageCarousal> { new ModelImageCarousal 
                                                    {
                                                        RecordCount = 7, 
                                                        Images = new List<Carwale.Entity.ViewModels.ModelImage> 
                                                        {  new Carwale.Entity.ViewModels.ModelImage 
                                                            {   HostUrl = "NewURL1",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata1" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago1", MaskingName = "tiago"}
                                                            },
                                                            new Carwale.Entity.ViewModels.ModelImage 
                                                            {   HostUrl = "NewURL2",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata2" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago2" } 
                                                            }, 
                                                            new Carwale.Entity.ViewModels.ModelImage 
                                                            {   HostUrl = "NewURL3",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata3" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago3" } 
                                                            }, 
                                                            new Carwale.Entity.ViewModels.ModelImage  
                                                            {   HostUrl = "NewURL4",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata4" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago4" } 
                                                            }, 
                                                            new Carwale.Entity.ViewModels.ModelImage  
                                                            {   HostUrl = "NewURL5",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata5" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago5" } 
                                                            }, 
                                                            new Carwale.Entity.ViewModels.ModelImage  
                                                            {   HostUrl = "NewURL6",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata6" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago6" } 
                                                            },
                                                            new Carwale.Entity.ViewModels.ModelImage  
                                                            {   HostUrl = "NewURL7",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata7" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago7" } 
                                                            }
                                                        }
                                                    },
                                                    new ModelImageCarousal 
                                                    {
                                                        RecordCount = 5, 
                                                        Images = new List<Carwale.Entity.ViewModels.ModelImage> 
                                                        {  new Carwale.Entity.ViewModels.ModelImage 
                                                            {   HostUrl = "NewURL1",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata1" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago1", MaskingName = "tiago"}
                                                            },
                                                            new Carwale.Entity.ViewModels.ModelImage 
                                                            {   HostUrl = "NewURL2",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata2" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago2" } 
                                                            }, 
                                                            new Carwale.Entity.ViewModels.ModelImage 
                                                            {   HostUrl = "NewURL3",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata3" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago3" } 
                                                            }, 
                                                            new Carwale.Entity.ViewModels.ModelImage  
                                                            {   HostUrl = "NewURL4",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata4" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago4" } 
                                                            }, 
                                                            new Carwale.Entity.ViewModels.ModelImage  
                                                            {   HostUrl = "NewURL5",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata5" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago5" } 
                                                            }
                                                        }
                                                    },
                                                     new ModelImageCarousal 
                                                    {
                                                        RecordCount = 3, 
                                                        Images = new List<Carwale.Entity.ViewModels.ModelImage> 
                                                        {  new Carwale.Entity.ViewModels.ModelImage 
                                                            {   HostUrl = "NewURL1",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata1" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago1", MaskingName = "tiago"}
                                                            },
                                                            new Carwale.Entity.ViewModels.ModelImage 
                                                            {   HostUrl = "NewURL2",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata2" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago2" } 
                                                            }, 
                                                            new Carwale.Entity.ViewModels.ModelImage 
                                                            {   HostUrl = "NewURL3",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata3" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago3" } 
                                                            }
                                                        }
                                                    },
                                                    new ModelImageCarousal 
                                                    {
                                                        RecordCount = 7, 
                                                        Images = new List<Carwale.Entity.ViewModels.ModelImage> 
                                                        {  new Carwale.Entity.ViewModels.ModelImage 
                                                            {   HostUrl = "NewURL1",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata1" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago1", MaskingName = "tiago"}
                                                            },
                                                            new Carwale.Entity.ViewModels.ModelImage 
                                                            {   HostUrl = "NewURL2",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata2" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago2" } 
                                                            }, 
                                                            new Carwale.Entity.ViewModels.ModelImage 
                                                            {   HostUrl = "NewURL3",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata3" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago3" } 
                                                            }, 
                                                            new Carwale.Entity.ViewModels.ModelImage  
                                                            {   HostUrl = "NewURL4",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata4" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago4" } 
                                                            }, 
                                                            new Carwale.Entity.ViewModels.ModelImage  
                                                            {   HostUrl = "NewURL5",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata5" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago5" } 
                                                            }, 
                                                            new Carwale.Entity.ViewModels.ModelImage  
                                                            {   HostUrl = "NewURL6",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata6" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago6" } 
                                                            },
                                                            new Carwale.Entity.ViewModels.ModelImage  
                                                            {   HostUrl = "NewURL7",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata7" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago7" } 
                                                            }
                                                        }
                                                    },
                                                    new ModelImageCarousal 
                                                    {
                                                        RecordCount = 4, 
                                                        Images = new List<Carwale.Entity.ViewModels.ModelImage> 
                                                        {  new Carwale.Entity.ViewModels.ModelImage 
                                                            {   HostUrl = "NewURL1",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata1" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago1", MaskingName = "tiago"}
                                                            },
                                                            new Carwale.Entity.ViewModels.ModelImage 
                                                            {   HostUrl = "NewURL2",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata2" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago2" } 
                                                            }, 
                                                            new Carwale.Entity.ViewModels.ModelImage 
                                                            {   HostUrl = "NewURL3",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata3" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago3" } 
                                                            }, 
                                                            new Carwale.Entity.ViewModels.ModelImage  
                                                            {   HostUrl = "NewURL4",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata4" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago4" } 
                                                            }
                                                        }
                                                    },
                                                     new ModelImageCarousal 
                                                    {
                                                        RecordCount = 3, 
                                                        Images = new List<Carwale.Entity.ViewModels.ModelImage> 
                                                        {  new Carwale.Entity.ViewModels.ModelImage 
                                                            {   HostUrl = "NewURL1",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata1" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago1", MaskingName = "tiago"}
                                                            },
                                                            new Carwale.Entity.ViewModels.ModelImage 
                                                            {   HostUrl = "NewURL2",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata2" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago2" } 
                                                            }, 
                                                            new Carwale.Entity.ViewModels.ModelImage 
                                                            {   HostUrl = "NewURL3",
                                                                MakeBase = new CarMakeEntityBase { MakeName = "Tata3" }, 
                                                                ModelBase = new CarModelEntityBase { ModelName = "Tiago3" } 
                                                            }
                                                        }
                                                    }
                                                };
        }
    }
}
