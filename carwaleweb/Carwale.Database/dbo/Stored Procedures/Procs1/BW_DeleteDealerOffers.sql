IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_DeleteDealerOffers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_DeleteDealerOffers]
GO

	
-- =============================================
-- Author:		Suresh Prajapati
-- Create date: 05th Nov 2014
-- Description:	To Delete Dealer offers.
-- Modified By : Sadhana Upadhyay on 8 Oct 2015
-- Summary : To delete multiple offers
-- =============================================
CREATE PROCEDURE [dbo].[BW_DeleteDealerOffers] 
@OfferIds VARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE BWO
	SET bwo.IsActive = 0
	FROM BW_PQ_Offers BWO 
	INNER JOIN dbo.fnSplitCSVValuesWithIdentity(@OfferIds) AS FN ON FN.ListMember = BWO.Id
END
