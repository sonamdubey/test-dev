IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_WarrantyActivationReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_WarrantyActivationReport]
GO

	-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 21st July 2015
-- Description : To fetch  Absure_WarrantyActivationReport data
-- Modified By : Added alias Guarantee for warranty
-- =============================================
CREATE PROCEDURE [dbo].[Absure_WarrantyActivationReport]
	@Status      BIT,
	@StartDate   DATETIME, 
	@EndDate     DATETIME
AS
BEGIN

 SELECT
		AA.CustName AS Name, AA.Mobile AS Mobile, AA.Email, AA.Address,ACD.Make AS Make, ACD.Model AS Model, ACD.Version AS Version,AA.RegNumber [RegistrationNo],
		AA.EngineNo,AA.ChassisNo,AWT.Warranty AS Guarantee,AA.Kilometer,AA.WarrantyStartDate AS [Start Date],AA.WarrantyEndDate AS [End Date],PolicyNo,CSM.TC_StockId AS StockId
		,C.Name AS City,S.Name AS State, 'India' AS Country ,A.PinCode AS [Post code] , D.ID AS [Dealer Id] , D.Organization AS [Dealer Company Name] , AA.MakeYear AS [Year of manufacture],AA.RegistrationDate[First Registration date of Vehicle] 
		,ACD.Colour AS Color, AA.Kilometer [Current Odometer Reading] ,AA.Kilometer+15000 AS [Odometer Reading valid till (kms)], AA.WarrantyStartDate [Extended Guarantee Issue Date],AA.AlternatePhone [Alternate Phone],ACD.SurveyDate [Survey Date],
		'http://www.autobiz.in/absure/carcertificate.aspx?carid='+CAST(ACD.id as varchar(1000)) [View Report] ,TDT.DealerType [Dealer Type]
		FROM AbSure_ActivatedWarranty AA WITH(NOLOCK)
			INNER JOIN AbSure_CarDetails ACD WITH(NOLOCK) ON AA.AbSure_CarDetailsId = ACD.Id
			INNER JOIN AbSure_Trans_Debits ATD WITH(NOLOCK) ON ATD.CarId = ACD.Id
			LEFT JOIN Cities C WITH(NOLOCK) ON ACD.OwnerCityId = C.ID
			LEFT JOIN States AS S WITH(NOLOCK) ON C.StateId = S.ID
			LEFT JOIN Areas A WITH(NOLOCK) ON A.ID = ACD.OwnerAreaId
			--LEFT JOIN TC_Users TU ON aa.UserId = tu.Id
			LEFT JOIN AbSure_WarrantyTypes AWT WITH(NOLOCK) ON AA.WarrantyTypeId = AWT.AbSure_WarrantyTypesId
			LEFT JOIN dealers D WITH(NOLOCK) ON aa.DealerId=d.ID 
			LEFT JOIN TC_DealerType AS TDT WITH(NOLOCK) ON TDT.TC_DealerTypeId= D.TC_DealerTypeId
			INNER JOIN AbSure_CarSurveyorMapping CSM WITH(NOLOCK) ON ACD.Id = CSM.AbSure_CarDetailsId
		WHERE 
		--D.Id not in (4271,3838) AND 
		ISNULL(AA.IsActivated,0) = @Status
	   -- AND AA.WarrantyStartDate >= @StartDate  AND AA.WarrantyEndDate<=@EndDate
		AND AA.WarrantyStartDate BETWEEN @StartDate AND @EndDate
		ORDER BY AA.WarrantyStartDate DESC
END


