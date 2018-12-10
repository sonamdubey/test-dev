IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ClaimOffer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ClaimOffer]
GO

	--Author: Tejashree Patil on 30 Nov 2014
--Description : To check whether dealer is allow to claim offer or not.
CREATE PROCEDURE [dbo].[TC_ClaimOffer] @VersionId INT
	,@BranchId INT
AS
BEGIN
	--Return all offers on that version
	EXEC GetAggregateOffers - 1
		,- 1
		,@VersionId
		,- 1

	--Return Dealer_NewCar table Id based on logged dealer's id i.e Dealer Table id 
	SELECT D.Id NewCarDealerId
		,D.Id DealerId
	FROM Dealers D WITH (NOLOCK)
	--INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = DNC.TcDealerId
	WHERE D.ID IS NOT NULL
		AND D.ID = @BranchId
		AND D.IsTCDealer = 1
		AND D.TC_DealerTypeId = 2
END

