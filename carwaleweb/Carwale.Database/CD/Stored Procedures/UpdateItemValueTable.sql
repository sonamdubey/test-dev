IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[UpdateItemValueTable]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[UpdateItemValueTable]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: 07 March 2013
-- Description:	Saves or deletes ItemValue (called from car data models page in OPR CarDataIO)
-- =============================================
/*
	Changes History:
       
       Edited By               		EditedON               		Description
       ----------------       -----------------              	-----------------------
       Amit Verma					28/03/2013					Added logic to save group item values
       Reshma Shetty				04/07/2013					Update RecommendCars in case the version exists in that table
       Amit Verma					13/09/2013					Added logic to fire trigger for itemvalues table on bulk updation
	   amit verma					10/14/2013					Update model summary on value change
*/

CREATE  PROCEDURE   [CD].[UpdateItemValueTable]
	@ItemMasterID int,
	@DeleteValues [CD].[VersionValue] READONLY,
	@UpdateValues [CD].[VersionValue] READONLY,
	@InsertValues [CD].[VersionValue] READONLY,
	@UpdatedBy	VARCHAR(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @dataType tinyint
	SET @dataType = (SELECT DatatypeId FROM CD.ItemMaster WHERE ItemMasterId = @ItemMasterID)
	IF((SELECT Count(VersionID) FROM @DeleteValues) != 0)
	BEGIN
		DELETE FROM CD.ItemValues WHERE ItemMasterId = @ItemMasterID AND
		CarVersionId IN (SELECT VersionID FROM @DeleteValues)
	END
	IF((SELECT Count(VersionID) FROM @UpdateValues) != 0)
	BEGIN
		IF (@dataType = 2 OR @dataType = 3)
		BEGIN
			UPDATE CD.ItemValues
			SET ItemValue = UV.Value , UpdatedOn = GETDATE() , UpdatedBy = @UpdatedBy
			FROM @UpdateValues UV JOIN CD.ItemValues IV ON
			UV.VersionID = IV.CarVersionId
			WHERE IV.ItemMasterId = @ItemMasterID
		END
		
		IF (@dataType = 4)
		BEGIN
			UPDATE CD.ItemValues
			SET UserDefinedId = UV.Value, UpdatedOn = GETDATE(), UpdatedBy = @UpdatedBy
			FROM @UpdateValues UV JOIN CD.ItemValues IV ON
			UV.VersionID = IV.CarVersionId
			WHERE IV.ItemMasterId = @ItemMasterID
		END
		
		IF (@dataType = 5)
		BEGIN
			UPDATE CD.ItemValues
			SET CustomText = UV.Value, UpdatedOn = GETDATE(), UpdatedBy = @UpdatedBy
			FROM @UpdateValues UV JOIN CD.ItemValues IV ON
			UV.VersionID = IV.CarVersionId
			WHERE IV.ItemMasterId = @ItemMasterID
		END
	END
	IF((SELECT Count(*) FROM @InsertValues) != 0)
	BEGIN
		IF(@dataType = 2 OR @dataType = 3)
		BEGIN
			INSERT INTO CD.ItemValues (ItemMasterId,CarVersionId,DataTypeId,ItemValue,UpdatedBy)
			SELECT @ItemMasterID,VersionID,@dataType,Value, @UpdatedBy FROM @InsertValues
		END
		
		IF(@dataType = 4)
		BEGIN
			INSERT INTO CD.ItemValues (ItemMasterId,CarVersionId,DataTypeId,UserDefinedId,UpdatedBy)
			SELECT @ItemMasterID,VersionID,@dataType,Value, @UpdatedBy FROM @InsertValues
		END
		
		IF(@dataType = 5)
		BEGIN
			INSERT INTO CD.ItemValues (ItemMasterId,CarVersionId,DataTypeId,CustomText,UpdatedBy)
			SELECT @ItemMasterID,VersionID,@dataType,Value, @UpdatedBy FROM @InsertValues
		END
	END
	DECLARE @versionIDs TABLE 
  ( 
     id        INT IDENTITY, 
     VersionID INT 
  ) 

	INSERT INTO @versionIDs 
	SELECT VersionID 
	FROM   @DeleteValues 
	UNION 
	SELECT VersionID 
	FROM   @UpdateValues
	UNION
	SELECT VersionID
	FROM @InsertValues 

	DECLARE @VersionID INT = -1 
	DECLARE @rowCount INT = (SELECT COUNT(id) 
	   FROM   @versionIDs) 




	WHILE ( @rowCount > 0 )
	  BEGIN 
		  SET @VersionID = (SELECT TOP 1 VersionID 
							FROM   @versionIDs 
							WHERE  id = @rowCount) 

		  EXEC [CD].[FillGroupItemValues] 
			@VersionID, 
			@ItemMasterID 
		
		  EXEC [CD].[FillSpecValues] @VersionID,@ItemMasterID --added by amit v


----------amit verma				10/14/2013					Update model summary on value change
		  IF(@ItemMasterID IN (14,15,26,29,30))
		  BEGIN
		      EXEC [CD].[GenModelSummary] @VersionID,@ItemMasterID
		  END

		  ---- Added by Amit on  23-09-2013 to fire trigger for itemvalues table on bulk updation
	UPDATE CD.ItemValues SET ItemValue = ItemValue WHERE
	ItemMasterId = @ItemMasterID and CarVersionId =@VersionID
-----------------------------------------------------------------------------------------------------
		  
		  SET @rowCount -=1 
	  END
	  
--Modified By Reshma Shetty 04/07/2013 Update RecommendCars in case the version exists in that table
---------------------------------------------------------------------------------------------------------------------------------------------	 
	 
	 IF (@itemMasterId=29 OR @itemMasterId=150 OR @itemMasterId=81 OR @itemMasterId=55 OR @itemMasterId=149 OR @itemMasterId=155) 
	 BEGIN
		 DECLARE @RCversionIDs TABLE 
		  ( 
			 id        INT IDENTITY, 
			 VersionID INT 
		  ) 
		  
		 INSERT INTO @RCversionIDs(VersionID)
		 SELECT VI.VersionID 
		 FROM @versionIDs VI
		 INNER JOIN RecommendCars RC WITH(NOLOCK) ON RC.Versionid=VI.VersionID
		 
		 SET @rowCount = (SELECT COUNT(id) 
		   FROM   @RCversionIDs) 

		 WHILE ( @rowCount > 0 )
	     BEGIN 
		    SET @VersionID = (SELECT VersionID 
							FROM   @RCversionIDs 
							WHERE  id = @rowCount) 

		    EXEC UpdateRecommendCars @VersionID

		    SET @rowCount -=1 
	     END
	 
	 END
---------------------------------------------------------------------------------------------------------------------------------------------	 
	 
END

