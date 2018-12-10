IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_TransferOPRUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_TransferOPRUser]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(13th May 2015)
-- Description	:	Used to transfer OPR user from one to second reporting person
--					also update all the children those are earlier reporting to
--					transferred OPR user.
-- execute DCRM_TransferOPRUser 3,7,54,3,null,null
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_TransferOPRUser]
	
	@UserMappingId	INT,
	@OprUserId	INT,
	@ReportsTo	INT = NULL,
	@TransferBy INT = NULL,
	@IsRecursive	BIT = NULL,
	@IsTransfered	BIT OUTPUT 
AS
	BEGIN
		
		PRINT @UserMappingId 
		PRINT @OprUserId
		PRINT @ReportsTo 
		PRINT @IsRecursive

		IF @IsRecursive IS NULL
			SET @IsTransfered = 0

		DECLARE @FirstLevelReportingUsers TABLE (Id INT IDENTITY(1,1) , MappingId	INT , FirstLevelUserId INT)--to store first level reporting users

		DECLARE	@LevelOneReporterMapppingId	INT,
				@LevelOneTotalReportingUsers	INT,
				@LevelOneRowCount	INT,
				@LevelOneUserId		INT,
				@LevelOneNodeRec	HIERARCHYID
		
		--read node code of user before transfer
		SET @LevelOneNodeRec = (SELECT NodeRec FROM DCRM_ADM_MappedUsers where OprUserId = @OprUserId AND IsActive = 1)
		
		PRINT CAST (@LevelOneNodeRec AS VARCHAR)

		--now transfer the user and update its hierarchy
		EXEC DCRM_UpdateOprUsersHierarchy  @UpdateId=@UserMappingId, @ParentID=@ReportsTo
		
		--updating other details
		IF @@ROWCOUNT = 1 AND @IsRecursive IS NULL
			BEGIN
				UPDATE 
					DCRM_ADM_MappedUsers
				SET
					TransferBy = @TransferBy,
					TransferOn = GETDATE()
				WHERE
					Id = @UserMappingId
			END
		
		--after successful transfer also upadting its children hierarchy
		IF ( @@ROWCOUNT = 1 OR @IsRecursive = 1)
			BEGIN
				IF @IsRecursive IS NULL
					SET @IsTransfered = 1
				
				--first check if there is any user who reprorts this user
				SELECT NodeCode From DCRM_ADM_MappedUsers where NodeRec.GetAncestor(1) = @LevelOneNodeRec AND IsActive = 1
				SET @LevelOneTotalReportingUsers = @@ROWCOUNT
				PRINT @LevelOneTotalReportingUsers

				--if children exist
				IF @LevelOneTotalReportingUsers >0
					BEGIN
						
						--now insert data of reporting users in first level temp table
						INSERT INTO @FirstLevelReportingUsers (MappingId, FirstLevelUserId) 
									SELECT Id , OprUserId FROM DCRM_ADM_MappedUsers	WHERE NodeRec.GetAncestor(1) = @LevelOneNodeRec AND IsActive = 1

						SET @LevelOneRowCount = 1

						WHILE(@LevelOneRowCount <= @LevelOneTotalReportingUsers)
							BEGIN
								--read data from first level reporting users table
								SELECT 
									@LevelOneReporterMapppingId = MappingId ,
									@LevelOneUserId = FirstLevelUserId
								FROM 
									@FirstLevelReportingUsers 
								WHERE 
									Id = @LevelOneRowCount
									
								PRINT 'Inside If Block'
								PRINT @LevelOneReporterMapppingId
								PRINT @LevelOneUserId
								PRINT @OprUserId

								--calling recursively to update the hierarchy
								EXECUTE DCRM_TransferOPRUser @LevelOneReporterMapppingId,@LevelOneUserId,@OprUserId,@TransferBy,1,NULL 
								
								SET @LevelOneRowCount += 1
							END
					END 
			END
		PRINT @IsTransfered
	END
