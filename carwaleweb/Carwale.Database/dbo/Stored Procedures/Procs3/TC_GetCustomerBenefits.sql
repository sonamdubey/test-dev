IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCustomerBenefits]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCustomerBenefits]
GO
	
-- =============================================    
-- Author:  Tejashree Patil    
-- Create date: 17 Sept 2012 on 2.30pm    
-- Description: Get details of Customer Benefits    
-- Exec TC_GetCustomerBenefits 3161   
-- =============================================    
CREATE PROCEDURE [dbo].[TC_GetCustomerBenefits]     
   @StockId BIGINT  
AS    
BEGIN    
	 -- SET NOCOUNT ON added to prevent extra result sets from    
	 -- interfering with SELECT statements.    
	 SET NOCOUNT ON;    
	    
		-- Insert statements for procedure here   
	 SELECT	CV.TC_CarValueAdditionsId AS BenefitId,ValueAddName,SCA.IsActive  
	 FROM	TC_CarValueAdditions CV WITH (NOLOCK)  
			LEFT JOIN TC_StockCarValueAdditions SCA WITH (NOLOCK) ON SCA.TC_CarValueAdditionsId=CV.TC_CarValueAdditionsId   
			AND SCA.TC_StockId=@StockId AND SCA.IsActive=1  
	 WHERE	CV.IsActive=1  
END 
