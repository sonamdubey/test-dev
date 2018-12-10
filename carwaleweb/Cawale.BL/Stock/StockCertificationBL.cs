using Carwale.Entity.Stock.Certification;
using Carwale.Interfaces.Stock;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace Carwale.BL.Stock
{
    public class StockCertificationBL : IStockCertificationBL
    {
        private readonly IStockCertificationRepository _stockCertificationRepo;
        private static readonly string _ipcQueueName = ConfigurationManager.AppSettings["IPCQueueNameMysql"].ToString();
        public const int MaxScore = 5;

        private static readonly Dictionary<CertificationCarItems, Tuple<string,string>> _carItems = new Dictionary<CertificationCarItems, Tuple<string,string>>
        {
            {CertificationCarItems.Engine, new Tuple<string,string>("Engine", "engine@3x.png")},
            {CertificationCarItems.Suspension, new Tuple<string,string>("Suspension", "suspension@3x.png")},
            {CertificationCarItems.Brakes, new Tuple<string,string>("Brakes", "brake@3x.png")},
            {CertificationCarItems.Transmission, new Tuple<string,string>("Transmission", "transmission@3x.png")},
            {CertificationCarItems.Electrical, new Tuple<string,string>("Electrical", "electric@3x.png")},
            {CertificationCarItems.AC, new Tuple<string,string>("A/C", "ac@3x.png")},
            {CertificationCarItems.Exterior, new Tuple<string,string>("Exterior", "exterior@3x.png")},
            {CertificationCarItems.Interior, new Tuple<string,string>("Interior", "interior@3x.png")},
            {CertificationCarItems.Tyres, new Tuple<string,string>("Tyres", "tyre@3x.png")},
            {CertificationCarItems.Accessories, new Tuple<string,string>("Accessories", "accessories@3x.png")}
        };

        public StockCertificationBL(IStockCertificationRepository stockCertificationrepo)
        {
            _stockCertificationRepo = stockCertificationrepo;
        }

        public StockCertification GetCarCertification(int inquiryId, bool isDealer)
        {
            StockCertification certification = null;
            if (inquiryId > 0)
            {
                certification = _stockCertificationRepo.GetStockCertification(inquiryId, isDealer);
                if (certification != null)
                {
                    certification.OverallScoreColor = GetColorHexCode(certification.OverallScoreColorId ?? 0);
                    foreach (var carItem in certification.Description)
                    {
                        carItem.Name = GetCarItemName(carItem.CarItemId ?? 0);
                        carItem.ScoreColor = GetColorHexCode(carItem.ScoreColorId ?? 0);
                    }
                }
            }
            return certification;
        }

        public void UploadCarExteriorImage(int certificationId, string imageUrl, string originalImgPath)
        {
            if (certificationId > 0 && !String.IsNullOrEmpty(imageUrl))
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("id", certificationId.ToString());
                nvc.Add("category", "stockcertification");
                nvc.Add("location", imageUrl);
                nvc.Add("originalimgpath", originalImgPath);
                nvc.Add("ismaster", Convert.ToString(1));
                nvc.Add("aspectratio", "1.777");

                RabbitMqPublish rabbitmqPublish = new RabbitMqPublish();
                rabbitmqPublish.PublishToQueue(_ipcQueueName, nvc);
            }
        }

        public string GetExteriorImagePath(string profileId, string originalImageUrl)
        {
            string extension = originalImageUrl.Substring(originalImageUrl.LastIndexOf('.'));
            if (extension.Contains("?"))
            {
                extension = extension.Substring(0, extension.IndexOf('?'));
            }
            return String.Format("/cw/ucp/carexterior/{0}_{1}{2}", profileId, DateTime.Now.Ticks, extension);
        }

        public static string GetColorHexCode(int colorId)
        {
            string colorHex;
            switch (colorId)
            {
                case 1:
                    colorHex = "12ba7e";
                    break;
                case 2:
                    colorHex = "fbb03b";
                    break;
                case 3:
                    colorHex = "d11900";
                    break;
                default:
                    colorHex = "a8a8a8";
                    break;
            }
            return colorHex;
        }

        public static string GetCarItemName(int carItemId)
        {
            Tuple<string, string> item;
            if (!_carItems.TryGetValue((CertificationCarItems)carItemId, out item))
            {
                return "Others";
            }
            return item.Item1;
        }

        public static int GetScoreFromLegendId(int legendId)
        {
            int score = -1;
            switch (legendId)
            {
                case 1:
                    score = 100;
                    break;
                case 2:
                    score = 80;
                    break;
                case 3:
                    score = 60;
                    break;
                case 4:
                    score = 40;
                    break;
                case 5:
                    score = 20;
                    break;
            }
            return score;
        }

        public static string GetColorFromLegendId(int legendId)
        {
            int colorId = 0;
            switch (legendId)
            {
                case 1:
                case 6:
                    colorId = 1;
                    break;
                case 2:
                case 3:
                    colorId = 2;
                    break;
                case 4:
                case 5:
                case 7:
                    colorId = 3;
                    break;
            }
            return GetColorHexCode(colorId);
        }

        public static string GetCarItemImageName(int carItemId)
        {
            Tuple<string, string> item;
            if (!_carItems.TryGetValue((CertificationCarItems)carItemId, out item))
            {
                return "exterior@3x.png";
            }
            return item.Item2;
        }

        public static string FormatCertificationScore(decimal? score)
        {
            return score != null ? score + "/" + MaxScore : null;
        }
    }
}
