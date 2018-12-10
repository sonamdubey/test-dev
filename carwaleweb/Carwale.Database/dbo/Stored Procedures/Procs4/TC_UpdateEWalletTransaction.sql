IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateEWalletTransaction]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateEWalletTransaction]
GO

	

-- =============================================
-- Author:		VINAY KUMAR PRAJAPATI
-- Create date:  12th Jan 2016
-- Description:	Save Transaction details for PayUmoney 
-- Updated By : Vinay Kumar prajapati  18th Feb 2016 , If payUmoney transaction failure Set requestType =1 (Pending)
-- =============================================
CREATE  PROCEDURE [dbo].[TC_UpdateEWalletTransaction]
	@TC_RedeemedPointsId	 INT,	
	@PayUMoneyTransactionId	 INT,
	@PayUMoneyStatus        BIT ,
	@PayUMoneyMessage    VARCHAR(500)
 
AS

BEGIN
	UPDATE TC_RedeemedPoints 
	SET  PayUMoneyTransactionId=@PayUMoneyTransactionId , PayUMoneyStatus = @PayUMoneyStatus , PayUMoneyMessage =@PayUMoneyMessage 	
	WHERE Id = @TC_RedeemedPointsId

	UPDATE TC_RedeemedPointsLog 
	SET  PayUMoneyTransactionId=@PayUMoneyTransactionId , PayUMoneyStatus = @PayUMoneyStatus , PayUMoneyMessage =@PayUMoneyMessage 	
	WHERE TC_RedeemedPointsId = @TC_RedeemedPointsId

	-- When  transaction fail then Update status Pending  
	IF @PayUMoneyStatus = 0
	    BEGIN
			UPDATE TC_RedeemedPoints 
			SET   RequestType = 1	
			WHERE Id = @TC_RedeemedPointsId
		END
	

END





