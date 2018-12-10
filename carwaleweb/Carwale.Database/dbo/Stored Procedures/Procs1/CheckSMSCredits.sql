IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckSMSCredits]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckSMSCredits]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 3-2-2015
-- Description:	SMS Credits Expired Alert 
-- =============================================
CREATE PROCEDURE [dbo].[CheckSMSCredits]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	CREATE TABLE #tempSMSCredits(
		alertcount BIGINT	
	)
	
    INSERT INTO #tempSMSCredits(alertcount)
    SELECT count(id)
	FROM smssent WITH(NOLOCK)
	WHERE SMSSentDateTime>dateadd(minute, -15, getdate()-2)
	and ReturnedMsg ='Credits Expired.'
	
	SELECT alertcount
	FROM #tempSMSCredits
	WHERE alertcount>0
	
	DROP table #tempSMSCredits
END
