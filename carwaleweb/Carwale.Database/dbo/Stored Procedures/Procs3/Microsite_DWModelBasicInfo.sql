IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DWModelBasicInfo]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DWModelBasicInfo]
GO

	
CREATE PROCEDURE [dbo].[Microsite_DWModelBasicInfo]
@DealerId INT,
@DWModelId INT
AS 
--Author:Rakesh Yadav On 29 Jun 2015
--Desc: Details of dealer model
--Modified by Rakesh Yadav on 05 Aug 2015, fetch OriginalImgPath
BEGIN
	SELECT Id AS DWModelId,CWModelId,DWModelName,HostUrl,ImgPath,ImgName,DWBodyStyleId,OriginalImgPath
	FROM TC_DealerModels WITH (NOLOCK) WHERE DealerId=@DealerId AND ID=@DWModelId
END