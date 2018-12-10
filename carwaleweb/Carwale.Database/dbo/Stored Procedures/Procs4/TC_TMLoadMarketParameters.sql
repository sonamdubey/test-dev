IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMLoadMarketParameters]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMLoadMarketParameters]
GO

	-- =============================================
-- Author:		Vishal Srivastava
-- Create date: 26-10-2013
-- Description:	Loads Market Parameters
--			  : Load the Saved Market Parameter
-- =============================================
CREATE PROCEDURE [dbo].[TC_TMLoadMarketParameters]
@TC_TMDistributionPatternMasterId INT =NULL
	
AS
BEGIN
	SET NOCOUNT ON;	
		SELECT ZoneName AS Region,TC_BrandZoneId AS ID FROM TC_BrandZone WITH(NOLOCK) WHERE IsActive=1 AND MakeId=20

		SELECT
			[Date],
			TCSU.UserName AS SpecialUser,
			Percentage,
			TCBZ.ZoneName AS BrandZone,
			TCSUS.UserName AS AMName,
			CM.Name AS CarModelName,
			M.MonthSmall AS StartMonth,
			MON.MonthSmall AS EndMonth,
			D.Organization AS Dealership
		FROM TC_TMMarketParameter AS TCTMMP WITH(NOLOCK)
				INNER JOIN TC_SpecialUsers AS TCSU WITH(NOLOCK) ON TCTMMP.TC_SpecialUsersId=TCSU.TC_SpecialUsersId
				LEFT JOIN TC_BrandZone AS TCBZ WITH(NOLOCK) ON TCTMMP.TC_BrandZoneId=TCBZ.TC_BrandZoneId
				LEFT JOIN TC_SpecialUsers AS TCSUS WITH(NOLOCK) ON TCTMMP.AMId=TCSUS.TC_SpecialUsersId
				LEFT JOIN CarModels AS CM WITH(NOLOCK) ON TCTMMP.CarModelId=CM.ID
				LEFT JOIN Dealers AS D WITH(NOLOCK) ON TCTMMP.DealerId=D.ID
				INNER JOIN Months AS M WITH(NOLOCK) ON TCTMMP.StartMonth= M.MonthsId
				INNER JOIN Months AS MON WITH(NOLOCK) ON TCTMMP.EndMonth=MON.MonthsId 
				WHERE( TCTMMP.TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId 
				            OR @TC_TMDistributionPatternMasterId IS NULL)
		ORDER BY TCTMMP.TC_TMMargetParameterId DESC
END
