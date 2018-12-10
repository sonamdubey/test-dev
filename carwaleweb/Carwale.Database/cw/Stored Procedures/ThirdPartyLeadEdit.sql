IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[ThirdPartyLeadEdit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[ThirdPartyLeadEdit]
GO

	
  
-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE [cw].[ThirdPartyLeadEdit]    
 -- Add the parameters for the stored procedure here    
   
    
 @Id int      
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
    
    -- Insert statements for procedure here    
select L.*,C.Name as ModelName from ThirdPartyLeadSettings L,CarModels C where L.ModelId=C.ID and ThirdPartyLeadSettingId=@Id; 

END    
