IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AP_VerifiedListingAlerts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AP_VerifiedListingAlerts]
GO

	
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- Modified by: Manish on 23-10-2013 adding the condition "CSI.ID NOT IN (SELECT SellInquiryId  FROM AP_DealerPackageInquiries)" in both below select queries
-- Modified by: Sachin Bharti on 18th Feb 2013
-- Modified by: :Increased delay of 30 mins for Verified Inquiries by Deepak 16th may 2016
-- EXEC [dbo].[AP_VerifiedListingAlerts] 
-- =============================================
CREATE PROCEDURE [dbo].[AP_VerifiedListingAlerts] 

AS
BEGIN
		DECLARE @SellInqId INT
		SELECT TOP 1 @SellInqId = VSI.SellInqId
		FROM AP_VerifiedSellInq AS VSI WITH(NOLOCK) 
		WHERE VSI.VerificationDate <= DATEADD(mi,-30,GETDATE()) AND VSI.IsProcessed = 0 
		ORDER BY Status
		
		
		SELECT CSI.Id AS SellInqId, CSI.CustomerId, CSI.Kilometers, CSI.Price, CSI.MakeYear, C.Id AS CityId, 
			  CSI.Color, C.Name AS City, ISNULL(CSID.Owners,0)AS Owners, CU.Name, CU.email, CU.Address, CSI.PackageType, 
			  CU.Mobile, CU.phone1 AS PhoneNo,
			  CMA.Name AS CarMake,CMO.Name AS CarModel,CV.Name AS VersionName,
			  (CMA.Name + ' ' + CMO.Name + ' ' + CV.Name) AS CarName, CSID.RegistrationPlace, 
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
                
		FROM CustomerSellInquiries  AS CSI WITH(NOLOCK)
              LEFT JOIN CustomerSellInquiryDetails AS CSID WITH(NOLOCK) ON  CSID.InquiryId=CSI.ID  
              INNER JOIN Customers AS CU WITH(NOLOCK) ON  CU.Id=CSI.CustomerId  
              INNER JOIN Cities AS C WITH(NOLOCK) ON C.ID=CSI.CityId  
              INNER JOIN CarVersions AS CV WITH(NOLOCK) ON CV.ID=CSI.CarVersionId  
              INNER JOIN CarModels AS CMO WITH(NOLOCK) ON CMO.ID=CV.CarModelId  
              INNER JOIN CarMakes AS CMA WITH(NOLOCK) ON CMA.ID= CMO.CarMakeId  
		WHERE CSI.ID = @SellInqId
			
		UPDATE AP_VerifiedSellInq SET IsProcessed = 1, ProcessDate = GETDATE() WHERE SellInqId = @SellInqId 
		
		--Previous Logic Commented By Deepak on 8th Nov 2016
		--SELECT TOP 1 VSI.SellInqId, CSI.CustomerId, CSI.Kilometers, CSI.Price, CSI.MakeYear, C.Id AS CityId, 
		--	  CSI.Color, C.Name AS City, ISNULL(CSID.Owners,0)AS Owners, CU.Name, CU.email, CU.Address, CSI.PackageType, 
		--	  CU.Mobile, CU.phone1 AS PhoneNo,
		--	  CMA.Name AS CarMake,CMO.Name AS CarModel,CV.Name AS VersionName,
		--	  (CMA.Name + ' ' + CMO.Name + ' ' + CV.Name) AS CarName, CSID.RegistrationPlace, 
		--	  CMA.Id AS MakeId, CMO.Id AS ModelId, '1' AS MailStatus, CarRegState, CSI.Comments,  
		--	  CSI.CarVersionId, MONTH(CSI.MakeYear) AS CarMonth, YEAR(CSI.MakeYear) AS CarYear, CSI.CarRegNo, 
		--	  CSID.Insurance, CSID.InsuranceExpiry, CSID.Tax,CSID.InteriorColor,  
		--	  CSID.CityMileage,CSID.AdditionalFuel,CSID.CarDriven,
		--	  CASE CSID.Accidental WHEN 1 THEN 'True' ELSE 'False' END AS Accidental,
		--	  CASE CSID.FloodAffected WHEN 1 THEN 'True' ELSE 'False' END AS FloodAffected, 
		--	  CSID.Accessories,CSID.Warranties,CSID.Modifications,CSID.ACCondition,CSID.BatteryCondition, 
		--	  CSID.BrakesCondition,CSID.ElectricalsCondition,CSID.EngineCondition,CSID.ExteriorCondition, 
		--	  CSID.SeatsCondition,CSID.SuspensionsCondition,CSID.TyresCondition,CSID.InteriorCondition, 
		--	  CSID.OverallCondition,CSID.Features_SafetySecurity,CSID.Features_Others,CSID.Features_Comfort
                
		--FROM CustomerSellInquiries  AS CSI WITH(NOLOCK)
  --            LEFT JOIN CustomerSellInquiryDetails AS CSID WITH(NOLOCK) ON  CSID.InquiryId=CSI.ID  
  --            INNER JOIN AP_VerifiedSellInq AS VSI WITH(NOLOCK) ON  CSI.ID=VSI.SellInqId  
  --            INNER JOIN Customers AS CU WITH(NOLOCK) ON  CU.Id=CSI.CustomerId  
  --            INNER JOIN Cities AS C WITH(NOLOCK) ON C.ID=CSI.CityId  
  --            INNER JOIN CarVersions AS CV WITH(NOLOCK) ON CV.ID=CSI.CarVersionId  
  --            INNER JOIN CarModels AS CMO WITH(NOLOCK) ON CMO.ID=CV.CarModelId  
  --            INNER JOIN CarMakes AS CMA WITH(NOLOCK) ON CMA.ID= CMO.CarMakeId  
		--	WHERE CSI.EntryDate <= DATEADD(mi,-30,GETDATE()) -- Increased delay of 30 mins by Deepak 16th may 2016
		--		AND  CSI.ID NOT IN (SELECT SellInquiryId  FROM AP_DealerPackageInquiries WITH (NOLOCK))  -- condition added by manish on 23-10-2013
		--		AND Status = 1  
		
		--SELECT TOP 1 VSI.SellInqId, CSI.CustomerId, CSI.Kilometers, CSI.Price, CSI.MakeYear, C.Id AS CityId, 
  --                CSI.Color, C.Name AS City, ISNULL(CSID.Owners,0)AS Owners, CU.Name, CU.email, CU.Address, CSI.PackageType, 
  --                CU.Mobile,CU.phone1 AS PhoneNo,
		--		  CMA.Name AS CarMake,CMO.Name AS CarModel,CV.Name AS VersionName,
		--		  (CMA.Name + ' ' + CMO.Name + ' ' + CV.Name) AS CarName, CSID.RegistrationPlace, 
  --                CMA.Id AS MakeId, CMO.Id AS ModelId, '0' AS MailStatus, CarRegState, 
  --                CSI.CarVersionId, MONTH(CSI.MakeYear) AS CarMonth, YEAR(CSI.MakeYear) AS CarYear, 
  --                CSI.CarRegNo, CSI.Comments,  
  --                CSID.Insurance, CSID.InsuranceExpiry, CSID.Tax,CSID.InteriorColor, 
  --                CSID.CityMileage,CSID.AdditionalFuel,CSID.CarDriven,
  --                CASE CSID.Accidental WHEN 1 THEN 'True' ELSE 'False' END AS Accidental,
		--		  CASE CSID.FloodAffected WHEN 1 THEN 'True' ELSE 'False' END AS FloodAffected,
  --                CSID.Accessories,CSID.Warranties,CSID.Modifications,CSID.ACCondition,CSID.BatteryCondition, 
  --                CSID.BrakesCondition,CSID.ElectricalsCondition,CSID.EngineCondition,CSID.ExteriorCondition, 
  --                CSID.SeatsCondition,CSID.SuspensionsCondition,CSID.TyresCondition,CSID.InteriorCondition, 
  --                CSID.OverallCondition,CSID.Features_SafetySecurity,CSID.Features_Others,CSID.Features_Comfort 

		--FROM CustomerSellInquiries  AS CSI WITH(NOLOCK)
		--	  LEFT JOIN CustomerSellInquiryDetails AS CSID WITH(NOLOCK) ON  CSID.InquiryId=CSI.ID  
		--	  INNER JOIN AP_VerifiedSellInq AS VSI WITH(NOLOCK) ON  CSI.ID=VSI.SellInqId  
		--	  INNER JOIN Customers AS CU WITH(NOLOCK) ON  CU.Id=CSI.CustomerId  
		--	  INNER JOIN Cities AS C WITH(NOLOCK) ON C.ID=CSI.CityId  
		--	  INNER JOIN CarVersions AS CV WITH(NOLOCK) ON CV.ID=CSI.CarVersionId   
		--	  INNER JOIN CarModels AS CMO WITH(NOLOCK) ON CMO.ID=CV.CarModelId  
		--	  INNER JOIN CarMakes AS CMA WITH(NOLOCK) ON CMA.ID= CMO.CarMakeId  
  --       WHERE VSI.VerificationDate <= DATEADD(hh,-24,GETDATE())
		--	AND  CSI.ID NOT IN (SELECT SellInquiryId  FROM AP_DealerPackageInquiries  WITH (NOLOCK))   -- condition added by manish on 23-10-2013
END



