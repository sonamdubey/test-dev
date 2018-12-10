IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RemoveStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RemoveStock]
GO

	
-- Author:		Binumon George
-- Create date: 12 Mar 2012
-- Description:	 Delete stock if stock not referenced to any other table.
-- Modified By : Tejashree Patil on 20 feb 2013, checked condition TC_LeadDispositionId IS NULL 
-- Modified By : Tejashree Patil on 4 Aug 2014, WITH(NOLOCK) implemented.
-- Modified By : vivek gupta on 15-07-2015, changed select query inside if not exists to make it fast
-- Modified By : Kritika Choudhary on 9th may 2016, added parameter @IsForce and @IsForce condition inside else part
-- Modified by : Kritika Choudhary on 5th oct 2016, remove stock for cartrade,added isforce=1 condition first 
-- Modified by : Kritika Choudhary on 6th oct 2016, added condition (if stockid and branchId does not match)
-- =============================================
CREATE PROCEDURE [dbo].[TC_RemoveStock] (
	@BranchId NUMERIC
	,@StockId INT
	,@Status INT OUTPUT
	,@IsForce BIT = 0
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status = 0

	IF (
			EXISTS (
				SELECT id
				FROM TC_Stock
				WHERE id = @StockId
					AND BranchId = @BranchId
				)
			) -- Added by : Kritika Choudhary on 6th sep 2016, condition for if stockid and branchId match
	BEGIN
		IF (@IsForce = 1) -- Added by : Kritika Choudhary on 5th sep 2016, remove stock for cartrade
		BEGIN
			UPDATE TC_Stock
			SET IsActive = 0
			WHERE Id = @StockId

			SET @Status = 1 --Successfully updated
		END
		ELSE
			IF NOT EXISTS --checkig here stock id available on basis of IsSychronizedCW=1 in tc_stock and TC_BuyerInquiries tables
				(
					SELECT TOP 1 ST.Id
					FROM TC_Stock ST WITH (NOLOCK)
					LEFT JOIN TC_BuyerInquiries BI WITH (NOLOCK) ON ST.Id = BI.StockId
						AND BI.TC_LeadDispositionId IS NULL -- Modified By : Tejashree Patil on 20 feb 2013
						AND BI.StockId = @StockId -- Modified By : vivek gupta on 15-07-2015
					WHERE (
							ST.IsSychronizedCW = 1
							AND ST.Id = @StockId
							)
						OR BI.StockId = @StockId
					)
			BEGIN
				UPDATE TC_Stock
				SET IsActive = 0
				WHERE Id = @StockId

				SET @Status = 1 --Successfully updated
			END
			ELSE
				SET @Status = 2 --this StockId referenced with Tc_Stock or TC_BuyerInquiries tables
	END
	ELSE
		SET @Status = 3 --stockid and branchid does not match
END
