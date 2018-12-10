IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RevertVersionPhotos_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RevertVersionPhotos_v15]
GO

	-- =============================================
-- Author:		Ashwini Todkar
-- Create date: 10 June 2015
-- Description:	Proc to change version image path to model image path
-- =============================================
CREATE PROCEDURE [dbo].[RevertVersionPhotos_v15.8.1]
	-- Add the parameters for the stored procedure here
	@VersionId INT
AS
BEGIN
	UPDATE CV
	SET CV.OriginalImgPath = CM.OriginalImgPath,
		CV.SpecialVersion = 0
	FROM CarVersions CV WITH (NOLOCK)
	JOIN CarModels CM   WITH (NOLOCK) ON CV.CarModelId = CM.ID 
	WHERE CV.ID = @VersionId
	
END

