using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.UsedDealers
{
    public class PremiumDealer
    {
        private string m_profileid;
        private string m_car;
        private string m_makeyear;
        private string m_makename;
        private string m_modelname;
        private string m_cityname;
        private string m_imageurlmedium;
        private string m_imageurlthumbsmall;
        private string m_hosturl;
        private string m_directorypath;
        private string m_originalimgpath;
        private string m_maskingname;

        public string Profileid
        {
            get
            {
                if (string.IsNullOrEmpty(m_profileid))
                    return string.Empty;
                else
                    return m_profileid;
            }
            set
            {
                m_profileid = value;
            }
        }
        public string Car
        {
            get
            {
                if (string.IsNullOrEmpty(m_car))
                    return string.Empty;
                else
                    return m_car;
            }
            set
            {
                m_car = value;
            }
        }
        public string Makeyear
        {
            get
            {
                if (string.IsNullOrEmpty(m_makeyear))
                    return string.Empty;
                else
                    return m_makeyear;
            }
            set
            {
                m_makeyear = value;
            }
        }
        public string Makename
        {
            get
            {
                if (string.IsNullOrEmpty(m_makename))
                    return string.Empty;
                else
                    return m_makename;
            }
            set
            {
                m_makename = value;
            }
        }
        public string Modelname
        {
            get
            {
                if (string.IsNullOrEmpty(m_modelname))
                    return string.Empty;
                else
                    return m_modelname;
            }
            set
            {
                m_modelname = value;
            }
        }
        public string Cityname
        {
            get
            {
                if (string.IsNullOrEmpty(m_cityname))
                    return string.Empty;
                else
                    return m_cityname;
            }
            set
            {
                m_cityname = value;
            }
        }
        public decimal Price { get; set; }
        public decimal Kilometers { get; set; }
        public Int16 Certificationid { get; set; }
        public string Imageurlmedium
        {
            get
            {
                if (string.IsNullOrEmpty(m_imageurlmedium))
                    return string.Empty;
                else
                    return m_imageurlmedium;
            }
            set
            {
                m_imageurlmedium = value;
            }
        }
        public string Imageurlthumbsmall
        {
            get
            {
                if (string.IsNullOrEmpty(m_imageurlthumbsmall))
                    return string.Empty;
                else
                    return m_imageurlthumbsmall;
            }
            set
            {
                m_imageurlthumbsmall = value;
            }
        }
        public string Hosturl
        {
            get
            {
                if (string.IsNullOrEmpty(m_hosturl))
                    return string.Empty;
                else
                    return m_hosturl;
            }
            set
            {
                m_hosturl = value;
            }
        }
        public string Directorypath
        {
            get
            {
                if (string.IsNullOrEmpty(m_directorypath))
                    return string.Empty;
                else
                    return m_directorypath;
            }
            set
            {
                m_directorypath = value;
            }
        }
        public string Originalimgpath
        {
            get
            {
                if (string.IsNullOrEmpty(m_originalimgpath))
                    return string.Empty;
                else
                    return m_originalimgpath;
            }
            set
            {
                m_originalimgpath = value;
            }
        }
        public string Maskingname
        {
            get
            {
                if (string.IsNullOrEmpty(m_maskingname))
                    return string.Empty;
                else
                    return m_maskingname;
            }
            set
            {
                m_maskingname = value;
            }
        }

    }
}
