IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetwarrantyVerificationDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetwarrantyVerificationDetails]
GO

	
-- =============================================
-- Author     : Nilima More.
-- Create Date : 23rd October 2015
-- Description : To get warranty verification Cutomer details.
-- =============================================
CREATE PROCEDURE [dbo].[Absure_GetwarrantyVerificationDetails] @CarId BIGINT = NULL
	,@WarrantyActivationFlag INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF @WarrantyActivationFlag = 1
	BEGIN
		IF NOT EXISTS(SELECT 1 from TC_CarTradeCertificationRequests TR WITH(NOLOCK)
					  JOIN TC_CarTradeCertificationLiveListing TL WITH(NOLOCK) ON TL.ListingId = TR.ListingId
					  WHERE TR.TC_CarTradeCertificationRequestId = @CarId) 
		BEGIN
			SELECT ACD.PolicyNo
				,AA.DuplicateCarId AS Id			
				,AA.CustName + ' - ' + AA.Mobile AS Customer
				,AA.CustName AS CustName
				,AA.Mobile AS Mobile
				,AA.AlternatePhone
				,AA.Email
				,AA.Address
				,C.NAME AS City
				,S.NAME AS STATE
				,'India' AS Country
				,A.PinCode AS [Post code]
				,AA.DealerId
				,D.FirstName + ' ' + D.LastName AS DealerName
				,D.Organization AS [Dealer Company Name]
				,ACD.Make AS Make
				,ACD.Model AS Model
				,ACD.Version AS Version
				,ACD.Make + ' ' + ACD.Model + ' ' + ACD.Version AS VehicalName
				,AA.MakeYear AS MakeYear
				,AA.RegistrationDate [First Registration date of Vehicle]
				,ACD.Make AS Make
				,ACD.Model AS Model
				,ACD.Version AS Version
				,AA.RegNumber [Vehicle_registration_No]
				,ACD.Colour
				,AA.Kilometer [Current Odometer Reading]
				,AA.Kilometer + 15000 AS [Odometer Reading valid till (kms)]
				,AWT.Warranty [Warranty_Type]
				,ACD.VIN AS VIN
				,AA.EntryDate
				,ACD.SurveyDate
				,AA.WarrantyStartDate
				,AA.WarrantyEndDate
				,AA.EngineNo
				,AA.ChassisNo
				,CSM.TC_StockId AS StockId
				,D.EmailId AS DealerEmailId
				,D.MobileNo AS DealerMobile
				,D.Address1 AS DealerAddress
				,AWT.AbSure_WarrantyTypesId AS AbSure_WarrantyTypes
				,'http://www.autobiz.in/absure/carcertificate.aspx?carid=' + CAST(ACD.id AS VARCHAR(1000)) ViewReport
				,ROW_NUMBER() OVER (
					PARTITION BY AA.CustName ORDER BY AA.AbSure_WarrantyActivationPendingId DESC
					) AS ROW
				,ACD.Id AS CarId
				,AA.WarrantyStartDate
				,AA.WarrantyEndDate
			FROM AbSure_WarrantyActivationPending AA WITH (NOLOCK)
			INNER JOIN AbSure_CarDetails ACD WITH (NOLOCK) ON AA.AbSure_CarDetailsId = ACD.Id
			LEFT JOIN Cities C WITH (NOLOCK) ON ACD.OwnerCityId = C.ID
			LEFT JOIN States AS S WITH (NOLOCK) ON C.StateId = S.ID
			LEFT JOIN Areas A WITH (NOLOCK) ON A.ID = ACD.OwnerAreaId
			LEFT JOIN AbSure_WarrantyTypes AWT WITH (NOLOCK) ON AA.WarrantyTypeId = AWT.AbSure_WarrantyTypesId
			LEFT JOIN Dealers D WITH (NOLOCK) ON AA.DealerId = D.ID
			LEFT JOIN AbSure_CarSurveyorMapping CSM WITH (NOLOCK) ON AA.DuplicateCarId = CSM.AbSure_CarDetailsId
			WHERE AA.DuplicateCarId = @CarId AND  AA.IsActive =1
		END
		ELSE
		BEGIN
			SELECT '' PolicyNo
				,AA.DuplicateCarId AS Id			
				,AA.CustName + ' - ' + AA.Mobile AS Customer
				,AA.CustName AS CustName
				,AA.Mobile AS Mobile
				,AA.AlternatePhone
				,AA.Email
				,AA.Address
				,C.NAME AS City
				,S.NAME AS STATE
				,'India' AS Country
				,A.PinCode AS [Post code]
				,AA.DealerId
				,D.FirstName + ' ' + D.LastName AS DealerName
				,D.Organization AS [Dealer Company Name]
				,TR.Make AS Make
				,TR.Model AS Model
				,TR.Variant AS Version
				,TR.Make + ' ' + TR.Model + ' ' + TR.Variant AS VehicalName
				,AA.MakeYear AS MakeYear
				,AA.RegistrationDate [First Registration date of Vehicle]
				,AA.RegNumber [Vehicle_registration_No]
				,TR.Color
				,AA.Kilometer [Current Odometer Reading]
				,AA.Kilometer + 15000 AS [Odometer Reading valid till (kms)]
				,'Gold' [Warranty_Type]
				,AA.ChassisNo AS VIN
				,AA.EntryDate
				,TC.InvCertifiedDate
				,AA.WarrantyStartDate
				,AA.WarrantyEndDate
				,AA.EngineNo
				,AA.ChassisNo
				,TR.ListingId AS StockId
				,D.EmailId AS DealerEmailId
				,D.MobileNo AS DealerMobile
				,D.Address1 AS DealerAddress
				,'1' AS AbSure_WarrantyTypes
				,'http://www.autobiz.in/absure/carcertificatenew.aspx?stockid=' + CAST(TR.ListingId AS VARCHAR(1000)) ViewReport
				,ROW_NUMBER() OVER (
					PARTITION BY AA.CustName ORDER BY AA.AbSure_WarrantyActivationPendingId DESC
					) AS ROW
				,AA.AbSure_CarDetailsId AS CarId
				,AA.WarrantyStartDate
				,AA.WarrantyEndDate
			FROM AbSure_WarrantyActivationPending AA WITH (NOLOCK)			
			LEFT JOIN Dealers D WITH (NOLOCK) ON AA.DealerId = D.ID
			LEFT JOIN Cities C WITH (NOLOCK) ON D.CityId = C.ID
			LEFT JOIN States AS S WITH (NOLOCK) ON C.StateId = S.ID
			LEFT JOIN Areas A WITH (NOLOCK) ON A.ID = D.AreaId
			LEFT JOIN TC_CarTradeCertificationRequests TR WITH(NOLOCK) ON TR.TC_CarTradeCertificationRequestId = AA.AbSure_CarDetailsId
			LEFT JOIN TC_CarTradeCertificationData TC WITH(NOLOCK) ON TC.ListingId = TR.ListingId
			WHERE AA.AbSure_CarDetailsId = @CarId AND  AA.IsActive =1
		END
	END
	ELSE
	BEGIN
		IF NOT EXISTS(SELECT 1 from TC_CarTradeCertificationRequests TR WITH(NOLOCK)
						  JOIN TC_CarTradeCertificationLiveListing TL WITH(NOLOCK) ON TL.ListingId = TR.ListingId
						  WHERE TR.TC_CarTradeCertificationRequestId = @CarId) 
		BEGIN
			SELECT ACD.PolicyNo
				,AA.DuplicateCarId AS Id			
				,AA.CustName + ' - ' + AA.Mobile AS Customer
				,AA.CustName AS CustName
				,AA.Mobile AS Mobile
				,AA.AlternatePhone
				,AA.Email
				,AA.Address
				,C.NAME AS City
				,S.NAME AS STATE
				,'India' AS Country
				,A.PinCode AS [Post code]
				,AA.DealerId
				,D.FirstName + ' ' + D.LastName AS DealerName
				,D.Organization AS [Dealer Company Name]
				,ACD.Make AS Make
				,ACD.Model AS Model
				,ACD.Version AS Version
				,AA.MakeYear AS MakeYear
				,AA.RegistrationDate [First Registration date of Vehicle]
				,ACD.Make + ' ' + ACD.Model + ' ' + ACD.Version AS VehicalName
				,ACD.Make AS Make
				,ACD.Model AS Model
				,ACD.Version AS Version
				,AA.RegNumber [Vehicle_registration_No]
				,ACD.Colour
				,AA.Kilometer [Current Odometer Reading]
				,AA.Kilometer + 15000 AS [Odometer Reading valid till (kms)]
				,AWT.Warranty [Warranty_Type]
				,ACD.VIN AS VIN
				,AA.EntryDate
				,ACD.SurveyDate
				,AA.WarrantyStartDate
				,AA.WarrantyEndDate
				,AA.EngineNo
				,AA.ChassisNo
				,CSM.TC_StockId AS StockId
				,D.EmailId AS DealerEmailId
				,D.MobileNo AS DealerMobile
				,D.Address1 AS DealerAddress
				,AWT.AbSure_WarrantyTypesId AS AbSure_WarrantyTypes
				,'http://www.autobiz.in/absure/carcertificate.aspx?carid=' + CAST(ACD.id AS VARCHAR(1000)) ViewReport
				,ROW_NUMBER() OVER (
					PARTITION BY AA.CustName ORDER BY AA.AbSure_WarrantyActivationPendingId DESC
					) AS ROW
				,ACD.Id AS CarId
				,AA.WarrantyStartDate
				,AA.WarrantyEndDate
			FROM AbSure_WarrantyActivationPending AA WITH (NOLOCK)
			INNER JOIN AbSure_CarDetails ACD WITH (NOLOCK) ON AA.AbSure_CarDetailsId = ACD.Id
			LEFT JOIN Cities C WITH (NOLOCK) ON ACD.OwnerCityId = C.ID
			LEFT JOIN States AS S WITH (NOLOCK) ON C.StateId = S.ID
			LEFT JOIN Areas A WITH (NOLOCK) ON A.ID = ACD.OwnerAreaId
			LEFT JOIN AbSure_WarrantyTypes AWT WITH (NOLOCK) ON AA.WarrantyTypeId = AWT.AbSure_WarrantyTypesId
			LEFT JOIN Dealers D WITH (NOLOCK) ON AA.DealerId = D.ID
			LEFT JOIN AbSure_CarSurveyorMapping CSM WITH (NOLOCK) ON AA.DuplicateCarId = CSM.AbSure_CarDetailsId
			WHERE AA.DuplicateCarId = @CarId
				AND AA.IsActive = 1
		END
		ELSE
		BEGIN
			SELECT '' PolicyNo
					,AA.DuplicateCarId AS Id			
					,AA.CustName + ' - ' + AA.Mobile AS Customer
					,AA.CustName AS CustName
					,AA.Mobile AS Mobile
					,AA.AlternatePhone
					,AA.Email
					,AA.Address
					,C.NAME AS City
					,S.NAME AS STATE
					,'India' AS Country
					,A.PinCode AS [Post code]
					,AA.DealerId
					,D.FirstName + ' ' + D.LastName AS DealerName
					,D.Organization AS [Dealer Company Name]
					,TR.Make AS Make
					,TR.Model AS Model
					,TR.Variant AS Version
					,TR.Make + ' ' + TR.Model + ' ' + TR.Variant AS VehicalName
					,AA.MakeYear AS MakeYear
					,AA.RegistrationDate [First Registration date of Vehicle]
					,AA.RegNumber [Vehicle_registration_No]
					,TR.Color
					,AA.Kilometer [Current Odometer Reading]
					,AA.Kilometer + 15000 AS [Odometer Reading valid till (kms)]
					,'Gold' [Warranty_Type]
					,AA.ChassisNo AS VIN
					,AA.EntryDate
					,TC.InvCertifiedDate
					,AA.WarrantyStartDate
					,AA.WarrantyEndDate
					,AA.EngineNo
					,AA.ChassisNo
					,TR.ListingId AS StockId
					,D.EmailId AS DealerEmailId
					,D.MobileNo AS DealerMobile
					,D.Address1 AS DealerAddress
					,'1' AS AbSure_WarrantyTypes
					,'http://www.autobiz.in/absure/carcertificatenew.aspx?stockid=' + CAST(TR.ListingId AS VARCHAR(1000)) ViewReport
					,ROW_NUMBER() OVER (
						PARTITION BY AA.CustName ORDER BY AA.AbSure_WarrantyActivationPendingId DESC
						) AS ROW
					,AA.AbSure_CarDetailsId AS CarId
					,AA.WarrantyStartDate
					,AA.WarrantyEndDate
				FROM AbSure_WarrantyActivationPending AA WITH (NOLOCK)			
				LEFT JOIN Dealers D WITH (NOLOCK) ON AA.DealerId = D.ID
				LEFT JOIN Cities C WITH (NOLOCK) ON D.CityId = C.ID
				LEFT JOIN States AS S WITH (NOLOCK) ON C.StateId = S.ID
				LEFT JOIN Areas A WITH (NOLOCK) ON A.ID = D.AreaId
				LEFT JOIN TC_CarTradeCertificationRequests TR WITH(NOLOCK) ON TR.TC_CarTradeCertificationRequestId = AA.AbSure_CarDetailsId
				LEFT JOIN TC_CarTradeCertificationData TC WITH(NOLOCK) ON TC.ListingId = TR.ListingId
				WHERE AA.AbSure_CarDetailsId = @CarId AND  AA.IsActive =1
		END
	END
END

---------------------------------------------------------------------------------------------------------

