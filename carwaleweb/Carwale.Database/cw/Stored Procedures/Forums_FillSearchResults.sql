IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_FillSearchResults]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_FillSearchResults]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[Forums_FillSearchResults]      -- execute cw.Forums_GetAllThreadDetails 770
 -- Add the parameters for the stored procedure here      
 @SearchId NUMERIC(18,0)
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
SET NOCOUNT ON;
SELECT Count(ForumThreadId) as tot FROM ForumSearchResults WHERE SearchId = @SearchId 
END 
