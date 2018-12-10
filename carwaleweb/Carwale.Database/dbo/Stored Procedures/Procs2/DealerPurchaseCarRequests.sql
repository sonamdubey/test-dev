IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DealerPurchaseCarRequests]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DealerPurchaseCarRequests]
GO

	


CREATE PROCEDURE [dbo].[DealerPurchaseCarRequests]
	@CarId			NUMERIC,	
	@isDealer		BIT,	
	@DealerId		NUMERIC,	
	@Message		VARCHAR(500),	
	@MsgSentTime		DATETIME
	
 AS
	
BEGIN
	
	
	INSERT INTO DealerCarRequests ( CarId , isDealer , DealerId , Message , MsgSentTime , isActive  )
	VALUES ( @CarId , @isDealer , @DealerId , @Message , @MsgSentTime ,  1)
	
END
