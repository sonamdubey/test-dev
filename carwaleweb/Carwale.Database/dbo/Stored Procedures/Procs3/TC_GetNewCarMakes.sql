IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetNewCarMakes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetNewCarMakes]
GO
	-- =============================================    
-- Author:  Tejashree Patil 
-- Create date: 4 March 2013    
-- Description: Return New Car Makes details    
-- =============================================    
CREATE PROCEDURE [dbo].[TC_GetNewCarMakes]
 -- Add the parameters for the stored procedure here    
 @BranchId BIGINT     
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
	SET NOCOUNT ON;  	 

		IF EXISTS(SELECT TOP 1 MakeId FROM TC_DealerMakes WHERE DealerId=@BranchId)
		BEGIN
			SELECT ID AS Value, Name AS Text     
			FROM CarMakes CM WITH (NOLOCK)
			INNER JOIN TC_DealerMakes DM WITH (NOLOCK) ON DM.MakeId= CM.ID AND DM.DealerId=@BranchId  
			WHERE IsDeleted = 0     
			AND Futuristic = 0    
			AND New = 1   
			ORDER BY Text    
		END
		ELSE
		BEGIN
			SELECT ID AS Value, Name AS Text     
			FROM CarMakes CM WITH (NOLOCK) 			
			WHERE IsDeleted = 0     
			AND Futuristic = 0    
			AND New = 1   
			ORDER BY Text  
		END      
    
END
