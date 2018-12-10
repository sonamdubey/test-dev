IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FOB_RequestRefund]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FOB_RequestRefund]
GO

	
--CREATED ON 11 SEP 2009 BY SENTIL    
--PROCEDURE FOR Request REFUND   
    
CREATE PROCEDURE [dbo].[FOB_RequestRefund]    
(    
	@BRId AS NUMERIC(18,0) = 0,
	@Amount AS NUMERIC(18,2) = 0.00,     
	@RefundRequestDate AS DATETIME,  
	@Reason AS VARCHAR(500) = NULL,  
	@ID AS BIGINT OUT     
)    
AS    
BEGIN    

	INSERT INTO FOB_BOOKINGREFUND    
		  ( BRId , Amount , RefundRequestDate , Reason)    
	VALUES( @BRId , @Amount , @RefundRequestDate , @Reason)     

	SET @ID = SCOPE_IDENTITY()    

    
--SELECT * FROM FOB_BOOKINGREFUND    
--TRUNCATE TABLE FOB_BOOKINGREFUND     
    
END     
    


