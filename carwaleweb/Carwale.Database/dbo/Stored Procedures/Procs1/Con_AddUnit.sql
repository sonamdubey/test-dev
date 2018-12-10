IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_AddUnit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_AddUnit]
GO

	
-- =============================================
-- Author:		<Deepak Tripathi>
-- Create date: <21-July-2008>
-- Description:	<Add SpareParts>
-- =============================================
CREATE PROCEDURE [dbo].[Con_AddUnit]
	-- Add the parameters for the stored procedure here
	@Id				NUMERIC,
	@Name			VARCHAR(50),
	@Status			BIT OUTPUT
AS
BEGIN
	IF @Id = -1
		BEGIN
			SELECT Id FROM Con_Units 
			WHERE Name = @Name

			IF @@RowCount = 0
				BEGIN
					INSERT INTO Con_Units ( Name )
					VALUES ( @Name )
					
					SET @Status = 1
				END
		END
	ELSE
		BEGIN
			SELECT Id FROM Con_Units 
			WHERE Name = @Name AND Id <> @Id 

			IF @@RowCount = 0
				BEGIN
					UPDATE Con_Units 
					SET Name = @Name WHERE Id = @Id
					SET @Status = 1
				END
		END
END

