IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_OfferActivation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_OfferActivation]
GO

	-- =============================================  
-- Author:  Chetan Kane
-- Create date: 27th July 2012
-- Description: To Activate And DeActivate DealerOffer for the DealerWebSite 
-- =============================================  
CREATE PROCEDURE [dbo].[Microsite_OfferActivation]   
(  
 @Id INT
)  
AS  
DECLARE @Activation BIT 
BEGIN   
	
	SET @Activation = (SELECT IsActive FROM Microsite_DealerOffers WHERE Id = @Id)
	
	IF(@Activation = 0)
		BEGIN  
			UPDATE Microsite_DealerOffers SET IsActive=1
			WHERE Id=@Id 
		END 
	ELSE
		BEGIN
			UPDATE Microsite_DealerOffers SET IsActive=0
			WHERE Id=@Id 
		END 
END 

