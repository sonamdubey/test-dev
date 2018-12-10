IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateInsuranceLeadStatus_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateInsuranceLeadStatus_v16]
GO

	
CREATE PROCEDURE [dbo].[UpdateInsuranceLeadStatus_v16.6.2]
	@Status VARCHAR(50),
	@Id INT,
	@StatusId TINYINT,
	@Premium DECIMAL(18,2) = null,
	@Quotation varchar(800)
AS
BEGIN
-- =============================================
-- Author:		Piyush Sahu
-- Create date: 03 March 2016
-- Description:	Proc to update the status received by policyboss
-- =============================================
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE INS_PremiumLeads
			SET PushStatus = @Status, 
			StatusId = @StatusId, 
			Premium = ISNULL(@Premium,Premium), ResponseTime = GETDATE(),
			Quotation = ISNULL(@Quotation,Quotation)
	WHERE ID = @Id
END

