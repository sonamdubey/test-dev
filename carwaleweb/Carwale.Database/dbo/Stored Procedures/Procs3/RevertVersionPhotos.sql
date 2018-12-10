IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RevertVersionPhotos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RevertVersionPhotos]
GO

	-- =============================================
-- Author:		Ashwini Todkar
-- Create date: 10 June 2015
-- Description:	Proc to change version image path to model image path
-- =============================================
CREATE PROCEDURE [dbo].[RevertVersionPhotos]
	-- Add the parameters for the stored procedure here
	@VersionId INT
AS
BEGIN
	UPDATE CV
	SET CV.SmallPic = CM.SmallPic
		,CV.LargePic = CM.LargePic
		,CV.HostURL = CM.HostURL,
		CV.SpecialVersion = 0
	FROM CarVersions CV WITH (NOLOCK)
	JOIN CarModels CM   WITH (NOLOCK) ON CV.CarModelId = CM.ID 
	WHERE CV.ID = @VersionId
	
END

