IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateLeads]
GO

	-- =============================================
-- Author:		<Prashant Vishe>
-- Create date: <21 Oct 2013>
-- Description:	<For Updating leads data.>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateLeads]
	-- Add the parameters for the stored procedure here
	@Id numeric,
	@StartTime tinyint,
	@EndTime tinyint,
	@LastUpdatedId numeric
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CRM_LeadAutoVerificationConfig
	SET StartTime=@StartTime,
		EndTime=@EndTime,
		LastUpdatedOn=GETDATE(),
		LastUpdatedBy=(select UserName from OprUsers where Id=@LastUpdatedId)
	WHERE Id=@Id

END

