IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMReleaseLock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMReleaseLock]
GO

	-- =============================================
-- Author:		Vishal Srivastava AE 1830
-- Create date: 28 November 2013 1341 HRS IST
-- Description:	Release Lock forcefully on click of Release Button

-- =============================================
CREATE PROCEDURE [dbo].[TC_TMReleaseLock] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	TRUNCATE TABLE TC_TMCheckUserLogin
    TRUNCATE TABLE TC_TMIntermediateLegacyDetail

	DELETE FROM TC_TMTargetCopyData WHERE TC_TMDistributionPatternMasterId = -1

END
