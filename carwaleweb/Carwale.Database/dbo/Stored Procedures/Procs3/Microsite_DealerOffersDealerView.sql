IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DealerOffersDealerView]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DealerOffersDealerView]
GO

	
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

-- =============================================
-- Author:		Chetan Kane
-- Create date: 13/3/2012
-- Description:	To View all all Dealer offers on dealer side
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_DealerOffersDealerView]
(  
 @DealerId INT 
 
) 	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id,OfferTitle,OfferDetails,OfferStartDate,OfferEndDate,OfferTermsCondition  FROM Microsite_DealerOffers
	WHERE DealerId=@DealerId AND IsActive=1 
END


