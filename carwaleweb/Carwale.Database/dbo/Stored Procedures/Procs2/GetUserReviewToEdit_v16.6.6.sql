IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUserReviewToEdit_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUserReviewToEdit_v16]
GO

	--THIS PROCEDURE IS FOR Edit customer reviews          
--CustomerReviews          
--CustomerId, MakeId, ModelId, VersionId, StyleR, ComfortR, PerformanceR, ValueR, FuelEconomyR, OverallR, Pros, Cons, Comments, Title,           
--EntryDateTime, IsNew, ReportAbused, Liked, Disliked 
--Modifier : Ajay Singh(on 15 feb 2016) 
--Description : Flipped the condition Related To Date        
--Modified By Rakesh Yadav on 30 June 2016, Fetch specific column instead of *          
CREATE PROCEDURE [dbo].[GetUserReviewToEdit_v16.6.6] --Execute [dbo].[GetUserReviewToEdit_v16.2.5] 45029,0           
 @Id   INT
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
FROM CustomerReviewsReplica AS CRR  WITH(NOLOCK)  where CRR.ReviewId = @Id
 
IF (@CRRDate IS NULL)
SELECT * FROM CustomerReviews  WITH(NOLOCK)  where ID = @Id 

ELSE IF ( @CRDate > @CRRDate)--Condition Reverted(flipped) By Ajay Singh on 11 feb 2016
SELECT * FROM CustomerReviews  WITH(NOLOCK)   where id = @Id
ELSE
SELECT CR.CustomerId,CR.IsVerified,CRR.Pros,CRR.Cons,CRR.Comments,CRR.Title,CRR.StyleR,CRR.ComfortR
,CRR.PerformanceR,CRR.ValueR,CRR.FuelEconomyR,CRR.OverallR,CR.VersionId,CR.ModelId
,CRR.Familiarity,CRR.Mileage,CRR.IsNewlyPurchased,CRR.IsOwned
FROM CustomerReviewsReplica CRR  WITH(NOLOCK)
inner join CustomerReviews CR  WITH(NOLOCK)   ON CR.ID = CRR.ReviewId AND CRR.ReviewId = @Id

END
ELSE
BEGIN
SELECT * FROM CustomerReviews   WITH(NOLOCK)  where id = @Id
END   
           
END 

