IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Cw_InsertUpdateCampaigns_v16_8_4]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Cw_InsertUpdateCampaigns_v16_8_4]
GO

	

--*****************************************
--Created By:Prashant Vishe         
--Description:For Inserting/Updating Sponsored Campaigns                
--Modified By:prashant Vishe On 21  May 2013        
--Modification:removed data insertion/updation for column IsDeleted  
--Modified By :Sachin Bharti on 29th Jan 2016
--Modification :  Insert new columns 
-- Modified By : Rakesh Yadav on 8 Aug 2016,added input paramters SubTitle,CardHeader,CardLink        
-- Modified By: Rakesh Yadav on 13 Oct 2016,save Url order inserting sponsored links, and check for isDefault=0 while cheking existing campaign
CREATE PROCEDURE [cw].[Cw_InsertUpdateCampaigns_v16_8_4]
	@CampaignId NUMERIC,                
	@ModelId NUMERIC,              
	@AdScript VARCHAR(MAX),                
	@IsActive BIT,                
	@CampaignCategory TINYINT,  
	@StartDate DATETIME, 
	@EndDate DATETIME,     
	@SponsoredTitle VARCHAR(200) = NULL,    
	@IsSponsored BIT= NULL,      
	@PlatformId INT = 1,
	@UpdatedBy	INT = -1,
    @VPosition VARCHAR(10)= NULL,
	@HPosition VARCHAR(10)= NULL,
	@JumbotronPos SMALLINT= NULL,
	@ImageUrl VARCHAR(300)= NULL,    
	@BackGroundColor VARCHAR(10)= NULL,
	@CategorySection SMALLINT = NULL,
	@SubTitle VARCHAR(200) = NULL,
	@CardHeader VARCHAR(200) = NULL,

	@CardLink VARCHAR(350) = NULL,
	@CarUrlOrder INT=1,
	@CardUrlId INT = null,

	@ButtonText VARCHAR(25) = NULL,
	@ButtonLink VARCHAR(350) = NULL,
	@UrlOrder VARCHAR(200) = NULL,
	@ButtonLinkId int = null,

	@ButtonText2 VARCHAR(25) = NULL,
	@ButtonLink2 VARCHAR(350) = NULL,
	@UrlOrder2 VARCHAR(200) = NULL,
	@ButtonLinkId2 int = null,

	@CardPosition VARCHAR(200)=null

AS                
BEGIN                
SET NOCOUNT ON;       
      
	DECLARE @NewSponsoredTitle varchar(50),
	@IsUpdated	BIT = NULL  ,
	@IsCampaignExist	BIT = 0 

	--Check whether an active campaign already exist for that timeline
	SELECT Id
	FROM SponsoredCampaigns SP(NOLOCK)
	WHERE SP.CampaignCategoryId = @CampaignCategory
		AND SP.PlatformId = @PlatformId
		AND SP.IsActive = 1
		AND SP.IsDeleted = 0
		AND (SP.IsDefault IS NULL OR SP.IsDefault = 0)
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
	ELSE   
	BEGIN      
		SET @NewSponsoredTitle=@SponsoredTitle   
	END       
       
	SET @IsUpdated = 0
	 
	IF @CampaignId IS NULL AND @IsCampaignExist = 0
		BEGIN                
		  INSERT INTO SponsoredCampaigns                
						(	ModelId, AdScript, IsActive, CampaignCategoryId, StartDate, EndDate, SponsoredTitle,
							IsSponsored, IsDefault, PlatformId, CreatedBy, VPosition, HPosition, JumbotronPos, ImageUrl , BackGroundColor,CategorySection
							,Subtitle,CardHeader,POSITION
							)     
				VALUES
						(	@ModelId, @AdScript, @IsActive, @CampaignCategory, @StartDate, @EndDate, @NewSponsoredTitle,
							@IsSponsored, NULL, @PlatformId, @UpdatedBy, @VPosition, @HPosition, @JumbotronPos, @ImageUrl , @BackGroundColor,@CategorySection
							,@SubTitle,@CardHeader,@CardPosition);      --removed data insertion for column IsDeleted          
			SET @CampaignId = SCOPE_IDENTITY()
			IF @CampaignId > 0
			BEGIN
				SET @IsUpdated = 1
				IF  @CampaignCategory = 8 -- app monetization
				BEGIN
					INSERT INTO SponsoredLinks (CampaignId,Name,IsInsideApp,Url,UrlOrder)
					VALUES (@CampaignId,'Card',CASE WHEN (@CardLink IS NULL OR @CardLink='') THEN 1 ELSE 0 END,ISNULL(@CardLink,''),@CarUrlOrder),
						   (@CampaignId,@ButtonText,CASE WHEN (@ButtonLink IS NULL OR @ButtonLink = '')THEN 1 ELSE 0 END,ISNULL(@ButtonLink,''),@UrlOrder)

					IF @CategorySection = 5
					BEGIN
						INSERT INTO SponsoredLinks (CampaignId,Name,IsInsideApp,Url,UrlOrder)
						VALUES (@CampaignId,@ButtonText2,CASE WHEN (@ButtonLink2 IS NULL OR @ButtonLink2 = '')THEN 1 ELSE 0 END,ISNULL(@ButtonLink2,''),@UrlOrder2)
					END

				END

			END
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
					--IsSponsored=@IsSponsored,
					PlatformId = @PlatformId,
					UpdatedBy = @UpdatedBy,
					UpdatedOn = GETDATE(),
					VPosition = @VPosition,
					HPosition = @HPosition,
					JumbotronPos = @JumbotronPos,
					ImageUrl = @ImageUrl,
					BackGroundColor = @BackGroundColor,
					CategorySection = @CategorySection,
					ModelId = @ModelId,
					Subtitle = @SubTitle,
					CardHeader = @CardHeader,
					POSITION=@CardPosition

			WHERE 
				Id = @CampaignId 
			IF @@ROWCOUNT > 0
				SET @IsUpdated = 1

			UPDATE SponsoredLinks
			SET 
			IsInsideApp = CASE WHEN (@CardLink IS NULL OR @CardLink='') THEN 1 ELSE 0 END,
			Url= ISNULL(@CardLink,''),
			UrlOrder=@CarUrlOrder
			WHERE 
			Id=@CardUrlId AND CampaignId = @CampaignId
			AND @CardUrlId IS NOT NULL

			UPDATE SponsoredLinks
			SET 
			Name=@ButtonText,
			IsInsideApp = CASE WHEN (@ButtonLink IS NULL OR @ButtonLink = '') THEN 1 ELSE 0 END,
			Url= ISNULL(@ButtonLink,''),
			UrlOrder=@UrlOrder
			WHERE 
			Id = @ButtonLinkId AND
			CampaignId = @CampaignId
			AND @CardUrlId IS NOT NULL

			UPDATE SponsoredLinks
			SET 
			Name=@ButtonText2,
			IsInsideApp = CASE WHEN (@ButtonLink2 IS NULL OR @ButtonLink2 = '') THEN 1 ELSE 0 END,
			Url= ISNULL(@ButtonLink2,''),
			UrlOrder=@UrlOrder2
			WHERE 
			Id = @ButtonLinkId2 AND
			CampaignId = @CampaignId
			AND @CardUrlId IS NOT NULL

			
		 END     
		 select @CampaignId as CampaignId, @IsUpdated as IsUpdated ,@IsCampaignExist as IsCampaignExist        
	END              					   	

