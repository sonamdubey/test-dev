IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetAreaDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetAreaDealers]
GO
	-- Author		:	Tejashree Patil.
-- Create date	:	8 Oct 2013.
-- Description	:	This SP used to get al area dealers.
-- =============================================    
CREATE PROCEDURE [dbo].[TC_GetAreaDealers] 
 -- Add the parameters for the stored procedure here    
 @UserId BIGINT
AS    
BEGIN    
	
	SELECT	DISTINCT ID AS Value , Organization AS Text
    FROM	Dealers 
	WHERE	TC_amId = @UserID
	AND IsDealerActive=1 
	ORDER BY ID DESC
		
END
