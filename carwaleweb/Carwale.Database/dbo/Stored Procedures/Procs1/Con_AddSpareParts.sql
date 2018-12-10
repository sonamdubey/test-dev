IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_AddSpareParts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_AddSpareParts]
GO

	-- =============================================
-- Author:		<Deepak Tripathi>
-- Create date: <21-July-2008>
-- Description:	<Add SpareParts>
-- =============================================
CREATE PROCEDURE [dbo].[Con_AddSpareParts]
	-- Add the parameters for the stored procedure here
	@Id				NUMERIC,
	@PartName		VARCHAR(50),
	@Status			NUMERIC OUTPUT
AS
BEGIN
	IF @Id = -1
		BEGIN
			SELECT Id FROM Con_SpareParts WHERE PartName = @PartName

			IF @@RowCount = 0
				BEGIN
					INSERT INTO Con_SpareParts ( PartName )
					VALUES ( @PartName )
					
					SET @Status = 1
				END
		END
	ELSE
		BEGIN
			SELECT Id FROM Con_SpareParts WHERE PartName = @PartName AND Id <> @Id 

			IF @@RowCount = 0
				BEGIN
					UPDATE Con_SpareParts SET PartName = @PartName WHERE Id = @Id
					SET @Status = 1
				END
		END
END
