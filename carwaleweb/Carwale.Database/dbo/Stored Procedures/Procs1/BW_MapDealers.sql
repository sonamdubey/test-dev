IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_MapDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_MapDealers]
GO

	
-----------------------------------------------
-- Created By : Sadhana Upadhyay on 7 Nov 2014
-- Summary : To Map dealer with list of area
-----------------------------------------------
CREATE PROCEDURE [dbo].[BW_MapDealers] 
	@DealerId INT
	,@AreaIdList VARCHAR(MAX)
AS
BEGIN
	DECLARE @Temp_DealerAreaMapping TABLE (DealerId INT ,AreaId INT)

	INSERT INTO @Temp_DealerAreaMapping (DealerId ,AreaId)
	SELECT @DealerId ,FN.ListMember FROM dbo.fnSplitCSVValues(@AreaIdList) FN;

	MERGE BW_DealerAreaMapping AS DAM
	USING ( SELECT DealerId ,AreaId FROM @Temp_DealerAreaMapping) AS TEMP
		ON TEMP.DealerId = DAM.DealerId AND TEMP.AreaId = dam.AreaId
	WHEN MATCHED
		THEN
			UPDATE
			SET DAM.IsActive = 1
	WHEN NOT MATCHED
		THEN
			INSERT (DealerId ,AreaId ,IsActive)
			VALUES (TEMP.DealerId ,TEMP.AreaId ,1);
END

