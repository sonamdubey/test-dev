IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetTVScoreBoard]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetTVScoreBoard]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 15 July 2014
-- Description:	FLC TV Score Board details for that time period (MTD). Multiple tables returned
--				1. Targets table
--				2. Lead Processed
--				3. Lead Assigned
--				4. Missed follow ups
--				5. Pool leads
-- Modifier:	Get data from table as per deepak with following ProcessType
--				Type 1 - make wise lead proccessed 
--				Type 2 - make wise lead assigned
--				Type 3 Get follow up leads i.e. lead that are in missed (set as followup)
--				Type 4 Leads in pool owner -1 leadstageid 1
--				Type 5 individual wise processed leads
--				Type 6 individual wise assigned leads
-- =============================================
CREATE PROCEDURE [dbo].[CRM_GetTVScoreBoard]
	-- Add the parameters for the stored procedure here
	--@FromDate		DATETIME = NULL,
	--@ToDate			DATETIME = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	--new table to be used as per Deepak
	SELECT * FROM CRM_FLCScoreboardData WITH (NOLOCK) ORDER BY MakeName


END
