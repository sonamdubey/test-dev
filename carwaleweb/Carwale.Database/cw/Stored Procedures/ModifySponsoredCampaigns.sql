IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[ModifySponsoredCampaigns]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[ModifySponsoredCampaigns]
GO

	--Created By:Prashant Vishe         
--Description:For Inserting/Updating Sponsored Campaigns                
--Modified By:prashant Vishe On 21  May 2013        
--Modification:removed data insertion/updation for column IsDeleted        
        
CREATE PROCEDURE [cw].[ModifySponsoredCampaigns]                 
@Id numeric,                
@ModelId numeric,              
@AdScript varchar(max),                
@IsActive bit,                
@CampaignCategory tinyint,                
@isUpdate bit,             
@StartDate datetime,            
@EndDate datetime,      
@SponsoredTitle varchar(25),    
@IsSponsored bit,      
@IsDefault bit,
@PlatformId INT = 1,
@UpdatedBy	INT = -1   
                
                
AS                
BEGIN                
SET NOCOUNT ON;       
      
 DECLARE @NewSponsoredTitle varchar(50);      
       
 IF (@SponsoredTitle IS NULL )AND (@CampaignCategory=1)      
  BEGIN      
  set @NewSponsoredTitle='Featured Car'      
  END      
 ELSE if(@SponsoredTitle IS NOT NULL )AND (@CampaignCategory=1)      
    BEGIN      
    set @NewSponsoredTitle=@SponsoredTitle      
    END       
 else      
   begin      
     set @NewSponsoredTitle=Null      
   end      
   
          
       
          
 IF @isUpdate=0                
  BEGIN                
  INSERT INTO SponsoredCampaigns                
   (ModelId,AdScript,IsActive,CampaignCategoryId,StartDate,EndDate,SponsoredTitle,IsSponsored,IsDefault, PlatformId, CreatedBy)                 
  VALUES(@ModelId,@AdScript,@IsActive,@CampaignCategory,@StartDate,@EndDate,@NewSponsoredTitle,@IsSponsored,@IsDefault, @PlatformId, @UpdatedBy);      --removed data insertion for column IsDeleted          
  
  SET @Id = SCOPE_IDENTITY()
  END
                   
 ELSE                
     BEGIN                
     UPDATE SponsoredCampaigns SET                
     AdScript=@AdScript,                
     IsActive=@IsActive,                
     CampaignCategoryId=@CampaignCategory ,             
     StartDate=@StartDate,            
     EndDate=@EndDate,      
     SponsoredTitle=@NewSponsoredTitle,    
     IsSponsored=@IsSponsored, 
	 IsDefault=@IsDefault,
	 PlatformId = @PlatformId,
	 UpdatedBy = @UpdatedBy,
	 UpdatedOn = GETDATE()
     WHERE Id = @Id         --removed data updation for column IsDeleted         
     END 

  
 
	  	                
END        
        
        
/****** Object:  StoredProcedure [cw].[RetrieveSponsoredCampaigns]    Script Date: 05/21/2013 16:44:04 ******/        
SET ANSI_NULLS ON 
