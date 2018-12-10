IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDataForVWNSCExcel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDataForVWNSCExcel]
GO
	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 10 Feb 2014 at 7 pm
-- Description:	To get all related data while import NSC excel for VW
-- [TC_GetDataForVWNSCExcel] 20 , 3
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetDataForVWNSCExcel]
	@MakeId INT,
	@InquiryType SMALLINT	
AS
BEGIN

	--Get make, models, version
	EXEC  TC_GetMakeModelVersion NULL,1,@MakeId

	--For cities
	SELECT	DISTINCT TD.CityId AS Id, C.Name AS Name, TD.DealerId
	FROM	TC_DealerCities AS TD
			INNER JOIN Cities AS C ON TD.CityID = C.ID
			INNER JOIN Dealers AS D ON D.ID=TD.DealerId
	WHERE   TD.IsActive = 1 AND C.IsDeleted=0 AND D.TC_BrandZoneId IS NOT NULL
	ORDER BY C.Name
	
	--To get inquiry source.
	SELECT	Id, Source,OrderBY
	FROM	TC_InquirySource
	WHERE	(MakeId IS NULL OR MakeId = @MakeId)
			AND	IsActive=1 AND IsVisible=1
	ORDER by OrderBY

	--Get Dealer code with ids
	EXEC  TC_CheckDealerBelongtoMake NULL, @MakeId
	
END

