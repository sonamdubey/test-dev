IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_RemoveBookmark]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_RemoveBookmark]
GO

	-- ==========================================================  
-- Author		: Vikas Choodikottammel 
-- Create date	: OCT 14, 2010  
-- Description	: Stored Procedure to Remove the listing of a 
--				  particular Bookmark for a user
-- ==========================================================  
Create PROCEDURE [dbo].[Classified_RemoveBookmark]  
 -- Add the parameters for the stored procedure here   
 @ProfileId		Varchar(50),  
 @CustomerId	Numeric  
As  
Begin    
	Update dbo.CustomerFavouritesUsed Set isActive = 0 Where CustomerId = @CustomerId And CarProfileId = @ProfileId
End  
  