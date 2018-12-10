IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_InsertIn_DCRM_ADM_MappedUsers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_InsertIn_DCRM_ADM_MappedUsers]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(11th May 2015)
-- Description	:	Used to insert data in DCRM_ADM_MappedUsers
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_InsertIn_DCRM_ADM_MappedUsers]
	@OprUserId	INT,
	@ReportsTo INT = NULL,
	@BusinessUnitId INT,
	@IsActive BIT,
	@MappedBy INT = NULL,
	@SuspendedBy INT = NULL , 
	@IsMapped	BIT OUTPUT
AS
BEGIN

	DECLARE	@MappedId INT;
	
	SET @IsMapped = 0
		
	--now insert data in DCRM_ADM_MappedUsers
	INSERT INTO DCRM_ADM_MappedUsers
								(	
									OprUserId,
									BusinessUnitId,
									IsActive,
									MappedOn,
									MappedBy
								)
							VALUES
								(	
									@OprUserId,
									@BusinessUnitId,
									@IsActive,
									GETDATE(),
									@MappedBy
								)
	SET @MappedId = SCOPE_IDENTITY()
	IF @MappedId IS NOT NULL
		BEGIN
			--after insertion now update hierarchy
			EXEC DCRM_UpdateOprUsersHierarchy  @UpdateId=@MappedId, @ParentID=@ReportsTo
		END

	--If data inserted then insert data in DCRM_ADM_Users
	IF @MappedId > 0 
		BEGIN
			SET @IsMapped = 1
			
			SELECT UserId FROM DCRM_ADM_Users WHERE UserId = @OprUserId
			--If no record exist then insert User In DCRM_ADM_Users
			IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO DCRM_ADM_Users
									(	UserId,
										UpdatedOn,
										UpdatedBy,
										IsActive
									)
								VALUES
									(	@OprUserId,
										GETDATE(),
										@MappedBy,
										@IsActive
									)
				END

		END
END

