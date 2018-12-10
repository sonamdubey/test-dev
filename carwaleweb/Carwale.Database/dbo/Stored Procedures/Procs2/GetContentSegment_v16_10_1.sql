IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetContentSegment_v16_10_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetContentSegment_v16_10_1]
GO

	-- Author:Jitendra
-- Created on:10.10.2016
-- Description: Get all the Category with masking name and id
-- Exec GetContentSegmentCount_v16_10_1 

CREATE PROCEDURE [dbo].[GetContentSegment_v16_10_1]
AS
BEGIN

SELECT Id as SubCategoryId, DisplayName as SubCategoryName FROM Con_EditCms_Category WITH(NOLOCK) WHERE IsActive = 1

END

