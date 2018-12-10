IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UsedCarOfferDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UsedCarOfferDetails]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 28-12-2013
-- Description:	Load Used Car Offer Details
-- =============================================
CREATE PROCEDURE [dbo].[TC_UsedCarOfferDetails]
@BranchId INT,
@TC_UsedCarOfferId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
	SELECT Top 1
	    TC_UsedCarOfferId, 
		OfferName, 
		StartDate AS ValidFrom,
		EndDate AS ValidTo, 
		OfferAmount, 
		Description,
		Terms
	FROM TC_UsedCarOffers O WITH(NOLOCK)
	WHERE O.BranchId = @BranchId
	AND ((TC_UsedCarOfferId = @TC_UsedCarOfferId) OR @TC_UsedCarOfferId IS NULL)
	AND IsActive = 1
	ORDER BY O.CreatedDate

	SELECT TC_UsedCarOfferId, OfferName, StartDate, EndDate
	FROM TC_UsedCarOffers 
	WHERE BranchId = @BranchId
	AND IsActive = 1
	ORDER BY CreatedDate
END













/****** Object:  StoredProcedure [dbo].[TC_DeleteUsedCarOffer]    Script Date: 2/5/2014 7:23:00 PM ******/
SET ANSI_NULLS ON
