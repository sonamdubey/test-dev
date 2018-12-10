IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMGetScenarioDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMGetScenarioDetails]
GO

	-- =============================================
-- Author:		Vishal Srivastava
-- Create date: 16 December 2013 1726 HRS IST
-- Description:	To fetch granular level target.
-- =============================================
CREATE PROCEDURE [dbo].[TC_TMGetScenarioDetails] 
	-- Add the parameters for the stored procedure here
	@TC_TMDistributionPatternMasterId INT,
	@Year INT =NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF(@TC_TMDistributionPatternMasterId=-1)
		BEGIN
			SELECT 
				DD.Organization,
				CC.Name,
				DATENAME(MONTH, DATEADD(MONTH, TCTMTSD.[Month], -1 )) AS [Month],
				TCTMTSD.[Year],
				TCTMTSD.[Target] 
			FROM 
					TC_DealersTarget AS TCTMTSD WITH(NOLOCK)
						INNER JOIN  Dealers AS DD WITH(NOLOCK)
																	ON DD.ID=TCTMTSD.DealerId
						INNER JOIN  CarVersions AS CC WITH(NOLOCK) 
						                                             ON CC.ID=TCTMTSD.CarVersionId
			WHERE TCTMTSD.[Year]=@Year
		END
	ELSE
		BEGIN
			SELECT 
				DD.Organization,
				CC.Name,
				DATENAME(MONTH, DATEADD(MONTH, TCTMTSD.[Month], -1 )) AS [Month],
				TCTMTSD.[Year],
				TCTMTSD.[Target] 
			FROM 
			      TC_TMTargetScenarioDetail AS TCTMTSD WITH(NOLOCK)
						INNER JOIN  Dealers AS DD WITH(NOLOCK) 
						                                           ON DD.ID=TCTMTSD.DealerId
						INNER JOIN  CarVersions AS CC WITH(NOLOCK)
						                                           ON CC.ID=TCTMTSD.CarVersionId
			WHERE 
					TCTMTSD.TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
		END
END
