IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_GetForumForUserPostEdit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_GetForumForUserPostEdit]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[Forums_GetForumForUserPostEdit]      -- execute cw.Forums_GetAllThreadDetails 770
 -- Add the parameters for the stored procedure here      
@ThreadId NUMERIC(18,0),
@ID NUMERIC(18,0) OUTPUT,
@Name VARCHAR(200) OUTPUT,
@Topic VARCHAR(200) OUTPUT,
@Url VARCHAR(400) OUTPUT,
@ForumUrl VARCHAR(400) OUTPUT
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
SET NOCOUNT ON;
SELECT @ID=FC.ID, @Name=FC.Name,  @Topic=F.Topic,  @Url=F.Url , @ForumUrl=FC.Url
FROM Forums AS F 
LEFT JOIN Customers AS C ON C.ID = F.CustomerId
LEFT JOIN ForumSubCategories AS FC ON FC.Id = F.ForumSubCategoryId
WHERE F.ID = @ThreadId AND F.IsActive = 1 AND FC.IsActive = 1 
 
END 
       
