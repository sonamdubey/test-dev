IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BerkshireInsuranceLeadsUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BerkshireInsuranceLeadsUpdate]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: 30-Jul-2012
-- Description:	updtae response status of requested Berkshire APIs to push insurance leads
-- =============================================
CREATE PROCEDURE [dbo].[BerkshireInsuranceLeadsUpdate]
	-- Add the parameters for the stored procedure here
	@LeadId BIGINT,
	@ReturnedBerkshireLeadId BIGINT,
	@PushStatusMessage VARCHAR(200),
	@IsPushedToBerkshire BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE BerkshireInsuranceLeads 
	SET ReturnedBerkshireLeadId = @ReturnedBerkshireLeadId,
	PushStatusMessage = @PushStatusMessage,
	IsPushedToBerkshire=@IsPushedToBerkshire
	WHERE BerkshireLeadId = @LeadId
END
