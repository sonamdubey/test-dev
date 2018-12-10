IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_UpdatePGTransaction]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_UpdatePGTransaction]
GO

	
-- =============================================
-- Author:		Vaibhav K
-- Create date: 9-8-2013
-- Description:	Update PG Transaction for Audi Booking Engine
-- =============================================
CREATE PROCEDURE [dbo].[OLM_AudiBE_UpdatePGTransaction]
	-- Add the parameters for the stored procedure here
	@PGTransactionId		numeric,
	@ResponseCode			numeric,
	@ResponseMsg			varchar(500),
	@EPGTransactionId		varchar(100),
	@AuthId					varchar(100),
	@ProcessCompleted		bit,
	@TransactionCompleted	bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- Insert statements for procedure here
	
	SET NOCOUNT ON;
	
	IF @PGTransactionId <> -1
		BEGIN
			UPDATE OLM_AudiBE_PGTransactions
				SET	ResponseCode = @ResponseCode,
					ResponseMsg = @ResponseMsg, EPGTransactionId = @EPGTransactionId,
					AuthId = @AuthId, ProcessCompleted = @ProcessCompleted,
					TransactionCompleted = @TransactionCompleted
			WHERE Id = @PGTransactionId
		END
	
END

