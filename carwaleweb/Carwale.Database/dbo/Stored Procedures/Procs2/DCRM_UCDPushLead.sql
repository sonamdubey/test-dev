IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UCDPushLead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UCDPushLead]
GO

	CREATE PROCEDURE [dbo].[DCRM_UCDPushLead](@CustomerCallingDetail CustomerCallingDetail READONLY)
AS
-- CREATED By Amit kumar on 16th april 2013.
 BEGIN
	DECLARE @customerId INT
	DECLARE @COUNT      INT
	DECLARE @ActionId   INT
	SELECT @customerId = CustomerId FROM @CustomerCallingDetail
	SELECT Id FROM DCRM_CustomerCalling WHERE CustomerId = @customerId AND ISNULL(ActionID,0) = 0
	IF @@ROWCOUNT = 0
		BEGIN
			 INSERT INTO DCRM_CustomerCalling
						 (CustomerId,
						  InquiryDate,	                
						  EntryDate,
						  EnteredBy,
						  CCExecutive)
			 SELECT CCD.CustomerId,
					CCD.InquiryDate,
					CCD.EntryDate,
					CCD.EnteredBy,
					CCD.CExecutive            
			FROM @CustomerCallingDetail CCD
		END
	ELSE
		BEGIN
			UPDATE DCRM_CustomerCalling SET EntryDate = GETDATE() WHERE CustomerId = @customerId
		END
  END 