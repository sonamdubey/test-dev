using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CompareCars
{
    [Serializable]
    public class CCarData
    {
        public List<SubCategory> Specs = new List<SubCategory>();
        public List<SubCategory> Features = new List<SubCategory>();
        public List<Item> OverView = new List<Item>();
        public List<List<Color>> Colors = new List<List<Color>>();
        public List<int> ValidVersionIds = new List<int>();
        public List<CarWithImageEntity> CarDetails = new List<CarWithImageEntity>();
        public int FeaturedVersionId = -1;
        public FeaturedCarDataEntity FeaturedCarData { get; set; }
    }

    [Serializable]
    public class SubCategory : IComparable
    {
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public List<Item> Items { get; set; }

        public int CompareTo(Object obj)
        {
            SubCategory item = (SubCategory)obj;
            int retval = 0;
            if (item.SortOrder < SortOrder && item.SortOrder != 0)
            {
                retval = 1;
            }
            else if (item.SortOrder > SortOrder && SortOrder != 0)
            {
                retval = -1;
            }

            return retval;
        }
    }

    [Serializable]
    public class Item : IComparable
    {
        public string ItemMasterId { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public string UnitType { get; set; }
        public List<string> Values { get; set; }
        public string ItemValue { get; set; }
        public int DataTypeId { get; set; }

        public int CompareTo(Object obj)
        {
            Item item = (Item)obj;
            int retval = 0;
            if (item.SortOrder < SortOrder && item.SortOrder != 0)
            {
                retval = 1;
            }
            else if (item.SortOrder > SortOrder && SortOrder != 0)
            {
                retval = -1;
            }

            return retval;
        }
    }

    [Serializable]
    public class Color
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    [Serializable]
    public class SubCategoryData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NodeCode { get; set; }
        public string SortOrder { get; set; }
        public List<Item> Items { get; set; }
    }

    [Serializable]
    public class ItemData
    {
        public string ItemMasterId { get; set; }
        public string Name { get; set; }
        public string NodeCode { get; set; }
        public string SortOrder { get; set; }
        public string OverviewSortOrder { get; set; }
        public string UnitType { get; set; }
        public bool IsOverviewable { get; set; }
        public List<string> Values { get; set; }
    }

    [Serializable]
    public class ValueData
    {
        public string ItemMasterId { get; set; }
        public string Value { get; set; }
    }    
}
