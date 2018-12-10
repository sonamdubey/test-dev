IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_DealerList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_DealerList]
GO

	



-- =============================================
-- Author		:	Sachin Bharti
-- Create date	:	12th Dec 2013
-- Description	:	Show all dealers for mobile website
-- Modifier	:	Sachin Bharti(3rd July 2015)
-- Purpose	:	Remove DCRM_Calls table from join and make Cities table
--				inner join with Dealers table
-- =============================================
CREATE PROCEDURE [dbo].[M_DealerList]
	@DealerType	SMALLINT = NULL,
	@CityName	VARCHAR(100) = NULL,
	@AreaId		INT = NULL,
	@UserId		NUMERIC(18,0) = NULL,
	@Organization	VARCHAR(200) = NULL,
	@PageIndex	SMALLINT = NULL,
    @PageSize	SMALLINT = NULL
AS
BEGIN

	
		SET NOCOUNT ON;
	
		DECLARE @FirstRow SMALLINT
		DECLARE @LastRow SMALLINT
	
		SET @FirstRow = @PageIndex * @PageSize 
		SET @LastRow = @FirstRow + @PageSize + 1

		SELECT  Dealer,DealerId ,
				MobileNo, EmailId,Area,CityName,Address1 AS DealerAddress,
				TC_DealerTypeId As DealerType,DealerStatus,IsDealerActive
			FROM	(
						SELECT DISTINCT D.ID AS DealerId,D.Organization AS Dealer,  
						D.MobileNo, replace(D.EmailId, ',', ' ') AS EmailId,A.Name AS Area,C.Name AS CityName,
						D.Address1,DC.Id AS CallId,ROW_NUMBER() OVER(ORDER BY D.ID ASC) AS DealerRank,D.TC_DealerTypeId,
						CASE WHEN ISNULL(D.Status,1) = 0 THEN 'active' WHEN  ISNULL(D.Status,1) = 1 THEN 'inActive' END AS DealerStatus,
						CASE WHEN ISNULL(D.Status,1) = 0 THEN 1 WHEN  ISNULL(D.Status,1) = 1 THEN 0 END AS IsDealerActive
						FROM Dealers (NOLOCK) AS D 
						INNER JOIN TC_DealerType(NOLOCK) TC  ON TC.TC_DealerTypeId = D.TC_DealerTypeId 
						INNER JOIN Cities C(NOLOCK) ON C.ID = D.CityId 
						LEFT JOIN Areas A(NOLOCK) ON A.ID = D.AreaId
						LEFT JOIN DCRM_Calls DC(NOLOCK) ON DC.DealerId = D.ID AND DC.ActionTakenId = 2 AND DC.UserId = @UserId
						WHERE 
						(@DealerType IS NULL OR TC.TC_DealerTypeId = @DealerType)
						AND (@Organization IS NULL OR D.Organization LIKE @Organization)
						AND (@CityName IS NULL OR C.Name = @CityName)
						AND (@AreaId IS NULL OR D.AreaId = @AreaId)
						AND (D.IsDealerDeleted=0)
						
					) AS DealerRowNumber
		WHERE DealerRank > @FirstRow AND
			DealerRank < @LastRow
		ORDER BY IsDealerActive DESC,Dealer
	
		SELECT COUNT(*) AS TotalDealer
			FROM Dealers (NOLOCK) AS D 
			INNER JOIN TC_DealerType(NOLOCK) TC  ON TC.TC_DealerTypeId = D.TC_DealerTypeId 
			INNER JOIN Cities C(NOLOCK) ON C.ID = D.CityId 
			WHERE 
			(@DealerType IS NULL OR TC.TC_DealerTypeId = @DealerType)
			AND (@Organization IS NULL OR  D.Organization LIKE @Organization)
			AND (@CityName IS NULL OR C.Name = @CityName)
			AND (@AreaId IS NULL OR D.AreaId = @AreaId)
			AND (D.IsDealerDeleted=0)
	
END
