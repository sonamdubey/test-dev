IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GoogleSiteMapDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GoogleSiteMapDetails]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <30/07/2013>      
-- Description: <Returns last 2 days news for Google SiteMap> 
-- Modified On : 7th Feb by Ravi  Added Auto Expo News Category - 9 
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- =============================================      
CREATE PROCEDURE [cw].[GoogleSiteMapDetails]  -- execute   [cw].[GoogleSiteMapDetails]   1
 -- Add the parameters for the stored procedure here      
 @ApplicationId INT
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
 SELECT DISTINCT CB.Id AS BasicId
,CB.AuthorName
,CB.Description
,CB.DisplayDate
,CB.Title
,CB.Url
,CEI.HostUrl
,CEI.ImagePathLarge
,CEI.OriginalImgPath
,dbo.GetGoogleTags (CB.Id)AS Tag
,CEI.Caption
,ROW_NUMBER() OVER (
ORDER BY DisplayDate DESC
) AS Row_No
,CB.IsSticky
FROM 
Con_EditCms_Basic AS CB WITH(NOLOCK)
LEFT JOIN Con_EditCms_Images CEI WITH(NOLOCK) ON CEI.BasicId = CB.Id
AND CEI.IsMainImage = 1
AND CEI.IsActive = 1
WHERE CB.CategoryId IN ( 1,9 )
AND CB.IsActive = 1
AND CB.ApplicationID = @ApplicationId
AND CB.IsPublished = 1
AND CB.DisplayDate>=GETDATE()-2
ORDER BY CB.DisplayDate DESC
 END




