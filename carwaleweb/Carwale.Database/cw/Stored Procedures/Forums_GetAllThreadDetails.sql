IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_GetAllThreadDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_GetAllThreadDetails]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- Modified by Ravi on 31-01-2014 added (WITH NOLOCK) KEYWORD
-- =============================================      
CREATE PROCEDURE [cw].[Forums_GetAllThreadDetails]      -- execute cw.Forums_GetAllThreadDetails 770
 -- Add the parameters for the stored procedure here      
@ThreadId NUMERIC(18,0),
@ID NUMERIC(18,0) OUTPUT,
@Name VARCHAR(200) OUTPUT,
@Description VARCHAR(200) OUTPUT,
@Topic VARCHAR(200) OUTPUT,
@ReplyStatus BIT OUTPUT,
@Url VARCHAR(400) OUTPUT,
@ForumUrl VARCHAR(400) OUTPUT,
@StartedOn datetime OUTPUT,
@StartedByEmail VARCHAR(400) OUTPUT,
@StartedByName VARCHAR(400) OUTPUT,
@IsStarterFake bit OUTPUT


 
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;
 
UPDATE Forums SET Views = IsNull(Views, 0) + 1 WHERE ID = @ThreadId
 
SELECT @ID=FC.ID, @Name=FC.Name, @Description=FC.Description, @Topic=F.Topic, @ReplyStatus=F.ReplyStatus, @Url=F.Url , @ForumUrl=FC.Url ,@IsStarterFake=IsNull(C.IsFake,0),
@StartedOn=F.StartDateTime, @StartedByEmail=IsNull(C.email,'') ,@StartedByName=IsNull(C.Name,'') 
FROM ForumSubCategories AS FC WITH (NOLOCK),
Forums AS F 
WITH (NOLOCK)
LEFT JOIN Customers   AS C WITH (NOLOCK)  ON C.ID = F.CustomerId 
WHERE F.ID = @ThreadId AND F.IsActive = 1 AND
FC.ID = F.ForumSubCategoryId AND FC.IsActive = 1 
 
END 
       




