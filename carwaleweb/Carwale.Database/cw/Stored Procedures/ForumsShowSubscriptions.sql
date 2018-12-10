IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[ForumsShowSubscriptions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[ForumsShowSubscriptions]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <01/08/2012>
-- Description:	<Returns the number of discussions,contributors and posts>
-- =============================================
CREATE procedure [cw].[ForumsShowSubscriptions] 
	-- Add the parameters for the stored procedure here
	@CustomerId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
SELECT FC.Url As forumUrl, 
F.Url AS Url,
F.ID AS TopicId,
ES.Name SubscriptionType,
F.Topic, 
IsNull(C.Name, 'anonymous') AS CustomerName,
(SELECT COUNT(ID) FROM ForumThreads WHERE ForumId = F.ID AND IsActive = 1) AS Replies,
IsNull(CP.Name, 'anonymous') AS LastPostBy, IsNull(CP.Id,'0') LastPostedById,
F.StartDateTime,
FC.Name ForumCategory, 
FC.Id ForumCategoryId,
FT.MsgDateTime AS LastPostTime,
IsNull(C.Id, '0') StartedById
FROM ((((((ForumSubscriptions FS LEFT JOIN Forums F ON FS.ForumThreadId=F.Id )
LEFT JOIN ForumThreads AS FT ON FT.ID = (SELECT MAX(ID)
FROM ForumThreads WHERE ForumId = F.ID AND IsActive = 1))
LEFT JOIN Customers AS CP ON CP.ID = FT.CustomerId)
LEFT JOIN Customers C ON C.ID = F.CustomerId )
LEFT JOIN ForumSubCategories FC ON FC.ID = F.ForumSubCategoryId )
LEFT JOIN EmailSubscriptions ES ON FS.EmailSubscriptionId = ES.Id )
WHERE FT.IsActive=1 AND FS.CustomerId = @CustomerId		
ORDER BY FT.MsgDateTime DESC
END




