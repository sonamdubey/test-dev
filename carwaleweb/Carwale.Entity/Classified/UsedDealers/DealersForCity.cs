using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.UsedDealers
{
    [Serializable]
    public class DealersForCity
    {    
        public int DealerId { get; set; }
        public int CityId { get; set; }

        private string m_Organization;
        public string Organization
        {
            get
            {
                if (string.IsNullOrEmpty(m_Organization))
                    return string.Empty;
                else
                    return m_Organization;
            }
            set
            {
                m_Organization = value;
            }
        }
        public long TotalView { get; set; }
        public long TotalCars { get; set; }

        private string m_ShowroomImg;
        public string ShowroomImg
        {
            get
            {
                if (string.IsNullOrEmpty(m_ShowroomImg))
                    return string.Empty;
                else
                    return m_ShowroomImg;
            }
            set
            {
                m_ShowroomImg = value;
            }
        }


        private string m_HostUrl;
        public string HostUrl
        {
            get
            {
                if (string.IsNullOrEmpty(m_HostUrl))
                    return string.Empty;
                else
                    return m_HostUrl;
            }
            set
            {
                m_HostUrl = value;
            }
        }


        private string m_OriginalImgPath;
        public string OriginalImgPath
        {
            get
            {
                if (string.IsNullOrEmpty(m_OriginalImgPath))
                    return string.Empty;
                else
                    return m_OriginalImgPath;
            }
            set
            {
                m_OriginalImgPath = value;
            }
        }


        private string m_ActiveMaskingNumber;
        public string ActiveMaskingNumber
        {
            get
            {
                if (string.IsNullOrEmpty(m_ActiveMaskingNumber))
                    return string.Empty;
                else
                    return m_ActiveMaskingNumber;
            }
            set
            {
                m_ActiveMaskingNumber = value;
            }
        }

        public bool IsWebSiteAvailable { get; set; }
        public string WebsiteUrl { get; set; }


    }
}
