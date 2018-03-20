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
        ///               eg. for 127 - All Days, 3 - Fri,Sat 
        ///               num > 127 is not valid returns [-------] 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static string ConvertToWeekDaysName(ushort? number)
        {
            // For handling previous null stored values also not allowing all days unchecked
            if (number == null || number == 127)
            {
                return "All Days";
            }
            else
            {
                StringBuilder sbWeekDays = new StringBuilder();

                sbWeekDays.Append(((number >> 6) & 1) == 1 ? "Sun, " : "")
                            .Append(((number >> 5) & 1) == 1 ? "Mon, " : "")
                            .Append(((number >> 4) & 1) == 1 ? "Tue, " : "")
                            .Append(((number >> 3) & 1) == 1 ? "Wed, " : "")
                            .Append(((number >> 2) & 1) == 1 ? "Thr, " : "")
                            .Append(((number >> 1) & 1) == 1 ? "Fri, " : "")
                            .Append(((number & 1) == 1) ? "Sat, " : "");

                if (sbWeekDays.Length > 1)
                {
                    // To remove the last comma and space in the string builder
                    sbWeekDays.Length -= 2;
                }
                return sbWeekDays.ToString();
            }
        }

        internal static IEnumerable<ManufacturerCampaignDetailsDTO> Convert(IEnumerable<ManufacturerCampaignDetailsList> _objMfgList)
        {
            Mapper.CreateMap<ManufacturerCampaignDetailsList, ManufacturerCampaignDetailsDTO>().ForMember(dest => dest.CampaignDays, opt => opt.MapFrom(src => ConvertToWeekDaysName(src.CampaignDays))); 

            return Mapper.Map<IEnumerable<ManufacturerCampaignDetailsList>, IEnumerable<ManufacturerCampaignDetailsDTO>>(_objMfgList);
        }
    }
}