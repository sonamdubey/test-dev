IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DealerWebsite_SkodaWebsiteSetup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DealerWebsite_SkodaWebsiteSetup]
GO

	
-- Author :     Vikas Jyoti (22th August 2013)
-- Description : For configuring website of skoda dealers 
-- Modified by : vikas on 04-09-2013 changing passord value
CREATE PROCEDURE [dbo].[DealerWebsite_SkodaWebsiteSetup]
@DealerId	INT,
@SkodaDealerId INT
AS 
BEGIN
	
	IF NOT EXISTS(SELECT Top 1 * FROM TC_APIUsers WHERE DealerId=@DealerId )
BEGIN 
     DECLARE @UserId VARCHAR(20)       ---Below logic used for generate user id in the format yyyy:mm:dd hh:mm:ss.abc to yyyymmddhhmmssabc
    SET @UserId='TC'+replace(replace(CONVERT(VARCHAR(8), SYSDATETIME(), 112)+CONVERT(VARCHAR(12), SYSDATETIME(), 114),':',''),'.','')
	INSERT INTO TC_APIUsers ( DealerId,
							  UserId,
							  Password,
							  IsActive,
							  EntryDate )
					   SELECT  @DealerId,      
							   @UserId,
							   '@TC!' + CONVERT(VARCHAR,@DealerId) +'T' , --modified by vikas on 04-09-2013 changing passord value
							   1,
							   GETDATE() 
    PRINT  'DealerId'+ CONVERT(VARCHAR,@DealerId)+',' +'UserId:'+@UserId
    PRINT 'TC_APIUsers Inserted'
    
END 

                           
 IF NOT EXISTS(SELECT Top 1 * FROM DealerWebsite_NavigationLinks WHERE DealerId=@DealerId )
		BEGIN                             
			INSERT INTO [dbo].[DealerWebsite_NavigationLinks] 
												 ([ModelId], 
												 [NavigationText], 
												 [NavigationLink], 
												 [isActive], 
												 [entryDateTime], 
												 [DealerId], 
												 [NavigationOrder], 
												 [NavigationId]) 
										   SELECT [ModelId], 
												  [NavigationText], 
												  [NavigationLink], 
												  [isActive], 
												  GETDATE(), 
												  @DealerId, 
												  [NavigationOrder], 
												  [NavigationId]
										   FROM DealerWebsite_SkodaNavigationLinkMaster
	        PRINT 'DealerWebsite_NavigationLinks Inserted'
		END				   
							   
		                          
	IF NOT EXISTS(SELECT Top 1 * FROM [Microsite_DealerContentSubCategories] WHERE DealerId=@DealerId )
  BEGIN  					   
							   
	INSERT INTO [dbo].[Microsite_DealerContentSubCategories] 
														  ([SubCatagoryId], 
														   [CategoryId], 
														   [SubCatagoryName], 
														   [IsActive], 
														   [UrlValue], 
														   [DealerId], 
														   [SubCategoryOrder], 
														   [NavigationId]) 
													SELECT 
														  [SubCatagoryId], 
														   [CategoryId], 
														   [SubCatagoryName], 
														   [IsActive], 
														   [UrlValue], 
														   @DealerId, 
														   [SubCategoryOrder], 
														   [NavigationId]
													 FROM  DealerWebsite_SkodaSubCategoryMaster
			
			PRINT 'Microsite_DealerContentSubCategories Inserted'
												 
 	END 											 
		
    	IF NOT EXISTS(SELECT Top 1 * FROM SkodaTCDealerMap WHERE TCDealerId=@DealerId )
    BEGIN 
												 
			INSERT INTO SkodaTCDealerMap 
										(TCDealerId,
										 SkodaDealerId,
										 DealerName) 
									 SELECT @DealerId,
											Id,
											DealerName 
									 FROM  FB_SkodaDealers WHERE ID=@SkodaDealerId
                
                PRINT 'SkodaTCDealerMap Inserted'
		                             
      END                             
		                             
END		                             