IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_GetDefinedDealerRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_GetDefinedDealerRules]
GO

	-- =============================================
-- Author	:	Sachin Bharti(15th July 2014)
-- Description	:	Get all the defined dealer rules for NCD_Dealers
-- =============================================
CREATE PROCEDURE [dbo].[NCD_GetDefinedDealerRules]
	@DealerID	INT  = NULL
AS
BEGIN
	
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		NCD.Id AS DealerRulesID,
		DN.Name AS Organization , 
		NCD.DealerID,
		CASE WHEN NCD.MakeId IS NULL THEN '' WHEN NCD.MakeId = '-1' THEN 'All' ELSE CK.Name  END AS Make,
		CASE WHEN NCD.ModelId IS NULL THEN '' WHEN NCD.ModelId = '-1' THEN 'All' ELSE CM.Name  END AS Model,
		CASE WHEN NCD.VersionId IS NULL THEN '' WHEN NCD.VersionId = '-1' THEN 'All' ELSE CV.Name  END AS Version,
		CASE WHEN NCD.StateId IS NULL THEN '' WHEN NCD.StateId = '-1' THEN 'All' ELSE S.Name  END AS State,
		CASE WHEN NCD.CityId IS NULL THEN '' WHEN NCD.CityId = '-1' THEN 'All' ELSE C.Name  END AS City,
		CASE WHEN NCD.ZoneId IS NULL THEN '' WHEN NCD.ZoneId = '-1' THEN 'All' ELSE CZ.ZoneName  END AS Zone,
		OU.UserName AS CreatedBy,
		CONVERT(VARCHAR,NCD.CreatedOn,0) AS CreatedOn
	FROM NCD_DefinedDealerRules NCD(NOLOCK)
	INNER JOIN Dealer_NewCar DN(NOLOCK) ON DN.Id = NCD.DealerId
	LEFT JOIN CarMakes CK(NOLOCK) ON CK.ID = NCD.MakeId
	LEFT JOIN CarModels CM(NOLOCK) ON CM.ID = NCD.ModelId
	LEFT JOIN CarVersions CV(NOLOCK) ON CV.ID = NCD.VersionId
	LEFT JOIN States S(NOLOCK) ON S.ID = NCD.StateId
	LEFT JOIN Cities C(NOLOCK) ON C.ID = NCD.CityId
	LEFT JOIN CityZones CZ(NOLOCK) ON CZ.Id = NCD.ZoneId
	LEFT JOIN OprUsers OU(NOLOCK) ON OU.Id = NCD.CreatedBy
	WHERE 
		(@DealerID IS NULL OR NCD.DealerID = @DealerID)
		AND NCD.IsActive = 1
	ORDER BY NCD.CreatedOn DESC
    
END
