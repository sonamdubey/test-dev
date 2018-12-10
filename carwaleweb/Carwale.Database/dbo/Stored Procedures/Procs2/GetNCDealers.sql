IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNCDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNCDealers]
GO

	
-- =============================================
-- Author:		Vicky Lund
-- Create date: 07/06/2016
-- EXEC [GetNCDealers] 18,'1,2,3,4,5,6',-1
-- =============================================
CREATE PROCEDURE [dbo].[GetNCDealers] @MakeIds varchar(150)
	,@StateIds varchar(100)
	,@CityIds varchar(3000)
AS
BEGIN
	SELECT D.ID Id
		,D.Organization [Name]
	FROM Dealers D WITH (NOLOCK)
	INNER JOIN TC_DealerMakes TDM WITH (NOLOCK) ON D.ID = TDM.DealerId
		AND D.TC_DealerTypeId IN (2, 3)
		AND [Status] = 0
	WHERE (
			@StateIds = '-1'
			OR D.StateId IN (SELECT ListMember FROM fnSplitCSVMAx(@StateIds))
			)
		AND (
			@CityIds = '-1'
			OR D.CityId IN (SELECT ListMember FROM fnSplitCSVMAx(@CityIds))
			)
		AND (
			@MakeIds = '-1'
			OR TDM.MakeId IN (SELECT ListMember FROM fnSplitCSVMAx(@MakeIds))
			)
END


