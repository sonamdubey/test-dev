IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_NewsDetail_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_NewsDetail_16]
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
-- Modified by: Rakesh Yadav on 2 June 2016, added categoryId and Content Page Name and removed joins of Con_EditCms_Images table
--======================================================================---
create PROCEDURE [dbo].[WA_NewsDetail_16.6.1] --[dbo].[WA_NewsDetail_16.6.1] 23326,1

@Id INT,
@ApplicationId INT


AS

BEGIN

DECLARE @DisplayDate  DATETIME



--Select Current news  data

SELECT  CB.Title,CB.CategoryId, CB.DisplayDate, CB.AuthorName,CP.PageName, CPC.Data, 
CB.MainImageSet,CB.HostUrl,--CEI.ImagePathThumbnail,
--CEI.ImagePathCustom,CEI.ImagePathLarge,CEI.Caption,
CB.Url,CB.MainImagePath AS OriginalImgPath,C.IsSinglePage
FROM Con_EditCms_Basic CB  WITH (NOLOCK)
LEFT JOIN Con_EditCms_Pages CP WITH (NOLOCK)  ON CP.BasicId = CB.Id 
LEFT JOIN Con_EditCms_PageContent CPC WITH (NOLOCK) ON CPC.PageId = CP.Id 
--LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id And CEI.IsMainImage = 1 And CEI.IsActive = 1 
INNER JOIN Con_EditCms_Category C WITH (NOLOCK) ON CB.CategoryId=C.Id
WHERE CB.Id = @Id AND CP.IsActive = 1 AND CB.ApplicationID=@ApplicationId
ORDER BY CP.Priority



--Select next news id and title
set @DisplayDate=( 
					SELECT ceb.DisplayDate 
					FROM Con_EditCms_Basic ceb WITH(NOLOCK) 
					WHERE ceb.Id = @Id AND ceb.ApplicationID=@ApplicationId
				 )


SELECT TOP 1 ceb1.Id AS NextId,ceb1.Title AS Title,ceb1.AuthorName AS AuthorName,
		ceb1.DisplayDate,ceb1.HostURL,--CEI.ImagePathThumbnail,CEI.ImagePathLarge,
			--CEI.ImagePathCustom,
			ceb1.MainImagePath AS OriginalImgPath 
FROM Con_EditCms_Basic ceb1 WITH (NOLOCK) 
--LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK)  ON CEI.BasicId=ceb1.Id 
	--											AND CEI.IsActive=1 AND CEI.IsMainImage=1 

WHERE  ceb1.IsActive = 1  AND ceb1.IsPublished = 1 
						AND ceb1.CategoryId IN (1,2,6,8,19)  --modified by ajay singh
						AND ceb1.ApplicationID=@ApplicationId  
						AND 
						ceb1.DisplayDate < @DisplayDate --(SELECT ceb.DisplayDate FROM Con_EditCms_Basic ceb WHERE ceb.Id = @Id AND ceb.ApplicationID=@ApplicationId) 
ORDER BY  ceb1.DisplayDate DESC



--select previous news id and title



SELECT TOP 1 ceb1.Id AS PrevId,ceb1.Title AS Title,ceb1.AuthorName AS AuthorName,ceb1.DisplayDate,
ceb1.HostURL,--CEI.ImagePathThumbnail,CEI.ImagePathLarge,CEI.ImagePathCustom,
ceb1.MainImagePath AS OriginalImgPath  

FROM Con_EditCms_Basic ceb1 WITH (NOLOCK) 
--LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK)  ON CEI.BasicId=ceb1.Id 
--													AND CEI.IsActive=1 AND CEI.IsMainImage=1 

WHERE  ceb1.IsActive = 1  AND ceb1.IsPublished = 1 
						 AND ceb1.ApplicationID=@ApplicationId 
						 AND ceb1.CategoryId IN (1,2,6,8,19)  --modified by ajay singh 
						  AND 

ceb1.DisplayDate > @DisplayDate -- (SELECT ceb.DisplayDate FROM Con_EditCms_Basic ceb WHERE ceb.Id = @Id AND ceb.ApplicationID=@ApplicationId)  
ORDER BY  ceb1.DisplayDate ASC 



END 

/****** Object:  StoredProcedure [dbo].[GetPqCityGroupsByModelId_v16_6_1]    Script Date: 6/6/2016 1:52:17 PM ******/
SET ANSI_NULLS ON
