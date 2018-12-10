IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AP_VerifiedListingAlerts_Test]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AP_VerifiedListingAlerts_Test]
GO

	
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- Modified by: Manish on 23-10-2013 adding the condition "CSI.ID NOT IN (SELECT SellInquiryId  FROM AP_DealerPackageInquiries)" in both below select queries
-- =============================================
CREATE PROCEDURE [dbo].[AP_VerifiedListingAlerts_Test] 

AS
BEGIN
		SELECT TOP 1 VSI.SellInqId, CSI.CustomerId, CSI.Kilometers, CSI.Price, CSI.MakeYear, C.Id AS CityId, 
			  CSI.Color, C.Name AS City, CSID.Owners, CU.Name, CU.email, CU.Address, CSI.PackageType, 
			  CU.Mobile, (CMA.Name + ' ' + CMO.Name + ' ' + CV.Name) AS CarName, CSID.RegistrationPlace, 
			  CMA.Id AS MakeId, CMO.Id AS ModelId, '1' AS MailStatus, CarRegState, CSI.Comments,  
			  CSI.CarVersionId, MONTH(CSI.MakeYear) AS CarMonth, YEAR(CSI.MakeYear) AS CarYear, CSI.CarRegNo, 
			  CSID.Insurance, CSID.InsuranceExpiry, CSID.Tax,CSID.InteriorColor,  
			  CSID.CityMileage,CSID.AdditionalFuel,CSID.CarDriven,
			  CASE CSID.Accidental WHEN 1 THEN 'True' ELSE 'False' END AS Accidental,
			  CASE CSID.FloodAffected WHEN 1 THEN 'True' ELSE 'False' END AS FloodAffected, 
			  CSID.Accessories,CSID.Warranties,CSID.Modifications,CSID.ACCondition,CSID.BatteryCondition, 
			  CSID.BrakesCondition,CSID.ElectricalsCondition,CSID.EngineCondition,CSID.ExteriorCondition, 
			  CSID.SeatsCondition,CSID.SuspensionsCondition,CSID.TyresCondition,CSID.InteriorCondition, 
			  CSID.OverallCondition,CSID.Features_SafetySecurity,CSID.Features_Others,CSID.Features_Comfort  
                
		FROM CustomerSellInquiries  AS CSI  
              LEFT JOIN CustomerSellInquiryDetails AS CSID WITH(NOLOCK) ON  CSID.InquiryId=CSI.ID  
              INNER JOIN AP_VerifiedSellInq AS VSI WITH(NOLOCK) ON  CSI.ID=VSI.SellInqId  
              INNER JOIN Customers AS CU WITH(NOLOCK) ON  CU.Id=CSI.CustomerId  
              INNER JOIN Cities AS C WITH(NOLOCK) ON C.ID=CSI.CityId  
              INNER JOIN CarVersions AS CV WITH(NOLOCK) ON CV.ID=CSI.CarVersionId  
              INNER JOIN CarModels AS CMO WITH(NOLOCK) ON CMO.ID=CV.CarModelId  
              INNER JOIN CarMakes AS CMA WITH(NOLOCK) ON CMA.ID= CMO.CarMakeId  
			WHERE Status = 1
		    AND  CSI.ID NOT IN (SELECT SellInquiryId  FROM AP_DealerPackageInquiries WITH (NOLOCK))  -- condition added by manish on 23-10-2013
		
		SELECT TOP 1 VSI.SellInqId, CSI.CustomerId, CSI.Kilometers, CSI.Price, CSI.MakeYear, C.Id AS CityId, 
                  CSI.Color, C.Name AS City, CSID.Owners, CU.Name, CU.email, CU.Address, CSI.PackageType, 
                  CU.Mobile, (CMA.Name + ' ' + CMO.Name + ' ' + CV.Name) AS CarName, CSID.RegistrationPlace, 
                  CMA.Id AS MakeId, CMO.Id AS ModelId, '0' AS MailStatus, CarRegState, 
                  CSI.CarVersionId, MONTH(CSI.MakeYear) AS CarMonth, YEAR(CSI.MakeYear) AS CarYear, 
                  CSI.CarRegNo, CSI.Comments,  
                  CSID.Insurance, CSID.InsuranceExpiry, CSID.Tax,CSID.InteriorColor, 
                  CSID.CityMileage,CSID.AdditionalFuel,CSID.CarDriven,
                  CASE CSID.Accidental WHEN 1 THEN 'True' ELSE 'False' END AS Accidental,
				  CASE CSID.FloodAffected WHEN 1 THEN 'True' ELSE 'False' END AS FloodAffected,
                  CSID.Accessories,CSID.Warranties,CSID.Modifications,CSID.ACCondition,CSID.BatteryCondition, 
                  CSID.BrakesCondition,CSID.ElectricalsCondition,CSID.EngineCondition,CSID.ExteriorCondition, 
                  CSID.SeatsCondition,CSID.SuspensionsCondition,CSID.TyresCondition,CSID.InteriorCondition, 
                  CSID.OverallCondition,CSID.Features_SafetySecurity,CSID.Features_Others,CSID.Features_Comfort 

		FROM CustomerSellInquiries  AS CSI  
			  LEFT JOIN CustomerSellInquiryDetails AS CSID WITH(NOLOCK) ON  CSID.InquiryId=CSI.ID  
			  INNER JOIN AP_VerifiedSellInq AS VSI WITH(NOLOCK) ON  CSI.ID=VSI.SellInqId  
			  INNER JOIN Customers AS CU WITH(NOLOCK) ON  CU.Id=CSI.CustomerId  
			  INNER JOIN Cities AS C WITH(NOLOCK) ON C.ID=CSI.CityId  
			  INNER JOIN CarVersions AS CV WITH(NOLOCK) ON CV.ID=CSI.CarVersionId   
			  INNER JOIN CarModels AS CMO WITH(NOLOCK) ON CMO.ID=CV.CarModelId  
			  INNER JOIN CarMakes AS CMA WITH(NOLOCK) ON CMA.ID= CMO.CarMakeId  
         WHERE VSI.VerificationDate <= DATEADD(hh,-24,GETDATE())
		AND  CSI.ID NOT IN (SELECT SellInquiryId  FROM AP_DealerPackageInquiries  WITH (NOLOCK))   -- condition added by manish on 23-10-2013
END

