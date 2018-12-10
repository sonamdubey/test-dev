IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UpdateDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UpdateDealers]
GO

	
CREATE PROCEDURE [dbo].[DCRM_UpdateDealers]
	@DealerIds		AS VARCHAR(1000),
	@NewUserId      AS NUMERIC,
	@NewUser		VARCHAR(50),
	@OldUserId		AS NUMERIC,
	@OldUser		AS VARCHAR(50),
	@UpdatedBy      AS NUMERIC = 13,
	@RoleId         AS NUMERIC,
	@AlertId		AS INT,
	@Result 		INT OUTPUT
	
AS
	
BEGIN
	DECLARE @Dealer VARCHAR(50) 
	DECLARE @DealerIndx VARCHAR(50)
	DECLARE @Subject	VARCHAR(100)
	SET		@Result = -1
	
	SET @Subject = 'Call transferred from '+ @OldUser +' to ' + @NewUser
								
	IF @DealerIds <> ''
		BEGIN
			SET @DealerIds =  @DealerIds + ',' 	  
			WHILE @DealerIds <> ''
				BEGIN
					SET @DealerIndx = CHARINDEX(',' , @DealerIds)
					IF  @DealerIndx > 0
					   BEGIN 
							SET @Dealer = LEFT(@DealerIds, @DealerIndx-1)  
							SET @DealerIds = RIGHT(@DealerIds, LEN(@DealerIds)- @DealerIndx)
						  
							-- Change Role
							EXECUTE [DCRM].[AssignDealerOnUserRole] @NewUserId,@RoleId,@Dealer,@UpdatedBy
							
							--Transfer Calls
							EXECUTE DCRM_UpdateDCRMCalls @Dealer, @RoleId, @OldUserId, @NewUserId, @OldUser, @NewUser,
									@UpdatedBy, @Subject, @AlertId
								
							SET @Result = 1
						END
				END
		END
END