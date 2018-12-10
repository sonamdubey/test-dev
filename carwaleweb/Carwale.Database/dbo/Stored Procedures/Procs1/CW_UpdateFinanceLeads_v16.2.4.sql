IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_UpdateFinanceLeads_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_UpdateFinanceLeads_v16]
GO

	-- =============================================
-- Author:		Piyush Sahu
-- Create date: 2/3/2016
-- Description:	Proc to update the status received by PaisaBazaar
-- =============================================
CREATE PROCEDURE [dbo].[CW_UpdateFinanceLeads_v16.2.4]
	@FinanceLeadId  Int,
	@ApiResponse  VARCHAR(100) = NULL,
	@IsPushSuccess bit = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CW_FinanceLeads
	SET ApiResponse = @ApiResponse,UpdatedOn = GETDATE(),IsPushSuccess = @IsPushSuccess
	WHERE Id = @FinanceLeadId

END
