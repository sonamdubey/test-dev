IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveMakeGroupDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveMakeGroupDetails]
GO

	
CREATE PROCEDURE [dbo].[SaveMakeGroupDetails]
@MakeGroupId NUMERIC (18,0),
@MakeId NUMERIC (18, 0),
@IsSaved BIT OUTPUT
AS
BEGIN

	DECLARE @Id NUMERIC (18,0)
	DECLARE @IsDeleted BIT
	SELECT @Id = ID, @IsDeleted = IsDeleted FROM MakeGroupDetails WHERE MakeGroupId = @MakeGroupId AND MakeId = @MakeId

	IF @Id IS NULL
	BEGIN
		INSERT INTO MakeGroupDetails
		(MakeGroupId, MakeId)
		VALUES
		(@MakeGroupId, @MakeId)
		
		SET @IsSaved = 1
	END
	ELSE IF @Id IS NOT NULL AND @IsDeleted = 1
	BEGIN
		UPDATE MakeGroupDetails
		SET IsDeleted = 0
		WHERE ID = @Id
		
		SET @IsSaved = 1	
	END
	ELSE IF @Id IS NOT NULL AND @IsDeleted = 0
	BEGIN
		SET @IsSaved = 0
	END
	
END
