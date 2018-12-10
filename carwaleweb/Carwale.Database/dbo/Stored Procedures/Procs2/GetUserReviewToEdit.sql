IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUserReviewToEdit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUserReviewToEdit]
GO

	--THIS PROCEDURE IS FOR Edit customer reviews          
--CustomerReviews          
--CustomerId, MakeId, ModelId, VersionId, StyleR, ComfortR, PerformanceR, ValueR, FuelEconomyR, OverallR, Pros, Cons, Comments, Title,           
--EntryDateTime, IsNew, ReportAbused, Liked, Disliked          
          
CREATE PROCEDURE [dbo].[GetUserReviewToEdit] --Execute [dbo].[GetUserReviewToEdit] 43248 , 1           
 @Id   BIGINT
,@IsModerator BIT
         
AS            
BEGIN

DECLARE @CRDate DATETIME
DECLARE @CRRDate DATETIME

IF @IsModerator <> 0
BEGIN

SELECT @CRDate = CR.LastUpdatedOn 
FROM CustomerReviews AS CR  WITH(NOLOCK)
WHERE CR.ID = @Id

SELECT @CRRDate = CRR.LastUpdatedOn 
FROM CustomerReviewsReplica AS CRR where CRR.ReviewId = @Id
 
IF (@CRRDate IS NULL)
SELECT * FROM CustomerReviews where ID = @Id 

ELSE IF ( @CRDate < @CRRDate)
SELECT * FROM CustomerReviews where id = @Id
ELSE
SELECT * , CR.CustomerId 
FROM CustomerReviewsReplica CRR
inner join CustomerReviews CR WITH(NOLOCK) ON CR.ID = CRR.ReviewId AND CRR.ReviewId = @Id

END
ELSE
BEGIN
SELECT * FROM CustomerReviews where id = @Id
END   
           
END 

