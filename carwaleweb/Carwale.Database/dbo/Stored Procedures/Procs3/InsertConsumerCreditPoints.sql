IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertConsumerCreditPoints]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertConsumerCreditPoints]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR Class Packages TABLE Packages
CREATE PROCEDURE [dbo].[InsertConsumerCreditPoints]
	@Id			NUMERIC,	-- Id. Will be -1 if Its Insertion
	@ConsumerType	SMALLINT,	
	@ConsumerId		NUMERIC,	--validaty in days
	@ExpiryDate		DATETIME,	
	@Points		NUMERIC,
	@STATUS		INTEGER OUTPUT	--return value, -1 for unsuccessfull attempt, and 0 for success
	
 AS
	
BEGIN
	
	set nocount on;
	SET @Status = 0
	
	IF @Id = -1 
		BEGIN
		
			INSERT INTO ConsumerCreditPoints (ConsumerType, ConsumerId, ExpiryDate, Points)		
			VALUES (@ConsumerType, @ConsumerId, @ExpiryDate, @Points)
			declare @RowId numeric
			set @RowId= SCOPE_IDENTITY();
		
			declare		   			   
			   @ActualValidity    NUMERIC =null,  	    
			   @PackageType   SMALLINT =null,
			   @PackageId   NUMERIC =null,
			   @InsertId int =2
begin try
			exec [dbo].[SyncConsumerCreditPointsWithMysql]
			   @RowId,
			   @ConsumerId,  
			   @ConsumerType,  
			   @Points,  			     	    
			   @PackageType,
			   @ExpiryDate ,
			   @PackageId,
			   @InsertId 
 	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','InsertConsumerCreditPoints',ERROR_MESSAGE(),'SyncConsumerCreditPointsWithMysql',@RowId,GETDATE(),@InsertId)
	END CATCH			   
			SET @Status = 1
		END
	ELSE
		BEGIN
			UPDATE ConsumerCreditPoints SET ExpiryDate=@ExpiryDate, Points=@Points
			WHERE ConsumerType = @ConsumerType AND ConsumerId=@ConsumerId
			
			Declare	
			@PreviousExpiryDate DATETIME = null,     
			@updateType Numeric = 9
			set @ActualValidity = null
			set @PackageType=null
			set @PackageId=null
			begin try
			exec [dbo].[SyncConsumerCreditPointsWithMysqlUpdate]
			 @ConsumerId,  
			 @ConsumerType,  
			 @Points,   
			 @ActualValidity,  
			 @ExpiryDate,	
			 @PreviousExpiryDate,     
			 @PackageType,
			 @PackageId,
			 @updateType,
			 @Id
			  end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','InsertConsumerCreditPoints',ERROR_MESSAGE(),'SyncConsumerCreditPointsWithMysqlUpdate',@ConsumerId,GETDATE(),@updateType)
	END CATCH	
			SET @Status=2
		END
	
END

