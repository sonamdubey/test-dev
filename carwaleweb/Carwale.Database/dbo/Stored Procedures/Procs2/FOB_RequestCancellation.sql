IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FOB_RequestCancellation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FOB_RequestCancellation]
GO

	
--CREATED ON 10 SEP 2009 BY SENTIL    
--PROCEDURE FOR SUBMIT REQUEST FOR CANCELLATION   
    
CREATE PROCEDURE [dbo].[FOB_RequestCancellation]
(    
	@BRId AS NUMERIC(18,0) = 0,    
	@CancelRequestDate AS DATETIME,
	@Amount AS NUMERIC(18,2) = 0.00, 
	@Reason AS VARCHAR(500) = NULL,  
	@ID AS BIGINT OUT     
)    
AS    
BEGIN    
    
	IF NOT EXISTS(SELECT BRId FROM FOB_BookingCancellation WHERE BRId = @BRId) 
		BEGIN
		  INSERT INTO FOB_BookingCancellation    
				( BRId,  CancelRequestDate, Amount , Reason )    
		  VALUES( @BRId, @CancelRequestDate, @Amount , @Reason )     
		    
		 SET @ID = SCOPE_IDENTITY()    
		END 
	ELSE
		BEGIN
			SET @ID= -1
		END			    
--SELECT * FROM FOB_BookingCancellation    
--TRUNCATE TABLE FOB_BookingCancellation     
    
END     


