IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetArticleViewsCount_v16_9_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetArticleViewsCount_v16_9_1]
GO

	
-- =============================================      
-- Author:  <Ajay Singh>      
-- Create date: <02 Sep 2016>      
-- Description: Returns the views count of a article by basicid
-- EXEC [dbo].[GetArticleViewsCount_v16_9_1] 2110
-- =============================================  
CREATE PROCEDURE [dbo].[GetArticleViewsCount_v16_9_1]
(
@BasicId INT
)
AS
BEGIN

     SET NOCOUNT ON;

          SELECT Views 
          FROM Con_EditCms_Basic WITH (NOLOCK)
          WHERE Id = @BasicId
END
