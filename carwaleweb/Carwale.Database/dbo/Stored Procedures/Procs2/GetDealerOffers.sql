IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerOffers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerOffers]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 19-7-2012
-- Description:	Get Dealer offers
-- EXEC GetDealerOffers 2,18
-- =============================================
CREATE PROCEDURE [dbo].[GetDealerOffers] 
	-- Add the parameters for the stored procedure here
	@CityId Int , 
	@ModelId Int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @cnt Int
	
	Select 
		@cnt = COUNT(*)
	From 
		DealerOffers Do
		Inner Join DealerOffersMakeModel Domm On Domm.OfferId = Do.ID
	Where 
		CityId=@CityId
		And ModelId=@ModelId		 
	
	If (@cnt >0)	
		Select 
			ModelId, CityId, OfferTitle, OfferDescription, MaxOfferValue, Conditions, StartDate, EndDate, IsCountryWide 
		From DealerOffers Do
			Inner Join DealerOffersMakeModel Domm On Domm.OfferId = Do.ID
		Where 
			CityId=@CityId
			And ModelId=@ModelId	
			And Convert(Date,Do.StartDate) <= Convert(Date,GETDATE())
			And Convert(Date,Do.EndDate) >= Convert(Date,GETDATE())
			And Do.IsActive = 1
	Else
		Select 
			ModelId, CityId, OfferTitle, OfferDescription, MaxOfferValue, Conditions, StartDate, EndDate, IsCountryWide 
		From 
			DealerOffers Do
			Inner Join DealerOffersMakeModel Domm On Domm.OfferId = Do.ID
		Where  
			ModelId=@ModelId
			And IsCountryWide=1    
			And Convert(Date,Do.StartDate) <= Convert(Date,GETDATE())
			And Convert(Date,Do.EndDate) >= Convert(Date,GETDATE())
			And Do.IsActive = 1
END


