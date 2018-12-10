IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[ThirdPartyLeadUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[ThirdPartyLeadUpdate]
GO

	    
-- =============================================      
-- Author:  <Author,,Name>      
-- Create date: <Create Date,,>      
-- Description: <Description,,>      
-- =============================================      
CREATE PROCEDURE [cw].[ThirdPartyLeadUpdate]      
 -- Add the parameters for the stored procedure here      
     
 @id int,  
 @MakeId  numeric(18),      
 @ModelId numeric(18),  
 @MakeName varchar(30),   
 @CampaignStartDate datetime,  
 @CampaignEndDate datetime,   
 @LeadVolume bigint,   

 @IsActive bit=1   
         
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
      
    -- Insert statements for procedure here      
 UPDATE dbo.ThirdPartyLeadSettings    
 SET  MakeId=@MakeId,ModelId= @ModelId , MakeName=@MakeName ,CampaignStartDate= @CampaignStartDate, CampaignEndDate=@CampaignEndDate,LeadVolume=@LeadVolume ,IsActive= @IsActive      
 WHERE ThirdPartyLeadSettingId=@Id;  
   
End