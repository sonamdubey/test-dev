IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertUpcomingCarVersion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertUpcomingCarVersion]
GO

	

-- =============================================
-- Author	:	Sachin Bharti(1st Sept 2016)
-- Description	:	Insert upcoming versions in ExpectedCarLaunches	
-- =============================================
CREATE PROCEDURE [dbo].[InsertUpcomingCarVersion]
	@ModelId INT,
	@VersionId	INT,
	@IsLaunched BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @ModelName VARCHAR(50),@MakeId INT
	SELECT @MakeId = CarMakeId,@ModelName = Name FROM CarModels(NOLOCK) WHERE Id = @ModelId
	SELECT ID FROM ExpectedCarLaunches(NOLOCK) WHERE CarVersionId = @VersionId AND IsDeleted = 0
	IF @@ROWCOUNT = 0
	BEGIN
		INSERT INTO ExpectedCarLaunches
		(	CarMakeId,
			CarModelId,
			ModelName,
			EntryDate,
			IsLaunched,
			CarVersionId
		)
		VALUES
		(	@MakeId,
			@ModelId,
			@ModelName,
			GETDATE(),
			@IsLaunched,
			@VersionId
		)
	END
END


