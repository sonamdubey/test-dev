IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetOfferTerms]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetOfferTerms]
GO

	

-- =============================================
-- Author:	Sangram Nandkhile
-- Create date: 30 Sep 2015
-- Description:	Proc to get Offe Terms and Condition
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetOfferTerms]
	-- Add the parameters for the stored procedure here
	@maskingName varchar(50) = NULL,
	@offerid int,
	@isExpired bit out
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @expiryDate as datetime

	-- If offerId == Zero then consider masking Name
	IF  @offerid = 0
	BEGIN
			select top 1 O.Terms
			from bw_offers O WITH(NOLOCK)
			where O.maskingName = @maskingName
			and O.isactive = 1;

			set @expiryDate = (select top 1 BW_Offers.ExpiryDate
			from bw_offers WITH(NOLOCK)
			where BW_Offers.maskingName = @maskingName
			and BW_Offers.isactive = 1)
	END
ELSE  -- ELSE use masking Name
	BEGIN
		select top 1 O.Terms
			from bw_offers O WITH(NOLOCK)
			where O.id = @offerid
			and O.isactive = 1;

			set @expiryDate = (select top 1 BW_Offers.ExpiryDate
			from bw_offers WITH(NOLOCK)
			where BW_Offers.id = @offerid
			and BW_Offers.isactive = 1)
	END

    -- If the offer has been expired
	if @expiryDate < GETDATE()
		set @isExpired = 1;
	else 
		set @isExpired = 0;

END

