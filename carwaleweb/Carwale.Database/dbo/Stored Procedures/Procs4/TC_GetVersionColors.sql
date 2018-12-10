IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetVersionColors]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetVersionColors]
GO

	-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date,06th June, 2013>
-- Description:	<Description,Gives new car Models based on user divisions>
-- Modified By : Tejashree Patil on 28 Jun 2013,Fetched details of colors independent on versionId.
-- Modified By : Tejashree Patil on 30 Aug 2013,Added condition of IsActive=1 in IF condition.
-- Modified By : Tejashree Patil on 30-10-2014, Added @ApplicationId to identify application and joined with vwAllMMV view.
--Modified By : Ashwini Dhamankar on July 20,2015 (Fetched VersionHexCode) and order colors alphabetically
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetVersionColors] 
	-- Add the parameters for the stored procedure here
	@VersionId INT,
	@ApplicationId TINYINT = 1 --  1 for Carwale
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF(@VersionId IS NOT NULL)
    BEGIN
		--SELECT V.ID, V.Color FROM VersionColors V WHERE CarVersionID = @VersionId AND IsActive=1
		
		SELECT V.VersionColorsId ID, V.VersionColor Color,V.VersionHexCode HexCode
		FROM   vwAllVersionColors V
		WHERE  V.VersionId = @VersionId
			   AND ApplicationId=@ApplicationId
		ORDER BY V.VersionColor
    END
    ELSE IF(@VersionId IS NULL)
    BEGIN
		--SELECT DISTINCT CarVersionID,Color,ID FROM VersionColors WHERE IsActive=1
		
		SELECT V.VersionId CarVersionID, V.VersionColorsId ID, V.VersionColor Color
		FROM   vwAllVersionColors V --VersionColors V 
	END
END

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


