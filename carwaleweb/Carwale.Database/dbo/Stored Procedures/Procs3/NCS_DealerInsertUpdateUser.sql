IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_DealerInsertUpdateUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_DealerInsertUpdateUser]
GO

	



CREATE PROCEDURE [dbo].[NCS_DealerInsertUpdateUser]

@ID NUMERIC,
@OrgName VARCHAR(200),
@LoginId VARCHAR(50),
@PassWord VARCHAR(50),
@MakeId NUMERIC,
@CityId NUMERIC,
@UpdatedBy NUMERIC,
@IsActive BIT,
@CreateDate DATETIME,
@UpdatedDate DATETIME,
@IsCWExecutive BIT,
@IsNCD BIT,
@Status NUMERIC OUTPUT


AS
	
BEGIN
	IF @ID = -1
		BEGIN
			IF EXISTS (SELECT Id FROM NCS_DealerOrganization WHERE LoginId = @LoginId)
				BEGIN
					SET @Status = 0
				END
			ELSE
				BEGIN
					INSERT INTO NCS_DealerOrganization (Name,LoginId,PassWord,MakeId,CityId,UpdatedBy,IsActive,CreateDate,IsCWExecutive,IsNCD)
					VALUES (@OrgName, @LoginId, @PassWord,@MakeId,@CityId,@UpdatedBy,@IsActive,@CreateDate,@IsCWExecutive,@IsNCD)
					SET @Status = SCOPE_IDENTITY()
				END
				
		END
	ELSE
		BEGIN
		
			IF EXISTS (SELECT Id FROM NCS_DealerOrganization WHERE ID <> @Id AND LoginId = @LoginId)
				BEGIN
					SET @Status = 0
				END
			ELSE	
				BEGIN
					Update NCS_DealerOrganization 
					Set Name=@OrgName, LoginId = @LoginId , PassWord = @PassWord, MakeId = @MakeId,CityId = @CityId,
					UpdatedBy = @UpdatedBy, IsActive = @IsActive, UpdatedDate = @UpdatedDate,
					IsCWExecutive = @IsCWExecutive,IsNCD = @IsNCD
					Where ID = @ID
					SET @Status = 1
				END
		END
END









