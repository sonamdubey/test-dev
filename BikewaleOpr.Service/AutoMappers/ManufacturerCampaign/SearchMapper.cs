using AutoMapper;
using Bikewale.ManufacturerCampaign.DTO.SearchCampaign;
using Bikewale.ManufacturerCampaign.Entities.SearchCampaign;
using System.Collections.Generic;
using System.Text;

namespace BikewaleOpr.Service.AutoMappers.ManufacturerCampaign
{
    public class SearchMapper
    {
        /// <summary>
        /// Created By  : Rajan Chauhan on 9 Mar 2018
        /// Description : Convert the numerical value to coressponding acronym form of week representation
        ///               eg. for 127 - [SMTWTFS], 62 - [-MTWTF-] 
        ///               num > 127 is not valid returns [-------] 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private static string convertToWeekAcronym(ushort? num)
        {
            // For handling previous null stored values also not allowing all days unchecked
            string hyphen = "-";
            if (num == null)
            {
                return "[SMTWTFS]";
            }
            else if (num > 127)
            {
                return "[-------]";
            }
            else
            {
                StringBuilder weekDaysAcronym = new StringBuilder();

                weekDaysAcronym.Append("[")
                            .Append(((num >> 6) & 1) == 1 ? "S" : hyphen)
                            .Append(((num >> 5) & 1) == 1 ? "M" : hyphen)
                            .Append(((num >> 4) & 1) == 1 ? "T" : hyphen)
                            .Append(((num >> 3) & 1) == 1 ? "W" : hyphen)
                            .Append(((num >> 2) & 1) == 1 ? "T" : hyphen)
                            .Append(((num >> 1) & 1) == 1 ? "F" : hyphen)
                            .Append(((num & 1) == 1) ? "S" : hyphen)
                            .Append("]");
                return weekDaysAcronym.ToString();
            }
        }

        internal static IEnumerable<ManufacturerCampaignDetailsDTO> Convert(IEnumerable<ManufacturerCampaignDetailsList> _objMfgList)
        {
            Mapper.CreateMap<ManufacturerCampaignDetailsList, ManufacturerCampaignDetailsDTO>().ForMember(dest => dest.CampaignDaysAcronym, opt => opt.MapFrom(src => convertToWeekAcronym(src.CampaignDays))); 

            return Mapper.Map<IEnumerable<ManufacturerCampaignDetailsList>, IEnumerable<ManufacturerCampaignDetailsDTO>>(_objMfgList);
        }
    }
}