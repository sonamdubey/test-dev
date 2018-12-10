IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelOnBranchId_Api_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelOnBranchId_Api_16]
GO

	 
--	============================================================
--	Modifier	:	Vinayak Mishra
--  Modified    :   Vicky Lund, 3/2/2016, Added makeId and renamed other columns
--	Purpose		:	To get all Models on which dealer is working based on its DealerId or BranchId
--	============================================================
CREATE PROCEDURE [dbo].[GetModelOnBranchId_Api_16.2.4] @BranchId INT
	,@ApplicationId TINYINT = 1
AS
BEGIN
	SELECT DISTINCT V.ModelId
		,V.Model AS ModelName
		,V.MakeId
	FROM TC_DealerMakes D WITH (NOLOCK)
	INNER JOIN Dealers AS DNC WITH (NOLOCK) ON DNC.ID = D.DealerId
	INNER JOIN vwMMV V WITH (NOLOCK) ON V.MakeId = D.MakeId
	WHERE D.DealerId = @BranchId
		AND V.IsModelNew = 1
		AND V.ModelId NOT IN (
			SELECT ModelId
			FROM TC_NoDealerModels WITH (NOLOCK)
			WHERE DealerId = @BranchId
			)
	ORDER BY V.Model
END

