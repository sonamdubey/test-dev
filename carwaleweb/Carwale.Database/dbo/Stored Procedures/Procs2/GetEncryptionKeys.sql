IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetEncryptionKeys]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetEncryptionKeys]
GO

	


-- Author		:	Chetan Dev
-- Create date	:	01/03/2013 15:58:26 
-- Description	:	     
-- =============================================    
CREATE PROCEDURE [dbo].[GetEncryptionKeys]  
 -- Add the parameters for the stored procedure here    
@PublicKey varchar(1000),
@UserName varchar(100) OUTPUT,
@PassWord varchar(100) OUTPUT,
@OutKey varchar(1000) OUTPUT

AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
SET NOCOUNT ON;

	Select @OutKey = Ek.EncryptionKey, @UserName = EK.UserName, @PassWord = Ek.PassWord From   EncryptionKeys Ek Where Ek.PublicKey = @PublicKey 
	
				 
END    




