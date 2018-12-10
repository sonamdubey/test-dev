IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DWModelColors]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DWModelColors]
GO

	
CREATE PROCEDURE [dbo].[DWModelColors]
@DealerId NUMERIC(18,0),
@ModelId NUMERIC(18,0)
AS
--Author: Rakesh Yadav on 09 jun 2015
--Desc: fetch colors and color images for dealer
--Modified By: Rakesh Yadav on 07 Aug 2015 added OriginalImgPath
BEGIN
	SELECT ColorName, HostUrl,ImgPath,ImgName,OriginalImgPath
	FROM 
	Microsite_DealerModelColors WITH(NOLOCk)
	WHERE 
	DealerId=@DealerId and DWModelId=@ModelId and IsActive=1
END