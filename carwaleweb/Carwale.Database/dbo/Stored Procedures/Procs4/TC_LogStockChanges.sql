IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LogStockChanges]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LogStockChanges]
GO

	--===================================================================================
-- Modified by Manish on 24-07-2014 commented left join with tc_carphotos since no data is fetching from that table.
-- Modified By : Suresh prajapati on 20th july, 2015
-- Description : To Log PurchaseCost & RefurbishmentCost changes
--===================================================================================
CREATE PROCEDURE [dbo].[TC_LogStockChanges] @BranchId INT
	,@UserId INT
	,@StockId BIGINT
	,@kms INT
	,@MakeYear DATETIME
	,@ExpectedPrice INT
	,@SpecialNote VARCHAR(MAX)
	,@PurchaseCost  INT=NULL
	,@RefurbishmentCost INT =NULL
AS
BEGIN
	DECLARE @ExpectedPriceOld VARCHAR(50)
		,@kmsOld INT
		,@MakeYearOld DATETIME
		,@SpecialNoteOld VARCHAR(500) --,	@PhotoCount SMALLINT = NULL

	IF (@StockId IS NOT NULL)
	BEGIN
		SELECT @kmsOld = ST.Kms
			,@MakeYearOld = St.MakeYear
			,@ExpectedPriceOld = ST.Price
			,@SpecialNoteOld = CC.Comments --, @PhotoCount = COUNT(CP.Id) OVER()
		FROM TC_Stock ST WITH (NOLOCK)
		INNER JOIN TC_CarCondition CC WITH (NOLOCK) ON CC.StockId = ST.Id
		--LEFT JOIN TC_CarPhotos CP WITH(NOLOCK) ON CP.StockId = ST.Id
		WHERE ST.Id = @StockId
			AND ST.BranchId = @BranchId

		IF (
				@kms <> @kmsOld
				OR @MakeYear <> @MakeYearOld
				OR @ExpectedPrice <> @ExpectedPriceOld
				OR @SpecialNote <> @SpecialNoteOld
				)
			INSERT INTO TC_StockChangesLog (
				TC_StockId
				,BranchId
				,Kms
				,MakeYear
				,ExpectedPrice
				,SpecialNote /*,PhotoCount*/
				,SpecialNoteCharCount
				,PurchaseCost
				,RefurbishmentCost
				)
			VALUES (
				@StockId
				,@BranchId
				,@kms
				,@MakeYear
				,@ExpectedPrice
				,@SpecialNote /*, @PhotoCount*/
				,LEN(@SpecialNote)
				,@PurchaseCost
				,@RefurbishmentCost
				)
	END
END
