IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMGetAreaDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMGetAreaDealers]
GO

	-- Author		:	Nilesh Utture.
-- Create date	:	18th Nov, 2013.
-- Description	:	This SP used to get all area dealers as well as for dealers whose target is set.
-- Modified By  :	Added parameter @MonthId
-- EXEC TC_GetAreaDealers 35,2,1
-- =============================================    
CREATE PROCEDURE [dbo].[TC_TMGetAreaDealers] 
 -- Add the parameters for the stored procedure here    
 @UserId BIGINT,
 @DealersFlag SMALLINT = NULL,
 @DistributionMasterId SMALLINT = NULL,
 @MonthId SMALLINT = NULL
AS    
BEGIN    
	
	DECLARE @Year SMALLINT 
	SELECT @Year = YEAR(GETDATE())

	IF(@DealersFlag IS  NULL) -- Get all active dealers
	BEGIN
		SELECT	DISTINCT D.ID AS Value , D.Organization AS Text
		FROM	Dealers D  WITH (NOLOCK) 
		JOIN TC_BrandZone AS TCB  WITH (NOLOCK)  ON TCB.TC_BrandZoneId=D.TC_BrandZoneId AND TCB.MakeId=20
		WHERE	TC_amId = @UserID
		AND IsDealerActive=1 
		ORDER BY ID DESC
	END

	IF(@DealersFlag = 1)-- Get all the dealers whose target is set
		BEGIN
			--Changed By Deepak on 4th Dec 2013
			IF @DistributionMasterId = -1
				BEGIN
					SELECT	DISTINCT D.ID AS Value , D.Organization AS Text,
					SUM(DT.Target) AS Target
					FROM	Dealers D  WITH (NOLOCK) 
					JOIN TC_BrandZone AS TCB WITH (NOLOCK)   ON TCB.TC_BrandZoneId=D.TC_BrandZoneId AND TCB.MakeId=20
					JOIN TC_TMIntermediateLegacyDetail DT  WITH (NOLOCK)  ON DT.DealerId = D.ID AND DT.Month >= @MonthId
					WHERE D.TC_AMId = @UserId
					AND D.IsDealerActive=1
					AND DT.DealerId IS NOT NULL
					GROUP BY D.ID, D.Organization
					ORDER BY ID DESC
				END
			ELSE
				
				BEGIN
					SELECT	DISTINCT D.ID AS Value , D.Organization AS Text,
					SUM(DT.Target) AS Target
					FROM	Dealers D  WITH (NOLOCK) 
					JOIN TC_BrandZone AS TCB WITH (NOLOCK)   ON TCB.TC_BrandZoneId=D.TC_BrandZoneId AND TCB.MakeId=20
					JOIN TC_TMTargetScenarioDetail DT  WITH (NOLOCK)  ON DT.DealerId = D.ID AND DT.TC_TMDistributionPatternMasterId = @DistributionMasterId AND DT.Month >= @MonthId
					WHERE D.TC_AMId = @UserId
					AND D.IsDealerActive=1
					AND DT.DealerId IS NOT NULL
					GROUP BY D.ID, D.Organization
					ORDER BY ID DESC
				END
		END
		
END
