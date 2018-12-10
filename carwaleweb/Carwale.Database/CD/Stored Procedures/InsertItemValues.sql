IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[InsertItemValues]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[InsertItemValues]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
	THIS STORED PROCEDURE INSERTS THE DATA VALUES OF ITEMS FROM ITEM MASTER
	FOR A PARTICULAR CAR VERSION
	IF THE RECORD ALREADY EXISTS, UPDATE THE RECORD

	WRITTEN BY : SHIKHAR MAHESHWARI ON 11 APR 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       Shikhar Maheshwari			April 30, 2012              Added a field of custom text to
       --------                		-----------       	        Item Values Table
       
       Amit Verma					Mar 28, 2013				Added logic to fill group item value
       Reshma Shetty				04/07/2013					Update RecommendCars in case the version exists in that table
	   amit verma					10/14/2013					Update model summary on value change
       --------                		-----------       	             
*/

CREATE  PROCEDURE   [CD].[InsertItemValues]
	@carVersionId INT = NULL,
	@itemMasterId INT = NULL,
	@dataTypeId INT = NULL,
	@itemValue FLOAT = NULL,
	@userDefinedId INT = NULL,
	@customText VARCHAR(200) = NULL,
	@userName VARCHAR(50) = NULL
AS
BEGIN
	IF EXISTS(SELECT * FROM CD.ItemValues WITH (NOLOCK) WHERE CarVersionId = @carVersionId AND ItemMasterId = @itemMasterId)
	BEGIN
		UPDATE [CD].[ItemValues]
		SET
			ItemValue = @itemValue,
			UserDefinedId = @userDefinedId,
			CustomText = @customText,
			UpdatedOn = GETDATE(),
			UpdatedBy = @userName
		WHERE
			CarVersionId = @carVersionId
		AND
			ItemMasterId = @itemMasterId
	END
	ELSE
	BEGIN
		INSERT INTO CD.ItemValues
		(
			CarVersionId,
			ItemMasterId,
			DataTypeId,
			ItemValue,
			UserDefinedId,
			CustomText,
			UpdatedBy
		)
		VALUES
		(
			@carVersionId,
			@itemMasterId,
			@dataTypeId,
			@itemValue,
			@userDefinedId,
			@customText,
			@userName	
		)
	END


	EXEC [CD].[FillGroupItemValues] @carVersionId,@itemMasterId --added by amit v
	EXEC [CD].[FillSpecValues] @carVersionId,@itemMasterId --added by amit v

--Modified By Reshma Shetty 04/07/2013 Update RecommendCars in case the version exists in that table
------------------------------------------------------------------------------------------------------------------------------------	
	IF ((@itemMasterId=29 OR @itemMasterId=150 OR @itemMasterId=81 OR @itemMasterId=55 OR @itemMasterId=149 OR @itemMasterId=155) 
		AND EXISTS (SELECT TOP 1 RecommendCarId FROM RecommendCars WITH(NOLOCK) WHERE Versionid=@carVersionId))
	EXEC UpdateRecommendCars @carVersionId

------------------------------------------------------------------------------------------------------------------------------------	

----amit verma				10/14/2013					Update model summary on value change
	IF(@ItemMasterID IN (14,15,26,29,30))
	BEGIN
		EXEC [CD].[GenModelSummary] @carVersionId,@ItemMasterID
	END
END


