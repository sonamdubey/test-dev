IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[StoreEncryptionKeys]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[StoreEncryptionKeys]
GO

	


-- Author		:	Chetan Dev
-- Create date	:	01/03/2013 15:58:26 
-- Description	:	     
-- =============================================    
CREATE PROCEDURE [dbo].[StoreEncryptionKeys]  
 -- Add the parameters for the stored procedure here    
@RsaKey varchar(1000),
@UserName varchar(100),
@PassWord varchar(100),
@PublicKey varchar(1000)


AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
SET NOCOUNT ON;

		
	Declare @Key varchar(1000)

	Select @Key = Ek.EncryptionKey From   EncryptionKeys Ek Where Ek.PublicKey = @PublicKey 

	IF(@Key is null)
	begin
	Insert into EncryptionKeys (EncryptionKey,CreatedOn,PublicKey,UserName,PassWord)
	Values (@RsaKey , Getdate(),@PublicKey ,@UserName,@PassWord)
	end
	 	else
		begin
		
		UPDATE EncryptionKeys  SET EncryptionKey = @RsaKey , UserName = @UserName, PassWord = @PassWord
		where PublicKey = @PublicKey
		end
		 
END    




