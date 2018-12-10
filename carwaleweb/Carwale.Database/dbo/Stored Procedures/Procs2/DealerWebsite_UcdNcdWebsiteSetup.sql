IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DealerWebsite_UcdNcdWebsiteSetup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DealerWebsite_UcdNcdWebsiteSetup]
GO

	
-- Author :     Vikas Jyoti (19th Nov 2013)
-- Description : For configuring website of skoda dealers 
CREATE PROCEDURE [dbo].[DealerWebsite_UcdNcdWebsiteSetup]
@DealerId	INT
AS 
BEGIN
	
	IF NOT EXISTS(SELECT Top 1 * FROM TC_APIUsers WHERE DealerId=@DealerId )
BEGIN 
     DECLARE @UserId VARCHAR(20)       ---Below logic used for generate user id in the format yyyy:mm:dd hh:mm:ss.abc to yyyymmddhhmmssabc
	 DECLARE @Password VARCHAR(20)       
    SET @UserId='TC'+replace(replace(CONVERT(VARCHAR(8), SYSDATETIME(), 112)+CONVERT(VARCHAR(12), SYSDATETIME(), 114),':',''),'.','')
	SET @Password='@TC!' + CONVERT(VARCHAR,@DealerId) +'T';
	INSERT INTO TC_APIUsers ( DealerId,
							  UserId,
							  Password,
							  IsActive,
							  EntryDate )
					   SELECT  @DealerId,      
							   @UserId,
							   @Password , 
							   1,
							   GETDATE() ;
						
	PRINT  '-----------';						   
    PRINT  '<add key="DealerId" value="'+ CONVERT(VARCHAR,@DealerId)+'" />';
	PRINT  '<add key="UserId" value="'+ @UserId +'" />';
	PRINT  '<add key="Password" value="'+ @Password +'" />';
	PRINT  '-----------';
    PRINT 'TC_APIUsers Data Inserted'
    
END 
    ELSE
		PRINT 'DealerId Already Present';

                           
END		                             