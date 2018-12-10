IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[ThirdPartyLeadInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[ThirdPartyLeadInsert]
GO

	-- ================================================        
-- Template generated from Template Explorer using:        
-- Create Procedure (New Menu).SQL        
--        
-- Use the Specify Values for Template Parameters         
-- command (Ctrl-Shift-M) to fill in the parameter         
-- values below.        
--        
-- This block of comments will not be included in        
-- the definition of the procedure.        
-- ================================================        
        
-- =============================================        
-- Author:  <Author,,Name>        
-- Create date: <Create Date,,>        
-- Description: <Description        
CREATE PROCEDURE [cw].[ThirdPartyLeadInsert]        
 -- Add the parameters for the stored procedure here   
   
   
 @MakeId  numeric(18),      
 @ModelId numeric(18),  
 @MakeName varchar(30),   
 @CampaignStartDate datetime,  
 @CampaignEndDate datetime,   
 @LeadVolume bigint,   
 @LeadsSent bigint,  
 @IsActive bit   
  
   
   
         
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.        
 SET NOCOUNT ON;        
        
      
    Insert into dbo.ThirdPartyLeadSettings(MakeId,ModelId,MakeName,CampaignStartDate,CampaignEndDate,LeadVolume,LeadsSent,IsActive)  
     values(@MakeId,@ModelId,@MakeName,@CampaignStartDate,@CampaignEndDate,@LeadVolume,@LeadsSent,@IsActive);        
          
END 