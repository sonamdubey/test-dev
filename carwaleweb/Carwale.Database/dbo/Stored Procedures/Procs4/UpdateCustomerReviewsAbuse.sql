IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCustomerReviewsAbuse]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCustomerReviewsAbuse]
GO

	CREATE PROCEDURE [dbo].[UpdateCustomerReviewsAbuse]  
 @ReviewId  NUMERIC,
 @Comments VARCHAR(500),
 @ReportedBy NUMERIC  
 AS  
BEGIN  
   
 UPDATE CustomerReviews  
 SET   
  ReportAbused = 1  
 WHERE  
  ID = @ReviewId  
  
 INSERT INTO ReviewAbusedDetails
 (CustomerReviewId, Comments, ReportedBy, ReportedDateTime)
 VALUES
 (@ReviewId, @Comments, @ReportedBy, GETDATE()) 
     
END  
