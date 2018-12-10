IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelOnBranchId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelOnBranchId]
GO

	--	Modifier	:	Vinayak Mishra
--	Purpose		:	To get all Models on which dealer is working based on its DealerId or BranchId
--	============================================================

CREATE PROCEDURE [dbo].[GetModelOnBranchId]

@BranchId	NUMERIC (18,0),
@ApplicationId TINYINT = 1
AS

BEGIN
	SELECT DISTINCT V.ModelId AS Value, V.Model AS Text 
     FROM TC_DealerMakes D WITH(NOLOCK) 
     INNER JOIN  Dealer_NewCar AS DNC WITH(NOLOCK)  ON DNC.TcDealerId=D.DealerId
	 INNER JOIN vwAllMMV V ON V.MakeId = D.MakeId 
     INNER JOIN CarModels CM ON CM.ID=V.ModelId
	 WHERE D.DealerId = @BranchId
	 AND V.New=1
	 AND CM.New=1 and CM.IsDeleted=0
	 AND V.ModelId NOT IN (Select ModelId from TC_NoDealerModels WITH(NOLOCK) WHERE DealerId = @BranchId )
	 AND V.ApplicationId = ISNULL(@ApplicationId,1)
	 ORDER BY V.Model
END