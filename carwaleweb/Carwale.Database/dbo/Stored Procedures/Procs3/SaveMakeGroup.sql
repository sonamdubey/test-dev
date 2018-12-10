IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveMakeGroup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveMakeGroup]
GO

	
CREATE PROCEDURE [dbo].[SaveMakeGroup]
@Name VARCHAR(50),
@ID NUMERIC(18,0) OUTPUT 
AS
BEGIN

	INSERT INTO MakeGroups 
	(Name) 
	VALUES 
	(@Name)
	
	SET @ID = SCOPE_IDENTITY()

END
