IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forums_ShowPreviousPosts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forums_ShowPreviousPosts]
GO

	-- =============================================      
-- Author:  <Ravi Koshal>
-- Create date: <09/13/2013>      
-- Description: <Check for sticky threads.> 
-- =============================================      
CREATE procedure [cw].[Forums_ShowPreviousPosts]      -- execute cw.Forums_GetAllThreadDetails 770
 -- Add the parameters for the stored procedure here      
@ForumId NUMERIC(18,0)
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;
 
SELECT TOP 10 FT.ID, IsNull(U.HandleName,'anonymous') AS PostedBy, FT.Message, FT.MsgDateTime 
FROM ForumThreads AS FT LEFT JOIN UserProfile AS U ON U.UserId = FT.CustomerId
 WHERE FT.ForumId = @ForumId AND FT.IsActive = 1 AND FT.IsModerated = 1 
 ORDER BY FT.MsgDateTime DESC 
 
END 
       

