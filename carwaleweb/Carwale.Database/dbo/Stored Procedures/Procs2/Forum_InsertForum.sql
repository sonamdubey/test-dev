IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Forum_InsertForum]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Forum_InsertForum]
GO

	CREATE Procedure [dbo].[Forum_InsertForum]
-- Rohan 13-02-2015  Added IsModerated in insert query 
@ID NUMERIC,
@ForumSubCategoryId NUMERIC,
@Topic VARCHAR(200),
@CustomerId NUMERIC,
@StartDateTime datetime,
@TempID NUMERIC OUTPUT 

AS
	
BEGIN
	IF @ID = -1
		BEGIN
		    -- Rohan 13-02-2015  Added IsModerated in insert query 
			INSERT INTO Forums (ForumSubCategoryId,Topic,CustomerId,StartDateTime,IsModerated) 
			VALUES (@ForumSubCategoryId,@Topic,@CustomerId,@StartDateTime,1)
			SET @TempID = SCOPE_IDENTITY() 
		END
END





/****** Object:  StoredProcedure [cw].[GetDealerUsedCarProfile_15.2.1]    Script Date: 2/16/2015 5:16:10 PM ******/
-- SET ANSI_NULLS ON
