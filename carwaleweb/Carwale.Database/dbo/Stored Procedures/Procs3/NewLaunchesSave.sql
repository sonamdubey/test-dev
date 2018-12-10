IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NewLaunchesSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NewLaunchesSave]
GO

	
CREATE PROCEDURE [dbo].[NewLaunchesSave]
@ModelId NUMERIC (18,0),
@Result NUMERIC (18,0) OUTPUT
AS
BEGIN

	DECLARE @Id NUMERIC (18,0)
	SET @Id = (SELECT Id FROM NewLaunches WHERE ModelId = @ModelId)
	
	IF @Id IS NULL
	BEGIN
	
		INSERT INTO NewLaunches
		(ModelId, EntryDate)
		VALUES
		(@ModelId, GETDATE())
		
		SET @Result = 1
	
	END
	ELSE
	BEGIN
	
		SET @Result = 2
	
	END

END
