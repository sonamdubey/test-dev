IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetForumLuceneDocument]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetForumLuceneDocument]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE PROCEDURE [cw].[GetForumLuceneDocument]      -- execute cw.GetForumLuceneDocument 770
 -- Add the parameters for the stored procedure here      
 @threadId NUMERIC(18,0)
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;

 SELECT FU.ID,FU.Topic,FU.StartDateTime,FU.URL,FU.LastPostId,FU.CustomerId,FU.Posts,FT.MsgDateTime,FU.Views,
STUFF(
(
SELECT ',' + FT.Message
FROM Forums F
INNER JOIN ForumThreads FT ON FT.ForumId=F.ID 
WHERE FT.IsActive=1 and FT.IsModerated=1 AND F.Id=FU.ID 
FOR XML PATH('')),1,1,'') AS Message
FROM Forums FU
INNER JOIN ForumThreads FT ON FT.ID = FU.LastPostId
WHERE FU.IsActive=1 and FU.IsModerated=1 and FU.ID=@threadId

END 
       


