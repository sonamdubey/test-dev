IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Get_Used_Car_Buying_Guide]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Get_Used_Car_Buying_Guide]
GO

	-- =============================================
-- Author:		Akansha Shrivastava
-- Create date: 2/10/2013 10:17:54 PM
-- Description:	To get recently listed used car
-- Modified By: Akansha on 10.4.2014
-- Description : Added Masking Name Column
-- =============================================
CREATE PROCEDURE [dbo].[Get_Used_Car_Buying_Guide] 
   @CategoryId  BigInt
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT TOP 2 CB.Id, 
			Title, 
			MainImageSet, 
			CMa.Name As MakeName, 
			CMo.Name As ModelName,
			CMo.MaskingName, 
			ECI.HostUrl + ECI.ImagePathThumbnail ThumbImgUrl
	FROM	Con_EditCms_Basic CB WITH (NOLOCK) 
			Inner Join Con_EditCms_Cars CC WITH (NOLOCK) ON CC.BasicId = CB.Id
			INNER JOIN Con_EditCms_Images ECI WITH (NOLOCK) ON ECI.BasicId = CB.Id 
			Left Join CarModels CMo  WITH (NOLOCK) ON CMo.ID = CC.ModelId
			Left Join CarMakes CMa  WITH (NOLOCK) ON CMa.ID = CMo.CarMakeId
	WHERE	CategoryId = @CategoryId And CB.IsPublished = 1 ORDER BY CB.Id DESC
END
