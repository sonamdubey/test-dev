IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_UpdateConversionDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_UpdateConversionDetails]
GO

	-- =============================================  
-- Author:  Kritika Choudhary
-- Create date: 17th July 2015
-- Description: To Activate And DeActivate Dealer Conversion Details 
-- =============================================  
CREATE PROCEDURE [dbo].[Microsite_UpdateConversionDetails]  
(  
 @Id INT
)  
AS  
DECLARE @Activation BIT 
BEGIN   
	
	SET @Activation = (SELECT IsActive FROM Microsite_DealerConversionDetails WHERE ID = @Id)
	
	IF(@Activation = 0)
		BEGIN  
			UPDATE Microsite_DealerConversionDetails SET IsActive=1
			WHERE ID=@Id 
		END 
	ELSE
		BEGIN
			UPDATE Microsite_DealerConversionDetails SET IsActive=0
			WHERE ID=@Id 
		END 
END  