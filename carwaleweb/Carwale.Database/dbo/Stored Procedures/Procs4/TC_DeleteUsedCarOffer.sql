IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DeleteUsedCarOffer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DeleteUsedCarOffer]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 14-01-2014
-- Description:	Deletes Used Car Offer
-- =============================================
CREATE PROCEDURE [dbo].[TC_DeleteUsedCarOffer]
@BranchId INT,
@TC_UsedCarOfferId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF(@TC_UsedCarOfferId IS NOT NULL)
	BEGIN
	  UPDATE TC_UsedCarOffers 
	  SET ActualEndDate = EndDate , EndDate = GETDATE()-1, IsActive = 0
	  WHERE BranchId = @BranchId 
	  AND TC_UsedCarOfferId = @TC_UsedCarOfferId 
	  AND IsActive = 1

	/*  UPDATE TC_MappingOfferWithStock
	  SET EndDate = CONVERT(DATE,GETDATE() - 1)
	  WHERE TC_UsedCarOfferId = @TC_UsedCarOfferId*/

	  DELETE FROM TC_MappingOfferWithStock WHERE TC_UsedCarOfferId = @TC_UsedCarOfferId
	END
END
/****** Object:  Trigger [dbo].[TR_TC_MappingOfferWithStock]    Script Date: 02/19/2014 11:03:50 ******/
SET ANSI_NULLS ON
