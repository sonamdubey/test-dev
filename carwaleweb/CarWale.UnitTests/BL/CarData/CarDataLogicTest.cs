using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carwale.DAL.ApiGateway;
using Moq;
using System.Collections.Generic;
using Carwale.Entity.CarData;
using Carwale.Entity;
using Carwale.BL.CarData;

namespace CarWale.UnitTests.BL.CarData
{
    [TestClass]
    public class CarDataLogicTest
    {
        Mock<IApiGatewayCaller> _apiGatewayCaller;
       

        [TestMethod]
        public void GetCombinedCarData()
        {
            _apiGatewayCaller = new Mock<IApiGatewayCaller>();
             List<int> versionIds = new List<int>
             {
                 4457,
                 4458,
                 4458
             }; 
            var itemList = new List<CategoryItem>();
            var categoryList = new List<Carwale.Entity.CarData.CarData>();
            string value = "4272";
             
          
            var item = new CategoryItem
            {
                Id = 1,
                Name = "Length",
                UnitTypeName="mm",
                Value = value,
                SortOrder = 1
            };
            itemList.Add(item);
            var category = new Carwale.Entity.CarData.CarData
            {
                CarDataType = ComponentType.Specs,
                CategoryName = "Dimension & Weight",
                SortOrder = 1,
                Items = itemList               
            };
            categoryList.Add(category);
            var result = new CarDataPresentation
            {
                Specifications = categoryList,
                Features = categoryList,
                Overview = itemList
            };            
        }
    }
}
