IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_InsertDealerAndCordinator]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_InsertDealerAndCordinator]
GO

	

-- =============================================
-- Author:		Dilip Vasu
-- Create date: 30th Aug 2011
-- Description:	This proc inserts DealerId, Dealer Coordinator ID, UpdatedOn,UpdatedBy,CreatedOn
-- =============================================

CREATE PROCEDURE [dbo].[CRM_InsertDealerAndCordinator]
(
	@DealerCoId INT,
	@DealerList VARCHAR(8000),
	@UpdatedBy INT
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @DealerId VARCHAR(10), @Pos INT

	SET @DealerList = LTRIM(RTRIM(@DealerList))+ ','
	SET @Pos = CHARINDEX(',', @DealerList, 1)

	IF REPLACE(@DealerList, ',', '') <> ''
	BEGIN
		WHILE @Pos > 0
		BEGIN
			SET @DealerId = LTRIM(RTRIM(LEFT(@DealerList, @Pos - 1)))
			IF @DealerId <> ''
			BEGIN
				INSERT INTO CRM_ADM_DCDealers(DealerId,DCID,UpdatedOn, UpdatedBy, CreatedOn)
					VALUES (CAST(@DealerId AS INT),@DealerCoId,GETDATE(),@UpdatedBy,GETDATE())						
			END
			SET @DealerList = RIGHT(@DealerList, LEN(@DealerList) - @Pos)
			SET @Pos = CHARINDEX(',', @DealerList, 1)

		END
	END	
		
END
