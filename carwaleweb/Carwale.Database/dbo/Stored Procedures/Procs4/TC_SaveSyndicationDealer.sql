IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveSyndicationDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveSyndicationDealer]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 10 Aug 2012
-- Description:	Get list of Syndication websites
-- Modified By: Tejashree Patil on 21 Sept 2012 at 7.20 pm
-- Description: Removed condition which check existance of @ToAdd websiteId in table TC_SyndicationDealer
-- exec [TC_SaveSyndicationDealer] ',',',',5
-- Modified   : Tejashree Patil on Date-16th Oct 2012 ,Changed PATINDEX to CHARINDEX
-- =============================================
CREATE PROCEDURE [dbo].[TC_SaveSyndicationDealer] 
	-- Add the parameters for the stored procedure here
	@ToAdd VARCHAR(50),
	@ToDelete VARCHAR(50),
	@BranchId INT
	--@Status INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Separator_position INT -- This is used to locate each separator character  
    DECLARE @array_value VARCHAR(1000)
	DECLARE @Separator CHAR(1)=','
		
	IF(@ToAdd IS NOT NULL)
	BEGIN
	 -- Loop through the string searching for separtor characters    
		WHILE CHARINDEX(@Separator, @ToAdd) <> 0   -- Modified   : Tejashree Patil on Date-16th Oct 2012 ,Changed PATINDEX to CHARINDEX
			BEGIN  			
				 -- patindex matches the a pattern against a string  
				SELECT  @Separator_position = CHARINDEX( @Separator ,@ToAdd) 
				SELECT  @array_value = LEFT(@ToAdd, @Separator_position - 1)
				-- Modified By: Tejashree Patil on 21 Sept 2012 at 7.20 pm
				INSERT INTO TC_SyndicationDealer (TC_SyndicationWebsiteId,BranchId,IsActive) VALUES(@array_value,@BranchId,1)
				SELECT  @ToAdd = STUFF(@ToAdd, 1, @Separator_position, '')				
			END
	END
	
	-- Loop through the string searching for separtor characters  
	IF(@ToDelete IS NOT NULL)
	BEGIN   
		WHILE CHARINDEX( @Separator, @ToDelete) <> 0   
			BEGIN  			
				 -- patindex matches the a pattern against a string  
				SELECT  @Separator_position = CHARINDEX(@Separator,@ToDelete)  
				SELECT  @array_value = LEFT(@ToDelete, @Separator_position - 1)
				UPDATE TC_SyndicationDealer SET IsActive=0 WHERE TC_SyndicationWebsiteId=@array_value
				SELECT  @ToDelete = STUFF(@ToDelete, 1, @Separator_position, '')
			END
	END
		
END




