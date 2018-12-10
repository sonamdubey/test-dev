IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDisscussionThreads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDisscussionThreads]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 30.08.2013
-- Description:	Get Top 3 Discussions from Forums with view count,post count and last post by 
-- Modified by: Akansha on 23.10.2013
-- Changes : Added no lock on tables
-- =============================================
CREATE PROCEDURE [dbo].[GetDisscussionThreads]
	@Top int=3
AS
BEGIN
	SELECT TOP (@Top) F.ID,IsNull(CT.Name, 'anonymous') Name, F.Topic,F.Views,F.StartDateTime as UpdatedTime,
	F.Posts AS Posts ,F.Url AS Url
	FROM Forums AS F WITH(NOLOCK)
	LEFT JOIN Customers AS CT WITH(NOLOCK) ON CT.ID = F.CustomerId 		
	WHERE F.IsActive = 1 AND CT.IsFake = 0 
	AND  F.IsModerated = 1 
	ORDER BY ID DESC
END
