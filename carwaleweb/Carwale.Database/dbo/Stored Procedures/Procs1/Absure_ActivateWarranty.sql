IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_ActivateWarranty]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_ActivateWarranty]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 7th Apr 2015
-- Description : To fetch and activate Absure_ActivatedWarranty data
-- Modified By : Yuga Hatolkar 13th May, 2015
-- Description : Changed IsActivated for Accepting NULL Values.
-- Modified By : Chetan Navin on Mar 1 2016 (Handled cartrade warranty activation)
-- EXEC Absure_ActivateWarranty NULL,NULL,NULL,NULL
-- =============================================
CREATE PROCEDURE [dbo].[Absure_ActivateWarranty] 
	@CarId       BIGINT = NULL,
	@FromIndex   INT = NULL, 
	@ToIndex     INT = NULL,
	@ActivatedBy INT = NULL
AS
BEGIN
	SET NOCOUNT OFF;

    IF (@CarId IS NULL)
	BEGIN
	    WITH CTE AS
		(
			SELECT  
--distinct 'CWGD15A1000'+cast(AbSure_ActivatedWarrantyId as varchar(1000)) as PolicyNo,
			CustName,
			Mobile,			
			[Customer],			
			[Address],
			[City],
			[Country],
			[Post code],
			[DealerId],
			[Dealer Company Name],
			[CarName],
			--[model],
			--[Year of manufacture],
			--[Version],
			[First Registration date of Vehicle],
			[Vehicle_registration_No],
			[Colour],
			[Current Odometer Reading],
			[Odometer Reading valid till (kms)],
			[Warranty_Type],
			--isnull(PolicyNo,'CWGD15A1000'+cast(AbSure_ActivatedWarrantyId as varchar(1000))) as PolicyNo,
			ISNULL(PolicyNo,'CW'+ AbSure_WarrantyTypes +'DA151000'+CAST(AbSure_ActivatedWarrantyId as varchar(1000))) as PolicyNo,
			PolicyNo as OrigPolicyNo,
			[Extended Warranty Issue Date],
			[Extended Warranty Start date],
			[Extended Warranty End date],
			EngineNo,ChassisNo,
			[AlternatePhone],
			[Email],
			[SurveyDate],
			[State],
			StockId	,
			MakeYear,
			Make,
			Model,
			Version,	
			DealerEmailId,
			DealerMobile,
			IsCarTradeWarranty,	
			CarTradeInvId,		
			ViewReport,ROW_NUMBER() OVER(ORDER BY [Extended Warranty Start date]) AS NumberForPaging,CarId 
			FROM 
			(
				SELECT PolicyNo,AA.AbSure_ActivatedWarrantyId ,AA.CustName + ' - ' + AA.Mobile AS Customer,AA.CustName AS CustName, AA.Mobile AS Mobile, AA.AlternatePhone, AA.Email, AA.Address, C.Name AS City, S.Name AS State, 'India' AS Country,
				A.PinCode AS [Post code], ACD.DealerId, D.Organization AS [Dealer Company Name], (ACD.Make +' '+ ACD.Model+ ' ' + ACD.Version ) AS CarName, AA.MakeYear AS MakeYear,
				AA.RegistrationDate[First Registration date of Vehicle], ACD.Make AS Make, ACD.Model AS Model, ACD.Version AS Version,AA.RegNumber[Vehicle_registration_No], ACD.Colour, 
				AA.Kilometer [Current Odometer Reading], AA.Kilometer+15000 AS [Odometer Reading valid till (kms)], AWT.Warranty [Warranty_Type], 
				--AA.AbSure_ActivatedWarrantyId AS [CW Extended  Warranty Policy no.], 
				ACD.SurveyDate, AA.WarrantyStartDate [Extended Warranty Issue Date], AA.WarrantyStartDate [Extended Warranty Start date], AA.WarrantyEndDate [Extended Warranty End date],
				AA.EngineNo,AA.ChassisNo, CSM.TC_StockId AS StockId,D.EmailId AS DealerEmailId, D.MobileNo AS DealerMobile,
				case AWT.AbSure_WarrantyTypesId when 1 then 'G' when 2 then 'S' end AbSure_WarrantyTypes,
				'http://www.autobiz.in/absure/carcertificate.aspx?carid='+ CAST(ACD.id as varchar(1000)) ViewReport, 
				ROW_NUMBER() over (PARTITION BY AA.CustName order by AA.AbSure_ActivatedWarrantyId desc) as row,ACD.Id AS CarId,AA.WarrantyStartDate,
			    AA.WarrantyEndDate
				,AA.IsCarTradeWarranty
				,-1 AS CarTradeInvId
				FROM AbSure_ActivatedWarranty AA WITH(NOLOCK)
				INNER JOIN AbSure_CarDetails ACD WITH(NOLOCK) ON AA.AbSure_CarDetailsId = ACD.Id
				INNER JOIN AbSure_Trans_Debits ATD WITH(NOLOCK) ON ATD.CarId = ACD.Id
				LEFT JOIN Cities C WITH(NOLOCK) ON ACD.OwnerCityId = C.ID
				LEFT JOIN States AS S WITH(NOLOCK) ON C.StateId = S.ID
				LEFT JOIN Areas A WITH(NOLOCK) ON A.ID = ACD.OwnerAreaId
				LEFT JOIN TC_Users TU WITH(NOLOCK) ON aa.UserId = tu.Id
				LEFT JOIN AbSure_WarrantyTypes AWT WITH(NOLOCK) ON AA.WarrantyTypeId = AWT.AbSure_WarrantyTypesId
				LEFT JOIN dealers D WITH(NOLOCK) ON aa.DealerId=d.ID 
				INNER JOIN AbSure_CarSurveyorMapping CSM WITH(NOLOCK) ON ACD.Id = CSM.AbSure_CarDetailsId
				WHERE 
				D.Id not in (4271,3838) AND 
				ISNULL(AA.IsActivated,0) <> 1 AND ACD.AbSureWarrantyActivationStatusesId = 2 AND ISNULL(AA.IsCarTradeWarranty,0) = 0
				UNION
				SELECT TR.PolicyNo,AA.AbSure_ActivatedWarrantyId ,AA.CustName + ' - ' + AA.Mobile AS Customer,AA.CustName AS CustName, AA.Mobile AS Mobile, AA.AlternatePhone, AA.Email, AA.Address, C.Name AS City, S.Name AS State, 'India' AS Country,
				A.PinCode AS [Post code], AA.DealerId, D.Organization AS [Dealer Company Name], 
				(TR.Make +' '+ TR.Model+ ' ' + TR.Variant ) AS CarName, AA.MakeYear AS MakeYear,
				AA.RegistrationDate[First Registration date of Vehicle], TR.Make AS Make, TR.Model AS Model, TR.Variant AS Version,AA.RegNumber[Vehicle_registration_No],TR.Color, 
				AA.Kilometer [Current Odometer Reading], AA.Kilometer+15000 AS [Odometer Reading valid till (kms)], 'Gold' [Warranty_Type], 
				--AA.AbSure_ActivatedWarrantyId AS [CW Extended  Warranty Policy no.], 
				TC.InvCertifiedDate SurveyDate, AA.WarrantyStartDate [Extended Warranty Issue Date], AA.WarrantyStartDate [Extended Warranty Start date], AA.WarrantyEndDate [Extended Warranty End date],
				AA.EngineNo,AA.ChassisNo, TR.ListingId AS StockId,D.EmailId AS DealerEmailId, D.MobileNo AS DealerMobile,
				'G' AbSure_WarrantyTypes,
				'http://www.autobiz.in/absure/carcertificatenew.aspx?stockid='+ CAST(TR.ListingId as varchar(1000)) ViewReport, 
				ROW_NUMBER() over (PARTITION BY AA.CustName order by AA.AbSure_ActivatedWarrantyId desc) as row,TR.TC_CarTradeCertificationRequestId AS CarId,AA.WarrantyStartDate,
			    AA.WarrantyEndDate
				,AA.IsCarTradeWarranty
				,TC.Id AS CarTradeInvId
				FROM AbSure_ActivatedWarranty AA WITH(NOLOCK)
				LEFT JOIN dealers D WITH(NOLOCK) ON aa.DealerId=d.ID 
				LEFT JOIN Cities C WITH(NOLOCK) ON D.CityId = C.ID
				LEFT JOIN States AS S WITH(NOLOCK) ON C.StateId = S.ID
				LEFT JOIN Areas A WITH(NOLOCK) ON A.ID = D.AreaId				
				LEFT JOIN TC_Users TU WITH(NOLOCK) ON aa.UserId = tu.Id		
				LEFT JOIN TC_CarTradeCertificationRequests TR WITH(NOLOCK) ON TR.TC_CarTradeCertificationRequestId = AA.AbSure_CarDetailsId
				LEFT JOIN TC_CarTradeCertificationData TC WITH(NOLOCK) ON TC.ListingId = TR.ListingId
				WHERE D.Id not in (4271,3838) AND 
				ISNULL(AA.IsActivated,0) <> 1 AND AA.IsCarTradeWarranty = 1
				--and AA.entrydate between @StartDate and @endDate
			) A
			WHERE ROW = 1
		)
		SELECT * INTO #TblTemp
		FROM CTE

		SELECT * FROM #TblTemp
		WHERE  
        (@FromIndex IS NULL AND @ToIndex IS NULL)
        OR
        (NumberForPaging  BETWEEN @FromIndex AND @ToIndex )

		SELECT COUNT(CarId) AS RecordCount
        FROM   #TblTemp
		
		DROP TABLE #TblTemp 
	END

	ELSE
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM AbSure_ActivatedWarranty WITH(NOLOCK) WHERE AbSure_CarDetailsId = @CarId AND IsCarTradeWarranty = 1)
		BEGIN
			UPDATE AbSure_ActivatedWarranty SET IsActivated = 1, ActivationDate = GETDATE(), ActivatedBy = @ActivatedBy
			WHERE AbSure_CarDetailsId = @CarId
			
			IF @@ROWCOUNT > 0
			BEGIN
				EXEC	[AbSure_UpdateStatus] 
						@AbSure_CarDetailsId	= @CarId,
						@Status					= 8,
						@ModifiedBy				= -1
			END
		END
		ELSE
		BEGIN
			UPDATE AbSure_ActivatedWarranty SET IsActivated = 1, ActivationDate = GETDATE(), ActivatedBy = @ActivatedBy
			WHERE AbSure_CarDetailsId = @CarId AND IsCarTradeWarranty = 1
			
			UPDATE TC_CarTradeCertificationRequests SET CertificationStatus = 8
			WHERE TC_CarTradeCertificationRequestId = @CarId
		END
	END
END

----------------------------------------------------------------------------------------------------------
 
/****** Object:  StoredProcedure [dbo].[AbSure_GetCarWarrantyData_v1]    Script Date: 3/31/2016 3:36:56 PM ******/
SET ANSI_NULLS ON
