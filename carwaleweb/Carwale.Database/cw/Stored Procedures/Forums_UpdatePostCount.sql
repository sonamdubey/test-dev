IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_UpdatePostCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_UpdatePostCount]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/17/2013>      
-- Description: <Delete a post and update post count for the customer accordingly in user profile table.> 
-- =============================================      
create procedure [cw].[Forums_UpdatePostCount]      -- execute cw.orums_DeletePost 1278,775
 -- Add the parameters for the stored procedure here      
 @UserId NUMERIC(18,0)
 
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;
with CTE
as
(SELECT UP.UserId as CTEUserId,COUNT(FT1.ID) as Cnt 
FROM ForumThreads FT1, Forums F,UserProfile UP 
WHERE F.Id=FT1.ForumId 
AND FT1.IsActive=1 AND F.IsActive=1 AND UP.UserId = FT1.CustomerId AND UP.UserId = @UserId
group by UP.UserId)

Update UserProfile
SET ForumPosts =Cnt
from UserProfile as UP 
Join CTE as C on C.CTEUserId=UP.UserId
END 
       
