IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LTS_DAILYSYNC]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LTS_DAILYSYNC]
GO

	CREATE PROCEDURE [dbo].[LTS_DAILYSYNC]
AS
BEGIN
	
	EXECUTE [dbo].[LTS_CTSummary] 
	EXECUTE [dbo].[LTS_STSummary]
	EXECUTE [dbo].[LTS_LeadSummary] 
	
	EXECUTE [dbo].[LTS_SyncSalesData]
END