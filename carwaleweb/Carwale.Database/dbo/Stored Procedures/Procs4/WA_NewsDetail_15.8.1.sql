IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_NewsDetail_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_NewsDetail_15]
GO
	


--======================================================================---
--Author: Rakesh Yadav	

--Date Created: 01 july 2013

-- Desc: fetch details of new, previous news id and title and newxt news id and title
-- modified on 4-8-2014 by natesh Kumar for applicationid
-- modified by: Manish on 12-08-2014 added with (nolock)
-- Modified by: Manish on 01-09-2014 store subquery result in @DisplayDate variable and commented subquery
-- Modified by: Chetan on 24-07-2015 added column OriginalImgPath of Con_EditCms_Images table.
-- Modified by: Ajay Singh on 17-05-2016 added more category in list
--======================================================================---
 CREATE PROCEDURE [dbo].[WA_NewsDetail_15.8.1] --[dbo].[WA_NewsDetail_15.8.1] 3,1

@Id INT,
@ApplicationId INT


AS

BEGIN

DECLARE @DisplayDate  DATETIME



--Select Current news  data

SELECT  CB.Title, CB.DisplayDate, CB.AuthorName, CPC.Data, 
CB.MainImageSet,CEI.HostUrl,CEI.ImagePathThumbnail,
CEI.ImagePathCustom,CEI.ImagePathLarge,CEI.Caption,
CB.Url,CEI.OriginalImgPath 

FROM Con_EditCms_Basic CB  WITH (NOLOCK)

LEFT JOIN Con_EditCms_Pages CP WITH (NOLOCK)  ON CP.BasicId = CB.Id 

LEFT JOIN Con_EditCms_PageContent CPC WITH (NOLOCK) ON CPC.PageId = CP.Id 

LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id And CEI.IsMainImage = 1 And CEI.IsActive = 1 

WHERE CB.Id = @Id AND CP.IsActive = 1 AND CB.ApplicationID=@ApplicationId



--Select next news id and title
set @DisplayDate=( 
					SELECT ceb.DisplayDate 
					FROM Con_EditCms_Basic ceb WITH(NOLOCK) 
					WHERE ceb.Id = @Id AND ceb.ApplicationID=@ApplicationId
				 )


SELECT TOP 1 ceb1.Id AS NextId,ceb1.Title AS Title,ceb1.AuthorName AS AuthorName,
		ceb1.DisplayDate,CEI.HostURL,CEI.ImagePathThumbnail,CEI.ImagePathLarge,
			CEI.ImagePathCustom,CEI.OriginalImgPath 

FROM Con_EditCms_Basic ceb1 WITH (NOLOCK) 
LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK)  ON CEI.BasicId=ceb1.Id 
												AND CEI.IsActive=1 AND CEI.IsMainImage=1 

WHERE  ceb1.IsActive = 1  AND ceb1.IsPublished = 1 
						AND ceb1.CategoryId IN (1,2,6,8,19)  --modified by ajay singh
						AND ceb1.ApplicationID=@ApplicationId  
						AND 
						ceb1.DisplayDate < @DisplayDate --(SELECT ceb.DisplayDate FROM Con_EditCms_Basic ceb WHERE ceb.Id = @Id AND ceb.ApplicationID=@ApplicationId) 
ORDER BY  ceb1.DisplayDate DESC



--select previous news id and title



SELECT TOP 1 ceb1.Id AS PrevId,ceb1.Title AS Title,ceb1.AuthorName AS AuthorName,ceb1.DisplayDate,CEI.HostURL,CEI.ImagePathThumbnail,CEI.ImagePathLarge,CEI.ImagePathCustom,CEI.OriginalImgPath  

FROM Con_EditCms_Basic ceb1 WITH (NOLOCK) 
LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK)  ON CEI.BasicId=ceb1.Id 
													AND CEI.IsActive=1 AND CEI.IsMainImage=1 

WHERE  ceb1.IsActive = 1  AND ceb1.IsPublished = 1 
						 AND ceb1.ApplicationID=@ApplicationId 
						 AND ceb1.CategoryId IN (1,2,6,8,19)  --modified by ajay singh 
						  AND 

ceb1.DisplayDate >@DisplayDate -- (SELECT ceb.DisplayDate FROM Con_EditCms_Basic ceb WHERE ceb.Id = @Id AND ceb.ApplicationID=@ApplicationId)  
ORDER BY  ceb1.DisplayDate ASC 



END 

