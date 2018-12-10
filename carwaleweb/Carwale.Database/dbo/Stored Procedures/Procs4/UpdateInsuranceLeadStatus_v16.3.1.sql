IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateInsuranceLeadStatus_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateInsuranceLeadStatus_v16]
GO

	-- =============================================
-- Author:		Piyush Sahu
-- Create date: 03 March 2016
-- Description:	Proc to update the status received by policyboss
-- =============================================
CREATE PROCEDURE [dbo].[UpdateInsuranceLeadStatus_v16.3.1]
	@Status VARCHAR(50),
	@Id INT,
	@StatusId TINYINT,
	@Premium DECIMAL(18,2) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE INS_PremiumLeads
	SET PushStatus = @Status, StatusId = @StatusId, Premium = @Premium, ResponseTime = GETDATE()
	WHERE ID = @Id
END
