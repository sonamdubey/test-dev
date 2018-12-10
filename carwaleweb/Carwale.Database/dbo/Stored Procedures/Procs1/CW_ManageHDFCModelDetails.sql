IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_ManageHDFCModelDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_ManageHDFCModelDetails]
GO

	CREATE PROCEDURE [dbo].[CW_ManageHDFCModelDetails]
@IsUpdate bit=0,
@ModelId Numeric(18,0),
@SegmentId int,
@TierId int,
@RecordExists bit output,
@UpdatedBy INT = NULL,
@UpdatedOn DATETIME = NULL
AS
--Author:Rakesh Yadav On 02 Aug 2015
--Desc: Add and update records in CW_CarModelDetails, If record is already present then prevent new entry in table
--Modifier : Vaibhav K added parameters for UpdatedBy & UpdatedOn
BEGIN
	SET @RecordExists = 1
	IF NOT EXISTS (SELECT ID  FROM CW_CarModelDetails WHERE CarModelId=@ModelId) AND @IsUpdate=0
	BEGIN
		INSERT INTO CW_CarModelDetails (CarModelId,CW_CarSegmentId,CW_CarTierId,IsActive, UpdatedBY)
		VALUES(@ModelId,@SegmentId,@TierId,1, @UpdatedBy)

		SET @RecordExists=0
	END
	ELSE
	BEGIN
		Set @RecordExists=1
		IF @IsUpdate=1
		BEGIN
			UPDATE CW_CarModelDetails
			SET CW_CarSegmentId=@SegmentId,CW_CarTierId=@TierId,IsActive=1,
				UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn
			WHERE CarModelId=@ModelId
		END
	END
END

