IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_SoldWarrantyCustomerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_SoldWarrantyCustomerDetails]
GO

	-- =============================================
-- Author      : Yuga Hatolkar
-- Create date : 19th Oct, 2015
-- Description : To get Customer Details with Sold Warranties.
-- Modified By : Kartik Rathod on 21 Oct, 2015 (added expirydate column and logic for the extended warranty in case of datediff between 28,29,30 days)
 --Modified By : Nilima More on 27th oct 2015 (fetching customer details with AbSureWarrantyActivationStatusesId =1(Request for warranty))
-- =============================================
CREATE PROCEDURE [dbo].[Absure_SoldWarrantyCustomerDetails] 
	@CarId       BIGINT = null	
AS
BEGIN
	SET NOCOUNT ON;
		IF @CarId IS NOT NULL
		BEGIN
				SELECT	ACD.PolicyNo,AA.AbSure_ActivatedWarrantyId ,
						AA.AbSure_CarDetailsId AS Id,
						AA.CustName + ' - ' + AA.Mobile AS Customer,
						AA.CustName AS CustName, AA.Mobile AS Mobile, 
						AA.AlternatePhone, 
						AA.Email, 
						REPLACE(REPLACE(REPLACE(AA.Address, ' ', '*^'), '^*', ''), '*^', ' ') AS Address,  -- Added by Yuga: To handle blank spaces in address
						--AA.Address, 
						C.Name AS City, 
						S.Name AS State, 
						'India' AS Country,
						A.PinCode AS [Post code], 
						AA.DealerId,ISNULL(D.FirstName, '') + ' ' + ISNULL(D.LastName, '') AS DealerName, 
						D.Organization AS [Dealer Company Name], 
						ACD.Make AS Make, 
						ACD.Model AS Model, 
						ACD.Version  AS Version, 
						AA.MakeYear AS MakeYear,
						AA.RegistrationDate[First Registration date of Vehicle], 
						ACD.Make AS Make, ACD.Model AS Model, 
						ACD.Version AS Version,
						AA.RegNumber[Vehicle_registration_No], 
						ACD.Colour, 
						AA.Kilometer [Current Odometer Reading], 
						AA.Kilometer+15000 AS [Odometer Reading valid till (kms)], 
						AWT.Warranty [Warranty_Type],
						ACD.VIN AS VIN,AA.EntryDate,
						--AA.AbSure_ActivatedWarrantyId AS [CW Extended  Warranty Policy no.], 
						ACD.SurveyDate, 
						AA.WarrantyStartDate, 
						AA.WarrantyEndDate,
						AA.EngineNo,AA.ChassisNo, 
						CSM.TC_StockId AS StockId,
						D.EmailId AS DealerEmailId, 
						D.MobileNo AS DealerMobile, 
						D.Address1 AS DealerAddress,
						AWT.AbSure_WarrantyTypesId AS AbSure_WarrantyTypes,
						'http://www.autobiz.in/absure/carcertificate.aspx?carid='+CAST(ACD.id as varchar(1000)) ViewReport, 
						ROW_NUMBER() OVER (PARTITION BY AA.CustName ORDER BY AA.AbSure_ActivatedWarrantyId DESC) AS ROW,
						AA.AbSure_CarDetailsId AS CarId,AA.WarrantyStartDate,
						AA.WarrantyEndDate
					
				FROM			AbSure_ActivatedWarranty AA WITH(NOLOCK) 
				LEFT JOIN		AbSure_CarDetails ACD WITH(NOLOCK) ON AA.AbSure_CarDetailsId = ACD.Id
								
				LEFT JOIN		Cities C WITH(NOLOCK) ON ACD.OwnerCityId = C.ID
				LEFT JOIN		States AS S WITH(NOLOCK) ON C.StateId = S.ID
				LEFT JOIN		Areas A WITH(NOLOCK) ON A.ID = ACD.OwnerAreaId				
				LEFT JOIN		AbSure_WarrantyTypes AWT WITH(NOLOCK) ON AA.WarrantyTypeId = AWT.AbSure_WarrantyTypesId
				LEFT JOIN		Dealers D WITH(NOLOCK) ON aa.DealerId=d.ID 
				LEFT JOIN		AbSure_CarSurveyorMapping CSM WITH(NOLOCK) ON AA.AbSure_CarDetailsId = CSM.AbSure_CarDetailsId
				
				WHERE			ACD.Id = @CarId
		END
		ELSE
		BEGIN
			SELECT		ACD.PolicyNo,
						AA.AbSure_WarrantyActivationPendingId ,
						AA.DuplicateCarId AS Id,
						AA.CustName + ' - ' + AA.Mobile AS Customer,
						AA.CustName AS CustName, AA.Mobile AS Mobile, 
						AA.AlternatePhone, 
						AA.Email, 
						REPLACE(REPLACE(REPLACE(AA.Address, ' ', '*^'), '^*', ''), '*^', ' ') AS Address,	-- Added by Yuga: To handle blank spaces in address
						--AA.Address, 
						C.Name AS City, 
						S.Name AS State, 
						'India' AS Country,
						A.PinCode AS [Post code], 
						AA.DealerId,ISNULL(D.FirstName, '') + ' ' + ISNULL(D.LastName, '') AS DealerName, 
						D.Organization AS [Dealer Company Name], 
						ACD.Make AS Make, 
						ACD.Model AS Model, 
						ACD.Version  AS Version, 
						AA.MakeYear AS MakeYear,
						AA.RegistrationDate[First Registration date of Vehicle], 
						AA.RegNumber[Vehicle_registration_No], 
						ACD.Colour, 
						AA.Kilometer [Current Odometer Reading], 
						AA.Kilometer+15000 AS [Odometer Reading valid till (kms)], 
						AWT.Warranty [Warranty_Type],
						ACD.VIN AS VIN,AA.EntryDate,
						--AA.AbSure_ActivatedWarrantyId AS [CW Extended  Warranty Policy no.], 
						ACD.SurveyDate, 
						AA.WarrantyStartDate, 
						AA.WarrantyEndDate,
						AA.EngineNo,AA.ChassisNo, 
						CSM.TC_StockId AS StockId,
						D.EmailId AS DealerEmailId, 
						D.MobileNo AS DealerMobile, 
						D.Address1 AS DealerAddress,
						case AWT.AbSure_WarrantyTypesId when 1 then 'Gold' when 2 then 'Silver' end AbSure_WarrantyTypes,
						'http://www.autobiz.in/absure/carcertificate.aspx?carid='+CAST(ACD.id as varchar(1000)) ViewReport, 
						ROW_NUMBER() OVER (PARTITION BY AA.CustName ORDER BY AA.AbSure_WarrantyActivationPendingId DESC) AS ROW,
						AA.AbSure_CarDetailsId AS CarId,AA.WarrantyStartDate,
						AA.WarrantyEndDate,AA.IsCarTradeWarranty
					
				FROM			AbSure_WarrantyActivationPending AA WITH(NOLOCK) 
				LEFT JOIN		AbSure_CarDetails ACD WITH(NOLOCK) ON AA.AbSure_CarDetailsId = ACD.Id				
				LEFT JOIN		Cities C WITH(NOLOCK) ON ACD.OwnerCityId = C.ID
				LEFT JOIN		States AS S WITH(NOLOCK) ON C.StateId = S.ID
				LEFT JOIN		Areas A WITH(NOLOCK) ON A.ID = ACD.OwnerAreaId				
				LEFT JOIN		AbSure_WarrantyTypes AWT WITH(NOLOCK) ON AA.WarrantyTypeId = AWT.AbSure_WarrantyTypesId
				LEFT JOIN		Dealers D WITH(NOLOCK) ON aa.DealerId=d.ID 
				LEFT JOIN		AbSure_CarSurveyorMapping CSM WITH(NOLOCK) ON AA.AbSure_CarDetailsId = CSM.AbSure_CarDetailsId				
				WHERE			(DATEDIFF(DAY,ACD.SurveyDate,GETDATE()) <=30)
								AND ACD.AbSureWarrantyActivationStatusesId = 1 AND AA.IsActive = 1 
				UNION
				SELECT  '',
						AA.AbSure_WarrantyActivationPendingId ,
						AA.AbSure_CarDetailsId AS Id,
						AA.CustName + ' - ' + AA.Mobile AS Customer,
						AA.CustName AS CustName, AA.Mobile AS Mobile, 
						AA.AlternatePhone, 
						AA.Email, 
						REPLACE(REPLACE(REPLACE(AA.Address, ' ', '*^'), '^*', ''), '*^', ' ') AS Address,	-- Added by Yuga: To handle blank spaces in address
						--AA.Address, 
						C.Name AS City, 
						S.Name AS State, 
						'India' AS Country,
						A.PinCode AS [Post code], 
						AA.DealerId,ISNULL(D.FirstName, '') + ' ' + ISNULL(D.LastName, '') AS DealerName, 
						D.Organization AS [Dealer Company Name], 
						TR.Make AS Make, 
						TR.Model AS Model, 
						TR.Variant  AS Version, 
						TR.ManufacturingYear AS MakeYear,
						AA.RegistrationDate[First Registration date of Vehicle], 						
						AA.RegNumber[Vehicle_registration_No], 
						TR.Color, 
						AA.Kilometer [Current Odometer Reading], 
						AA.Kilometer+15000 AS [Odometer Reading valid till (kms)], 
						'' [Warranty_Type],
						'' AS VIN,AA.EntryDate,
						--AA.AbSure_ActivatedWarrantyId AS [CW Extended  Warranty Policy no.], 
						TC.InvCertifiedDate, 
						AA.WarrantyStartDate, 
						AA.WarrantyEndDate,
						AA.EngineNo,AA.ChassisNo, 
						TR.ListingId AS StockId,
						D.EmailId AS DealerEmailId, 
						D.MobileNo AS DealerMobile, 
						D.Address1 AS DealerAddress,
						case TC.IsWarranty when 1 then 'Gold' when 2 then 'Silver' end AbSure_WarrantyTypes,
						'http://www.autobiz.in/absure/carcertificatenew.aspx?stockid='+CAST(TR.ListingId as varchar(1000)) ViewReport, 
						ROW_NUMBER() OVER (PARTITION BY AA.CustName ORDER BY AA.AbSure_WarrantyActivationPendingId DESC) AS ROW,
						AA.AbSure_CarDetailsId AS CarId,AA.WarrantyStartDate,
						AA.WarrantyEndDate,AA.IsCarTradeWarranty
				FROM			AbSure_WarrantyActivationPending AA WITH(NOLOCK) 			
				LEFT JOIN		Dealers D WITH(NOLOCK) ON aa.DealerId=d.ID 
				LEFT JOIN		Cities C WITH(NOLOCK) ON D.CityId = C.ID
				LEFT JOIN		States AS S WITH(NOLOCK) ON C.StateId = S.ID
				LEFT JOIN		Areas A WITH(NOLOCK) ON A.ID = D.AreaId
				LEFT JOIN       TC_CarTradeCertificationLiveListing TL WITH(NOLOCK) ON AA.AbSure_CarDetailsId = TL.TC_CarTradeCertificationRequestId
				LEFT JOIN       TC_CarTradeCertificationRequests TR WITH(NOLOCK) ON TR.TC_CarTradeCertificationRequestId = TL.TC_CarTradeCertificationRequestId
				LEFT JOIN		TC_CarTradeCertificationData TC WITH(NOLOCK) ON TC.ListingId = TR.ListingId 
				WHERE (DATEDIFF(DAY,TC.InvCertifiedDate,GETDATE()) <= 45) AND AA.IsActive = 1 AND TR.CertificationStatus = 6
				ORDER BY AA.EntryDate DESC
		END
END
----------------------------------------------------------------------------------------------------------------------------
