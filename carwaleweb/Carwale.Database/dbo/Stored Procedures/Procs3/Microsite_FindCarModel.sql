IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_FindCarModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_FindCarModel]
GO

	
    
    
-- =============================================      
-- Author:  Chetan Kane      
-- Create date: 2nd July 2012    
-- Description: To slect car Model for the select Model dropdown in FindCar widget, only the models available in the dealers stock will be fetch      
-- =============================================      
CREATE PROCEDURE [dbo].[Microsite_FindCarModel]    
(      
 @DealerId INT,
 @MakeID INT    
)      
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
     
	SELECT DISTINCT Model AS Text,ModelId AS Value 
	FROM vwMMV AS V 
	INNER JOIN TC_Stock AS TS ON V.VersionId = TS.VersionId 
	WHERE TS.BranchId = @DealerId AND V.MakeId = @MakeID    
END 
