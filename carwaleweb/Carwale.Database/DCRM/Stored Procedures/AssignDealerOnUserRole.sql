IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[DCRM].[AssignDealerOnUserRole]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [DCRM].[AssignDealerOnUserRole]
GO

	

-- Description	:	Assign Dealer OnUserRole
-- Author		:	Dilip V. 20-Jul-2012
-- Modifier		:	
CREATE PROCEDURE [DCRM].[AssignDealerOnUserRole]
	@UserId		BIGINT,
	@RoleId		BIGINT,
	@DealerId	BIGINT,
	@UpdatedBy	BIGINT = 13,
	@AlertId	INT = NULL,
	@NewUserName	VARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON			
	DECLARE @SalesEXECId		INT = -1
	DECLARE @SalesEXECName	VARCHAR(50) = NULL
	DECLARE @ServiceEXECId	INT = -1
	DECLARE @ServiceEXECName	VARCHAR = NULL
	DECLARE @Subject	VARCHAR(100)
	
	IF @UserId <> -1 AND @UserId <> 0 AND @RoleId <> -1 AND @RoleId <> 0 AND @DealerId <> -1 AND @DealerId <> 0
	BEGIN
		--If user transfer either for Sales or Service field executives
		IF @RoleId = 3 OR @RoleId = 5
			BEGIN
				SELECT @SalesEXECId = DAU.UserId ,@SalesEXECName = OU.UserName
				FROM DCRM_ADM_UserDealers DAU WITH(NOLOCK) 
				INNER JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = DAU.UserId
				WHERE DealerId = @DealerId  AND RoleId = 3
				
				SELECT @ServiceEXECId = DAU.UserId ,@ServiceEXECName = OU.UserName
				FROM DCRM_ADM_UserDealers DAU WITH(NOLOCK) 
				INNER JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = DAU.UserId
				WHERE DealerId = @DealerId  AND RoleId = 5
				
				--If transfer to Sales field then delete Service field executive for Dealer
				IF @RoleId = 3 AND @ServiceEXECId IS NOT NULL 
					BEGIN
						SET @Subject = 'Call transferred from '+ @ServiceEXECName +' to ' + @NewUserName
						
						--Transfer Calls
						EXECUTE DCRM_UpdateDCRMCalls @DealerId, @RoleId, @ServiceEXECId, @UserId , @ServiceEXECName, @NewUserName,
								@UpdatedBy, @Subject, @AlertId
						
						DELETE FROM DCRM_ADM_UserDealers WHERE DealerId = @DealerId AND RoleId = 5	
					END
				--If transfer to Service field then delete Sales field executive for Dealer
				ELSE IF @RoleId = 5 AND @SalesEXECId IS NOT NULL 
					BEGIN
						SET @Subject = 'Call transferred from '+ @SalesEXECName +' to ' + @NewUserName
						
						--Transfer Calls
						EXECUTE DCRM_UpdateDCRMCalls @DealerId, @RoleId, @SalesEXECId, @UserId , @SalesEXECName, @NewUserName,
								@UpdatedBy, @Subject, @AlertId
						
						DELETE FROM DCRM_ADM_UserDealers WHERE DealerId = @DealerId AND RoleId = 5	
					END
					
				--Made a entry in DCRM_SalesDealer for new executives
				UPDATE DCRM_ADM_UserDealers SET UserId = @UserId WHERE DealerId = @DealerId AND RoleId = @RoleId
				IF @@ROWCOUNT = 0
					BEGIN
						INSERT INTO DCRM_ADM_UserDealers (UserId,RoleId,DealerId,UpdatedOn,UpdatedBy) VALUES (@UserId,@RoleId,@DealerId,GETDATE(),@UpdatedBy)
					END
			END
		ELSE
			BEGIN
				UPDATE DCRM_ADM_UserDealers SET UserId = @UserId WHERE DealerId = @DealerId AND RoleId = @RoleId
				IF @@ROWCOUNT = 0
					BEGIN
						INSERT INTO DCRM_ADM_UserDealers (UserId,RoleId,DealerId,UpdatedOn,UpdatedBy) VALUES (@UserId,@RoleId,@DealerId,GETDATE(),@UpdatedBy)
					END
			END
	END
END