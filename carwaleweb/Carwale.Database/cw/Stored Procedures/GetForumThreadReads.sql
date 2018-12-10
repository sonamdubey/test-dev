IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetForumThreadReads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetForumThreadReads]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 

-- Modified By -- Ravi Koshal
--Description :  Added IsNull condition and Inner join made left join
-- =============================================      
CREATE PROCEDURE [cw].[GetForumThreadReads]      -- execute cw.Forums_GetAllThreadDetails 770
 -- Add the parameters for the stored procedure here      
 @threadIdList [dbo].[GetLuceneSearchData] READONLY
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;

SELECT FU.ID AS TopicId,
FU.Topic As Topic,
FU.StartDateTime AS StartDateTime,
FU.URL As URL,
FU.LastPostId,
FU.CustomerId As StartedById,
FU.Posts As Replies,
FU.Views As Reads,
FS.URL As ForumUrl,
IsNull (UP.HandleName,'anonymous') As HandleName, 
FT.MsgDateTime As LastPostTime, 
IsNull (UF.UserId,0) As LastPostedById , 
IsNull (UF.HandleName, 'anonymous') As PostHandleName , 
FS.Name As ForumCategory
FROM Forums AS FU  WITH (NOLOCK)   
LEFT JOIN ForumSubCategories FS WITH (NOLOCK)  ON FS.ID = FU.ForumSubCategoryId
LEFT JOIN UserProfile UP WITH (NOLOCK)  ON UP.UserId = FU.CustomerId
LEFT JOIN ForumThreads FT WITH (NOLOCK)  ON FT.ID = FU.LastPostId
LEFT JOIN UserProfile UF WITH (NOLOCK)  ON UF.UserId = FT.CustomerId
INNER JOIN @threadIdList UT  ON UT.ThreadID = FU.ID
WHERE FU.IsActive=1 and FU.IsModerated=1 

END 
       

