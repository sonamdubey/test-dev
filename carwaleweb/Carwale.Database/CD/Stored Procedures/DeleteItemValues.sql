IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[DeleteItemValues]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[DeleteItemValues]
GO

	
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
	THIS STORED PROCEDURE DELETE THE DATA VALUES OF ITEMS FROM ITEM VALUES
	TABLE FOR A PARTICULAR CAR VERSION

	WRITTEN BY : SHIKHAR MAHESHWARI ON 13 APR 2012

	Changes History:
       
       Edited By               		EditedON               		Description
       Amit Verma					Mar 28, 2013				Added logic to fill group item value
       Reshma Shetty				04/07/2013					Update RecommendCars in case the version exists in that table
	   amit verma					10/14/2013					Update model summary on value change
       --------                	-----------       	             ----------
*/

CREATE  PROCEDURE   [CD].[DeleteItemValues]
	@carVersionId INT = NULL,
	@itemMasterId INT = NULL
AS
BEGIN
	DELETE FROM 
		[CD].[ItemValues]
	WHERE
		CarVersionId = @carVersionId
	AND
		ItemMasterId = @itemMasterId
	
	EXEC [CD].[FillGroupItemValues] @carVersionId,@itemMasterId --added by amit v
	EXEC [CD].[FillSpecValues] @carVersionId,@itemMasterId --added by amit v

--Modified By Reshma Shetty 04/07/2013 Update RecommendCars in case the version exists in that table
------------------------------------------------------------------------------------------------------------------------------------	
	IF ((@itemMasterId=29 OR @itemMasterId=150 OR @itemMasterId=81 OR @itemMasterId=55 OR @itemMasterId=149 OR @itemMasterId=155) 
	AND EXISTS (SELECT TOP 1 RecommendCarId FROM RecommendCars WITH(NOLOCK) WHERE Versionid=@carVersionId))
    EXEC UpdateRecommendCars @carVersionId
------------------------------------------------------------------------------------------------------------------------------------	

-----amit verma					10/14/2013					Update model summary on value change
	IF(@ItemMasterID IN (14,15,26,29,30))
	BEGIN
		EXEC [CD].[GenModelSummary] @carVersionId,@ItemMasterID
	END

END

