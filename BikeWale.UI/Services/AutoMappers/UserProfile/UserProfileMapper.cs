using Bikewale.DTO.UserProfile;
using Bikewale.Entities.UserProfile;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.UserProfile
{
    public class UserProfileMapper
    {
        /// <summary>
        /// Created By : Deepak Israni on 6 July 2018
        /// Description : Mapper for converting UserProfileTargeting entity to DTO.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static UserProfileTargetingDTO Convert(UserProfileTargeting entity)
        {
            UserProfileTargetingDTO output = null;

            if (entity != null)
            {
                output = new UserProfileTargetingDTO();                

                IDictionary<string, string> targetingData = new Dictionary<string, string> ();

                foreach (KeyValuePair<UserProfileKeysEnum, string> entry in entity.TargetingData)
                {
                    targetingData.Add(new KeyValuePair<string, string>(entry.Key.ToString(), entry.Value));                    
                }

                output.TargetingData = targetingData; 
            }

            return output;
        }
    }
}