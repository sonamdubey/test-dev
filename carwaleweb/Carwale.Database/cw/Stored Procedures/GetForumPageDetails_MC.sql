IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetForumPageDetails_MC]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetForumPageDetails_MC]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/05/2013>      
-- Description: <Returns the all the details required fot forum threads> 
-- =============================================      
CREATE  procedure [cw].[GetForumPageDetails_MC]      -- execute cw.GetForumPageDetails_MC  35995,1,20
 -- Add the parameters for the stored procedure here      
 @ThreadId NUMERIC(18,0),      
 @startindex INT ,       
 @endindex INT 
    
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;    
 -- Get the list of all posts for the provided threadid.
 


WITH CTE AS(
				/*SELECT FT.ID, FT.Message, FT.MsgDateTime, FT.LastUpdatedTime,
		IsNull(C.Name,'anonymous') AS PostedBy, IsNull(C.Id, 0) AS PostedById, 		
		Ct.Name AS City, 
		IsNull(UP.AvtarPhoto,'') As Avtar, 
		IsNull(UP.Signature,'') AS Signature ,
		CR.Role AS Role,
		CG.CustomerId,
		IsNull(FB.CustomerId,-1) AS BannedCust,
		IsNull(UP.HandleName,'anonymous') AS HandleName,
		IsNull(UP.ThanksReceived,0) AS ThanksReceived,
		IsNull(CONVERT(VARCHAR,UP.JoiningDate,106),0) AS JoiningDate,
		F.URL,		
		IsNull(UP2.HandleName,'anonymous') LastUpdatedHandle,
		ROW_NUMBER() OVER(ORDER BY FT.ID) AS Rno,
		--IsNull ((SELECT COUNT(FT.ID) OVER(Partition By FT.CustomerId)),0) AS Posts,
		IsNull( C1.Name,'N/A') AS UpdatedBy ,*/
		SELECT FT.ID, FT.Message, FT.MsgDateTime, FT.LastUpdatedTime,
               C.Name AS PostedBy,
               C.Id AS PostedById,                
               Ct.Name AS City,
               UP.AvtarPhoto As Avtar,
               UP.Signature AS Signature ,
               CR.Role AS Role,
               CG.CustomerId,
               FB.CustomerId AS BannedCust,
               UP.HandleName AS HandleName,
               UP.ThanksReceived AS ThanksReceived,
               UP.JoiningDate AS JoiningDate,
               F.URL,                
               UP2.HandleName AS  LastUpdatedHandle,
               ROW_NUMBER() OVER(ORDER BY FT.ID) AS Rno,
               C1.Name AS UpdatedBy,
		IsNull((SELECT COUNT(FT1.ID) FROM ForumThreads FT1 WITH (NOLOCK) ,Forums AS F1 WITH (NOLOCK)
		               WHERE  FT.CustomerId=FT1.CustomerId 
					      AND F1.ID=FT1.ForumId
					      AND FT1.IsActive = 1 
					      AND FT1.IsModerated = 1 
						  AND F1.IsActive=1),0) Posts
	 	 FROM ForumThreads AS FT WITH (NOLOCK)
		  JOIN Customers C WITH(NOLOCK) ON C.ID=FT.CustomerId
		  JOIN Forums F WITH(NOLOCK) ON F.ID = FT.ForumId	
		 LEFT JOIN Cities Ct WITH(NOLOCK) ON C.CityId=Ct.Id
		 LEFT JOIN UserProfile UP WITH(NOLOCK) ON UP.UserID =FT.CustomerId		
		 LEFT JOIN UserProfile UP2 WITH(NOLOCK) ON (UP2.UserId = FT.UpdatedBy AND FT.UpdatedBy IS NOT NULL)
		 LEFT JOIN ForumCustomerRoles FCR WITH(NOLOCK) ON FCR.CustomerId = FT.CustomerId
		 LEFT JOIN CarwaleRoles CR WITH(NOLOCK) ON CR.Id = FCR.RoleId
		 LEFT JOIN CarwaleGuys CG WITH(NOLOCK) ON CG.CustomerId = FT.CustomerId
		 LEFT JOIN Forum_BannedList AS FB WITH(NOLOCK) ON FB.CustomerId = C.ID
		 LEFT JOIN Customers C1 WITH(NOLOCK) ON (C1.Id = FT.UpdatedBy AND FT.UpdatedBy IS NOT NULL)
		  where FT.ForumId= @ThreadId--7106 -- 19966 -- 
		     AND FT.IsActive = 1 
	          AND FT.IsModerated = 1 
	
 )

--SELECT COUNT(ID) TotalPostCount FROM CTE

SELECT * FROM CTE WHERE Rno BETWEEN @startindex AND @endindex
ORDER BY MsgDateTime

 -- Get total number of posts for a thread.
SELECT Count(ID) as totalPostscount  FROM ForumThreads WITH(NOLOCK)  WHERE IsActive = 1 AND ForumId = @ThreadId AND IsModerated = 1
 
 
 --select * from ForumThreads where id=301175

				  
END 




