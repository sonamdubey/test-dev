IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SaveLogs]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SaveLogs]
GO

	

CREATE PROCEDURE [dbo].[DCRM_SaveLogs]
	@InquiryId			NUMERIC = NULL,
	@DealerId		    NUMERIC = NULL,
	@CustomerId		    NUMERIC = NULL,
	@ActionId			NUMERIC,
	@LogBy				NUMERIC	
	
AS
BEGIN
	
	INSERT INTO DCRM_ActionLog(DealerId, InquiryId, CustomerId, ActionId, LogDate, LogBy) 
	VALUES(@DealerId, @InquiryId, @CustomerId, @ActionId, GETDATE(), @LogBy)
	
END


