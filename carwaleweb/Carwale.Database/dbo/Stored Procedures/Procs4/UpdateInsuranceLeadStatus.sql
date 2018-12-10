IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateInsuranceLeadStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateInsuranceLeadStatus]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 13 April 2015
-- Description:	Proc to update the status received by chola mandalam
-- =============================================
CREATE PROCEDURE [dbo].[UpdateInsuranceLeadStatus]
	@Status VARCHAR(50),
	@Id BIGINT,
	@StatusId TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE INS_PremiumLeads
	SET PushStatus = @Status, StatusId = @StatusId, ResponseTime = GETDATE()
	WHERE ID = @Id
END
