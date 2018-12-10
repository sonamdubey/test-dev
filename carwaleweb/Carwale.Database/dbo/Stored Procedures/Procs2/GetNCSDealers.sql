IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNCSDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNCSDealers]
GO

	
-- =============================================
-- Author:		ashish verma
-- Create date: 16/9/2014
-- Description:	Returns all the New Car Dealers for a city for a particular make
-- Modified By - Deepak 14th Sep 2013, Added AND NCSD.IsNCDDealer = 0 AND NCSD.Name NOT LIKE '%H-5000%'
-- Modified By - Deepak 30th Jan 2014
-- Modified By - Raghu 30th jan 2014, Added MakeId <> 11 and NCSD.Name NOT LIKE '%H5000%'
-- Modified By - Vinayak 7th oct 2014, Added AreaName in select statement'
-- Modified By : Shalini Nair, 23/08/2016 to pass ModelId as input parameter and use TC_NoDealerModels table
-- =============================================
CREATE PROCEDURE [dbo].[GetNCSDealers] @ModelId INT
	,@CityId INT
AS
BEGIN
	DECLARE @MakeId INT;

	SELECT @MakeId = CarMakeId
	FROM CarModels WITH (NOLOCK)
	WHERE id = @ModelId

	SELECT DISTINCT NCSD.ID AS DealerId
		,ISNULL(NCSD.AreaName + ' - ', '') + NCSD.DealerTitle AS DealerName
		,NCSD.Address AS DealerArea -- Modified By - Vinayak 7th oct 2014, Added AreaName in select statement'
	FROM NCS_Dealers AS NCSD WITH (NOLOCK)
	LEFT JOIN NCS_DealerMakes DM WITH (NOLOCK) ON NCSD.Id = DM.DealerId
		AND NCSD.IsActive = 1
	WHERE NCSD.CityId = @CityId
		AND DM.MakeId = @MakeId --AND DM.MakeId <> 11
		AND NCSD.DealerType = 0
		AND NCSD.NAME NOT LIKE '%H-5000%'
		AND NCSD.NAME NOT LIKE '%H5000%'
		----Added By Deepak on 30th Jan 2014 to stop showing blocked dealers.
		--AND NCSD.ID NOT IN (
		--	SELECT DealerID
		--	FROM CRM.FLCDealerPriority WITH (NOLOCK)
		--	WHERE (
		--			(
		--				ModelId IN (
		--					SELECT Id
		--					FROM CarModels WITH (NOLOCK)
		--					WHERE CarMakeId = @MakeId
		--					)
		--				AND Priority = 2
		--				)
		--			OR (
		--				ModelId = - 1
		--				AND Priority = 2
		--				)
		--			)
		--	)
		AND NCSD.ID NOT IN (
			SELECT DealerID
			FROM TC_NoDealerModels TCM WITH (NOLOCK)
			WHERE ModelId = @ModelId
				AND TCM.Source = 2 -- NCS dealer source
			)
	ORDER BY DealerName
END
