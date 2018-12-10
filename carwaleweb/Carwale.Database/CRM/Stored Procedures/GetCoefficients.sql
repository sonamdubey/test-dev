IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[GetCoefficients]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[GetCoefficients]
GO

	
-- Author		:	Chetan Dev
-- Create date	:	01/03/2013 15:58:26 
-- Description	:	fetch coefficients for lead score calculations
-- =============================================    
CREATE PROCEDURE [CRM].[GetCoefficients]  
 -- Add the parameters for the stored procedure here  
 @LeadScoreVersion smallint  
 

As
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
SET NOCOUNT ON;

SELECT CategoryId,Name, Value 
FROM CRM.LSCategory  WITH(NOLOCK)
WHERE LeadScoreVersion =@LeadScoreVersion


END    


