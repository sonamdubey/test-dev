IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetWarrantyInspectionSummary]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetWarrantyInspectionSummary]
GO

	


-- =============================================
-- Author		: Suresh Prajapati
-- Create date	: 25th May 2015
-- Description	: To get the dealer report for a surveyor
-- Modifier		: Sachin Bharti(22nd July 2015)
-- Purpose		: Added constraint isActive 1 to get only active users	
-- exec M_GetWarrantyInspectionSummary 3
-- =============================================
CREATE PROCEDURE [dbo].[M_GetWarrantyInspectionSummary] 
	@UserId INT
AS
BEGIN
	DECLARE @DirectReportingUserId AS TABLE ( UserId INT,UserName VARCHAR(500))

	INSERT INTO 
		@DirectReportingUserId
	SELECT MU.OprUserId	,OU.UserName
	FROM DCRM_ADM_MappedUsers MU(NOLOCK)
	INNER JOIN OprUsers AS OU WITH (NOLOCK) ON OU.Id = MU.OprUserId
	WHERE 
		MU.NodeRec.GetAncestor(1) = (SELECT NodeRec	FROM DCRM_ADM_MappedUsers WITH (NOLOCK)	WHERE OprUserId = @UserId AND IsActive = 1)
		AND MU.IsActive = 1--select reporting users

	--now INSERT reporting user also IN the TEMP TABLE
	INSERT INTO @DirectReportingUserId 
				(UserId ,UserName)
				SELECT @UserId,UserName
				FROM	
					OprUsers
				WHERE 
					Id = @UserId

	SELECT * FROM @DirectReportingUserId WHERE UserId <> @UserId --ORDER BY UserName DESC

	DECLARE @TempUserId INT = 0

	-- Iterate over all users
	WHILE (1 = 1)
		BEGIN
			-- Get next UserId
			SELECT TOP 1 @TempUserId = UserId
			FROM @DirectReportingUserId
			WHERE UserId > @TempUserId
			ORDER BY UserId

			-- Exit loop if no more Users
			IF @@ROWCOUNT = 0
				BREAK;

			-- call your sproc
			EXEC M_GetWarrantyInspectionStatus @TempUserId,NULL
		END

	EXEC AbSure_DealerReportforSurveyor @UserId,1,NULL,NULL,NUll,NULL,NULL,NULL
END
