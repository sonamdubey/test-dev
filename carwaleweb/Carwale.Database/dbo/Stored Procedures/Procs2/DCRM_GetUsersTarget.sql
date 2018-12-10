IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetUsersTarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetUsersTarget]
GO

	

-- =============================================
-- Author	:	Sachin Bharti(18th May 2015)
-- Description	:	Used to get all users and thier targets
-- Execute DCRM_GetUsersTarget '4,5,6',1,37,'2015'
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetUsersTarget]
	@MonthIds VARCHAR(10) = NULL,
	@BusinessUnitId	SMALLINT = NULL,
	@MetricId	SMALLINT = NULL,
	@TargetYear VARCHAR(5) = NULL
AS
BEGIN

	SELECT 
		OU.Id AS OprUserId,
		OU.UserName,
		MU.UserLevel,
		ISNULL(ET.TargetMonth,0) AS TargetMonth,
		ISNULL(ET.UserTarget,0) AS UserTarget
	FROM	
		DCRM_ADM_MappedUsers MU(NOLOCK)
		INNER JOIN OprUsers OU(NOLOCK) ON MU.OprUserId = OU.Id AND MU.IsActive = 1
		LEFT JOIN DCRM_FieldExecutivesTarget ET(NOLOCK) ON MU.OprUserId = ET.OprUserId 
				AND ET.TargetMonth IN (SELECT *FROM fnSplitCSV(@MonthIds))
				AND ET.TargetYear = @TargetYear
				AND ET.MetricId = @MetricId
				AND ET.BusinessUnitId = @BusinessUnitId
	WHERE
		MU.BusinessUnitId = @BusinessUnitId
	ORDER BY
		MU.NodeCode  
END

