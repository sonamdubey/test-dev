IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_InsertVersionColor]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_InsertVersionColor]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <6/6/2014>
-- Description:	<Copy color from one version to other>
-- =============================================
CREATE PROCEDURE [dbo].[Con_InsertVersionColor]
	@SrcVersionId	INT,
	@TarVersionId	INT,
	@ColorId		INT
AS
BEGIN
	DECLARE @HexCode VARCHAR(50)
	DECLARE @Code VARCHAR(50)
	DECLARE @Color VARCHAR(50)
	SELECT @HexCode=HexCode , @Code=Code , @Color = Color FROM VersionColors WHERE CarVersionID = @SrcVersionId AND ID = @ColorId
	SELECT ID FROM VersionColors WHERE CarVersionID = @TarVersionId AND Color = @Color AND HexCode=@HexCode AND Code = @Code
	IF(@@ROWCOUNT = 0)		
		INSERT INTO VersionColors ( Color, Code, HexCode, CarVersionId ) VALUES(@Color,@Code,@HexCode,@TarVersionId)

END
