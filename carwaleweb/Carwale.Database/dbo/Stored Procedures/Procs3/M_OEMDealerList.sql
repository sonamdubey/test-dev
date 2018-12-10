IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_OEMDealerList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_OEMDealerList]
GO

	-- =============================================
-- Author	:	Sachin Bharti
-- Create date	:	13Th Dec 2013
-- Description	:	Get OEM Dealers for OPR mobile website
-- =============================================
CREATE PROCEDURE [dbo].[M_OEMDealerList] 
	@CityName	VARCHAR(100) = NULL,
	@AreaId		INT		=	NULL,
	@UserId		NUMERIC(18,0)	= NULL,
	@Organization	VARCHAR(200)	=	NULL,
	@PageIndex	SMALLINT = NULL,
    @PageSize	SMALLINT = NULL
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @FirstRow SMALLINT
	DECLARE @LastRow SMALLINT
	
	SET @FirstRow = @PageIndex * @PageSize 
	SET @LastRow = @FirstRow + @PageSize + 1

	
	SELECT  Name AS Dealer, ID AS DealerId ,Mobile AS MobileNo, 
			Address,Email AS EmailId,Area,CityName,Address AS DealerAddress,CallId,
			4 As DealerType,DealerStatus,IsDealerActive
		FROM	
		(	SELECT  ND.ID, ND.Name,replace(ND.EMail, ',', ' ') as email,ND.Mobile,
			A.Name AS Area,C.Name AS CityName,ND.Address,ISNULL(ND.ID,0) AS CallId,
			CASE WHEN ISNULL(ND.IsActive,0) = 1 THEN 'active' WHEN  ISNULL(ND.IsActive,0) = 0 THEN 'inActive' END AS DealerStatus,
			CASE WHEN ISNULL(ND.IsActive,0) = 0 THEN 0 WHEN  ISNULL(ND.IsActive,0) = 1 THEN 1 END AS IsDealerActive,
			ROW_NUMBER() OVER(ORDER BY ND.ID ASC) AS DealerRank
			FROM NCS_Dealers AS ND WITH(NOLOCK) 
			LEFT JOIN Cities C (NOLOCK) ON C.ID = ND.CityId	
			LEFT JOIN Areas  A (NOLOCK) ON A.ID = ND.AreaId
			LEFT JOIN DCRM_Calls DC(NOLOCK) ON DC.DealerId = ND.ID AND DC.UserId = @UserId AND DC.ActionTakenId = 2
			WHERE (@CityName IS NULL OR C.Name = @CityName)
			AND (@AreaId IS NULL OR  ND.AreaId = @AreaId )
			AND (@Organization IS NULL OR ND.Name LIKE @Organization)
		) 
		AS DealerRowNumber
		WHERE DealerRank > @FirstRow AND DealerRank < @LastRow
	ORDER BY Dealer

	SELECT COUNT(*) AS TotalDealer
		FROM  NCS_Dealers AS ND WITH(NOLOCK) 
		LEFT JOIN Cities C (NOLOCK) ON C.ID = ND.CityId	
		LEFT JOIN Areas  A (NOLOCK) ON A.ID = ND.AreaId
		LEFT JOIN DCRM_Calls DC(NOLOCK) ON DC.DealerId = ND.ID AND DC.UserId = @UserId AND DC.ActionTakenId = 2
		WHERE (@CityName IS NULL OR C.Name = @CityName)
		AND (@AreaId IS NULL OR  ND.AreaId = @AreaId )
		AND (@Organization IS NULL OR ND.Name LIKE @Organization)

END



/****** Object:  StoredProcedure [dbo].[M_GetUserDealers]    Script Date: 12/22/2014 4:46:42 PM ******/
SET ANSI_NULLS ON
