IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_PotentiallyLostSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_PotentiallyLostSave]
GO

	
CREATE PROCEDURE [dbo].[CRM_PotentiallyLostSave]  
-- Modifier: Amit Kumar (23rd may 2013) SET Default value of @status = 0
@CbdId              NUMERIC(18,0),  
@Comments			VARCHAR(1500),  
@TaggedBy           NUMERIC(18,0),   
@TaggedOn           DATETIME,  
@UpdatedBy			NUMERIC(18,0),  
@UpdatedOn			DATETIME,  
@DealerId			NUMERIC(18,0),  
@model				VARCHAR(50),  
@make				VARCHAR(50),  
@Status				NUMERIC OUTPUT 
  
AS   
BEGIN  
 UPDATE CRM_PotentiallyLostCase SET Comment=@Comments , Make =@make , Model = @model  WHERE CBDId = @CbdId AND IsActionTaken = 0  
 --SELECT id FROM CRM_PotentiallyLostCase WITH(NOLOCK) WHERE CBDId = @CbdId AND IsActionTaken = 0     
 --print @@ROWCOUNT
 
	IF @@ROWCOUNT = 0  
		BEGIN  
		   INSERT INTO CRM_PotentiallyLostCase(CBDId,Comment,TaggedBy,TaggedOn,UpdatedBy,UpdatedOn,DealerId,Make,Model)   
		   VALUES (@CbdId,@Comments,@TaggedBy,@TaggedOn,@UpdatedBy,@UpdatedOn,@DealerId,@make ,@model)  
		   SET @Status = SCOPE_IDENTITY()  
		END 
	ELSE
		BEGIN
			 SET @Status = 0 
		END 
END  