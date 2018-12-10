IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RetrieveLeadsForDay]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RetrieveLeadsForDay]
GO

	-- =============================================
-- Author:		<Prashant Vishe>
-- Create date: <21 Oct 2013>
-- Description:	<For retrieving leads for specific day.>
-- =============================================
CREATE PROCEDURE [dbo].[RetrieveLeadsForDay]
	-- Add the parameters for the stored procedure here
	@Id numeric
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id,day,StartTime,EndTime,LastUpdatedOn,LastUpdatedBy 
	FROM CRM_LeadAutoVerificationConfig 
	WHERE Id=@Id
END

