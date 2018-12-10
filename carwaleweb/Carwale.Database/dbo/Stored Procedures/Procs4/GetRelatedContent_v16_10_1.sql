USE [CarWale]
GO
/****** Object:  StoredProcedure [dbo].[GetRelatedContent_v16_10_1]    Script Date: 9/15/2016 11:00:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================      
-- Author:  <Ajay Singh>      
-- Create date: <28 Sep 2016>      
-- Description: Returns the list of all related articles
-- EXEC [dbo].[GetRelatedContent_v16_10_1]
-- =============================================  
IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetRelatedContent_v16_10_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetRelatedContent_v16_10_1]
GO

CREATE PROCEDURE [dbo].[GetRelatedContent_v16_10_1]
(
@BasicId INT,
@RecordCount INT = 10
)
AS
BEGIN 

 SELECT DISTINCT Top (@RecordCount)
		        CB.Id AS BasicId,CB.CategoryId,CEC.CategoryMaskingName,(SELECT CategoryId FROM Con_EditCms_Basic WITH(NOLOCK) WHERE Id = @BasicId) AS ParentCatId,CB.DisplayDate	
		  FROM   
				Con_EditCms_Basic CB  WITH(NOLOCK)  
				INNER JOIN Con_EditCms_Category CEC WITH(NOLOCK) ON  CEC.Id = CB.CategoryId      
                INNER JOIN  Con_EditCms_BasicTags CEB WITH(NOLOCK)  ON CB.Id = CEB.BasicId 
				INNER JOIN Con_EditCms_Tags CET WITH(NOLOCK) ON CET.Id = CEB.TagId AND CET.Tag IN (SELECT 
				                       ET.Tag 
				                FROM
						          Con_EditCms_BasicTags AS CT WITH(NOLOCK)
                                  INNER JOIN Con_EditCms_Tags AS ET WITH(NOLOCK) ON ET.Id = CT.TagId AND CT.BasicId = @BasicId) 
		  WHERE
		        CB.ApplicationID = 1
			    AND CB.IsActive = 1
				AND CB.IsPublished = 1
				AND CB.DisplayDate < = GETDATE()
				--AND CB.DisplayDate >=  DATEADD(month, -6, GETDATE())
			    AND CB.Id <> @BasicId													
				AND (
					CB.IsSticky IS NULL
					OR CB.IsSticky = 0
					OR CAST(CB.StickyToDate AS DATE) < CAST(GETDATE() AS DATE)
					)
	     ORDER BY CB.DisplayDate DESC
END