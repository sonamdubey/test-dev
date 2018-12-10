IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_PressReleases_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_PressReleases_SP]
GO

	

CREATE PROCEDURE [dbo].[CW_PressReleases_SP]
	-- Add the parameters for the stored procedure here
	@PRID		    INT,
	@Title			VARCHAR(150),
	@Summary		VARCHAR(250),
	@Detailed		VARCHAR(MAX),
	@AttachedFile	VARCHAR(50) = NULL,
	@ReleaseDate	DATETIME,
	@IsActive		BIT,
	@ID             INT OUTPUT 
AS
BEGIN
	IF @PRID = -1 
		BEGIN
			INSERT INTO CW_PressReleases
			(Title, Summary, Detailed, AttachedFile, ReleaseDate, IsActive)
			VALUES (@Title, @Summary, @Detailed, @AttachedFile, @ReleaseDate, @IsActive)
			SET @ID = SCOPE_IDENTITY() 
		END
	ELSE BEGIN
			UPDATE CW_PressReleases		 
			SET Title = @Title, Summary = @Summary, Detailed = @Detailed, AttachedFile= @AttachedFile, ReleaseDate = @ReleaseDate, IsActive = @IsActive
			WHERE Id = @PRID
			SET @ID = @PRID
	END
END


