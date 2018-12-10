IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOfferDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetOfferDetails]
GO

	
-- =============================================
-- Author:		Ashish Verma
-- Create date: 26/11/2014
-- Description:	Fetch all offers
-- =============================================
CREATE PROCEDURE [dbo].[GetOfferDetails]
	-- Add the parameters for the stored procedure here
	
	@OfferId INT 
	,@Title VARCHAR(100) OUTPUT
	,@Description VARCHAR(1000) OUTPUT
	,@OfferType INT OUTPUT
	,@ExpiryDate Varchar(30) Output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT TOP 1 
		@Title = DO.OfferTitle
		,@Description = DO.OfferDescription
		,@OfferType = DO.OfferType
		,@ExpiryDate =  CONVERT(varchar, DO.EndDate, 106)
	FROM DealerOffers DO WITH (NOLOCK)
	
	WHERE DO.ID = @OfferId
		

END

