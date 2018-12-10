IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_ListingValuation_Save_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_ListingValuation_Save_SP]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: 14-JUN-2012
-- Description:	To save car valuation at the time of classified listing.
-- =============================================
CREATE PROCEDURE [dbo].[Classified_ListingValuation_Save_SP]
	-- Add the parameters for the stored procedure here
	@InquiryId BIGINT,
	@SellerType TINYINT,
	@FairValuation INT,
	@GoodValuation INT,
	@ExcellantValuation INT,
	@EntryDate DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @GoodValuation IS NOT NULL
	BEGIN
		INSERT INTO Classified_ListingValuation(InquiryId, SellerType, FairValuation, GoodValuation, ExcellantValuation, EntryDate)
		VALUES(@InquiryId, @SellerType, @FairValuation, @GoodValuation, @ExcellantValuation, @EntryDate)
	END
END
