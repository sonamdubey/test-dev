IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WA_NewsDetail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WA_NewsDetail]
GO
	

/*

Author: Rakesh Yadav	

Date Created: 01 july 2013

Desc: fetch details of new, previous news id and title and newxt news id and title
modified on 4-8-2014 by natesh Kumar for applicationid
modified by Manish on 12-08-2014 added with (nolock)
-- Modified by Manish on 01-09-2014 store subquery result in @DisplayDate variable and commented subquery

*/



CREATE PROCEDURE [dbo].[WA_NewsDetail]

@Id INT,
@ApplicationId INT


AS

BEGIN

DECLARE @DisplayDate  DATETIME



--Select Current news  data

SELECT  CB.Title, CB.DisplayDate, CB.AuthorName, CPC.Data, CB.MainImageSet,CEI.HostUrl,CEI.ImagePathThumbnail,CEI.ImagePathCustom,CEI.ImagePathLarge,CEI.Caption,CB.Url 

FROM Con_EditCms_Basic CB  WITH (NOLOCK)

LEFT JOIN Con_EditCms_Pages CP WITH (NOLOCK)  ON CP.BasicId = CB.Id 

LEFT JOIN Con_EditCms_PageContent CPC WITH (NOLOCK) ON CPC.PageId = CP.Id 

LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK) ON CEI.BasicId = CB.Id And CEI.IsMainImage = 1 And CEI.IsActive = 1 

WHERE CB.Id = @Id AND CP.IsActive = 1 AND CB.ApplicationID=@ApplicationId



--Select next news id and title
set @DisplayDate=(SELECT ceb.DisplayDate FROM Con_EditCms_Basic ceb WHERE ceb.Id = @Id AND ceb.ApplicationID=@ApplicationId)


SELECT TOP 1 ceb1.Id AS NextId,ceb1.Title AS Title,ceb1.AuthorName AS AuthorName,ceb1.DisplayDate,CEI.HostURL,CEI.ImagePathThumbnail,CEI.ImagePathLarge,CEI.ImagePathCustom  

FROM Con_EditCms_Basic ceb1 WITH (NOLOCK) LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK)  ON CEI.BasicId=ceb1.Id AND CEI.IsActive=1 AND CEI.IsMainImage=1 

WHERE  ceb1.IsActive = 1  AND ceb1.IsPublished = 1 AND ceb1.CategoryId IN (1,9) AND ceb1.ApplicationID=@ApplicationId  AND 

ceb1.DisplayDate < @DisplayDate --(SELECT ceb.DisplayDate FROM Con_EditCms_Basic ceb WHERE ceb.Id = @Id AND ceb.ApplicationID=@ApplicationId) 
ORDER BY  ceb1.DisplayDate DESC



--select previous news id and title



SELECT TOP 1 ceb1.Id AS PrevId,ceb1.Title AS Title,ceb1.AuthorName AS AuthorName,ceb1.DisplayDate,CEI.HostURL,CEI.ImagePathThumbnail,CEI.ImagePathLarge,CEI.ImagePathCustom  

FROM Con_EditCms_Basic ceb1 WITH (NOLOCK) LEFT JOIN Con_EditCms_Images CEI WITH (NOLOCK)  ON CEI.BasicId=ceb1.Id AND CEI.IsActive=1 AND CEI.IsMainImage=1 

WHERE  ceb1.IsActive = 1  AND ceb1.IsPublished = 1 AND ceb1.ApplicationID=@ApplicationId AND ceb1.CategoryId IN (1,9)  AND 

ceb1.DisplayDate >@DisplayDate -- (SELECT ceb.DisplayDate FROM Con_EditCms_Basic ceb WHERE ceb.Id = @Id AND ceb.ApplicationID=@ApplicationId) 
ORDER BY  ceb1.DisplayDate ASC 



END 


