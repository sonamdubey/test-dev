IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_UnmapDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_UnmapDealers]
GO

	---------------------------------------------------------
-- Created By : Sadhana Upadhyay on 7 Nov 2014
-- Summary : To unmap dealers from list of areas
---------------------------------------------------------
CREATE PROCEDURE [dbo].[BW_UnmapDealers] 
	@DealerId INT
	,@AreaIdList VARCHAR(MAX)
AS
BEGIN
	UPDATE BW_DealerAreaMapping
	SET IsActive = 0
	FROM BW_DealerAreaMapping DAM
	INNER JOIN dbo.fnSplitCSVValues(@AreaIdList) AS FN ON FN.ListMember = DAM.AreaId
		AND DAM.DealerId = @DealerId
END

