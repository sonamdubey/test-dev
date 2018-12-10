USE [CarWale]
GO
/****** Object:  StoredProcedure [dbo].[GetTopArticlesByBasicId_v16_10_1]    Script Date: 10/6/2016 5:27:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================      
-- Author:  <Ajay Singh>      
-- Create date: <6 Oct 2016>      
-- Description: Returns top articles-default top 10 articles are returned 
-- EXEC [dbo].[GetTopArticlesByBasicId_v16_10_1]
-- =============================================  
IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTopArticlesByBasicId_v16_10_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTopArticlesByBasicId_v16_10_1]
GO

CREATE PROCEDURE [dbo].[GetTopArticlesByBasicId_v16_10_1]
(
@BasicId INT,
@RecordCount INT = 10
)
AS
 BEGIN
       DECLARE @CategoryId INT
       SET @CategoryId = (SELECT categoryid FROM Con_EditCms_Basic WITH(NOLOCK) WHERE Id = @BasicId) 
       SELECT TOP (@RecordCount) CEB.Id AS BasicId,CEB.CategoryId,CEC.CategoryMaskingName,CEB.CategoryId AS ParentCatId
       FROM Con_EditCms_Basic CEB WITH(NOLOCK)
       INNER JOIN Con_EditCms_Category AS CEC WITH(NOLOCK) ON CEB.CategoryId = CEC.Id
       WHERE 
	   CEB.ApplicationID = 1
	   AND CEB.IsActive = 1
	   AND CEB.IsPublished = 1
	   AND CEB.DisplayDate < = GETDATE()
	   AND CEB.DisplayDate >=  DATEADD(month, -6, GETDATE())
	   AND CEB.Id <> @BasicId
	   AND CEB.CategoryId = @CategoryId
	   AND CEB.Id <> @BasicId
       ORDER BY CEB.DisplayDate DESC
   END