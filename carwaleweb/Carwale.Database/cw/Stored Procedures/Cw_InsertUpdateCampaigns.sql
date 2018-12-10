IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Cw_InsertUpdateCampaigns]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Cw_InsertUpdateCampaigns]
GO

	--Created By:Prashant Vishe         
--Description:For Inserting/Updating Sponsored Campaigns                
--Modified By:prashant Vishe On 21  May 2013        
--Modification:removed data insertion/updation for column IsDeleted  
--Modified By :Sachin Bharti on 29th Jan 2016
--Modification :  Insert new columns 
        
CREATE PROCEDURE [cw].[Cw_InsertUpdateCampaigns]
	@CampaignId NUMERIC,                
	@ModelId NUMERIC,              
	@AdScript VARCHAR(MAX),                
	@IsActive BIT,                
	@CampaignCategory TINYINT,  
	@StartDate DATETIME, 
	@EndDate DATETIME,     
	@SponsoredTitle VARCHAR(25) = NULL,    
	@IsSponsored BIT= NULL,      
	@PlatformId INT = 1,
	@UpdatedBy	INT = -1,
    @VPosition VARCHAR(10)= NULL,
	@HPosition VARCHAR(10)= NULL,
	@JumbotronPos SMALLINT= NULL,
	@ImageUrl VARCHAR(300)= NULL,    
	@BackGroundColor VARCHAR(10)= NULL,
    @IsUpdated	BIT = NULL OUTPUT ,
	@IsCampaignExist	BIT = NULL OUTPUT,
	@CategorySection SMALLINT = NULL
AS                
BEGIN                
SET NOCOUNT ON;       
      
	DECLARE @NewSponsoredTitle varchar(50);

	--Check whether an active campaign already exist for that timeline
	SELECT Id
	FROM SponsoredCampaigns SP(NOLOCK)
	WHERE SP.CampaignCategoryId = @CampaignCategory
		AND SP.PlatformId = @PlatformId
		AND SP.IsActive = 1
		AND SP.IsDeleted = 0
		AND SP.IsDefault IS NULL
		AND ( @StartDate BETWEEN  SP.StartDate AND SP.EndDate
			OR @EndDate BETWEEN SP.StartDate AND SP.EndDate)
		AND (@CampaignId IS NULL OR SP.Id <> @CampaignId)
		AND (@CategorySection IS NULL OR CategorySection = @CategorySection)

	IF @@ROWCOUNT > 0
		SET @IsCampaignExist = 1
	ELSE 
       SET @IsCampaignExist = 0

	IF (@SponsoredTitle IS NULL )AND (@CampaignCategory=1)      
	BEGIN      
		SET @NewSponsoredTitle='Featured Car'      
	END      
	ELSE if(@SponsoredTitle IS NOT NULL )AND (@CampaignCategory=1)      
	BEGIN      
		SET @NewSponsoredTitle=@SponsoredTitle      
	END       
	ELSE   
	BEGIN      
		SET @NewSponsoredTitle=Null      
	END      
       
	SET @IsUpdated = 0
	 
	IF @CampaignId IS NULL AND @IsCampaignExist = 0
		BEGIN                
		  INSERT INTO SponsoredCampaigns                
						(	ModelId, AdScript, IsActive, CampaignCategoryId, StartDate, EndDate, SponsoredTitle,
							IsSponsored, IsDefault, PlatformId, CreatedBy, VPosition, HPosition, JumbotronPos, ImageUrl , BackGroundColor,CategorySection)     
				VALUES
						(	@ModelId, @AdScript, @IsActive, @CampaignCategory, @StartDate, @EndDate, @NewSponsoredTitle,
							@IsSponsored, NULL, @PlatformId, @UpdatedBy, @VPosition, @HPosition, @JumbotronPos, @ImageUrl , @BackGroundColor,@CategorySection);      --removed data insertion for column IsDeleted          
			SET @CampaignId = SCOPE_IDENTITY()
			IF @CampaignId > 0 
				SET @IsUpdated = 1
		END           
	 ELSE IF @CampaignId IS NOT NULL AND @IsCampaignExist = 0
		 BEGIN                
			UPDATE SponsoredCampaigns 
			SET                
					AdScript=@AdScript,                
					IsActive=@IsActive,                
					CampaignCategoryId=@CampaignCategory ,             
					StartDate=@StartDate,            
					EndDate=@EndDate,      
					SponsoredTitle=@NewSponsoredTitle,    
					IsSponsored=@IsSponsored,
					PlatformId = @PlatformId,
					UpdatedBy = @UpdatedBy,
					UpdatedOn = GETDATE(),
					VPosition = @VPosition,
					HPosition = @HPosition,
					JumbotronPos = @JumbotronPos,
					ImageUrl = @ImageUrl,
					BackGroundColor = @BackGroundColor,
					CategorySection = @CategorySection,
					ModelId = @ModelId
			WHERE 
				Id = @CampaignId 
			IF @@ROWCOUNT > 0
				SET @IsUpdated = 1
		 END                 
	END              
        
/****** Object:  StoredProcedure [cw].[RetrieveSponsoredCampaigns]    Script Date: 05/21/2013 16:44:04 ******/        
SET ANSI_NULLS ON 

