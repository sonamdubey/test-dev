IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_InsertAndUpdateNewCarROI]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_InsertAndUpdateNewCarROI]
GO

	CREATE PROCEDURE [dbo].[CW_InsertAndUpdateNewCarROI]
@CarSegmentId INT,
@CityCategoryId INT,
@IsActive BIT,
@ROIValues VARCHAR(200),
@UpdatedOn DATETIME = NULL,
@UpdatedBy INT 
AS
BEGIN
	DECLARE @Index INT
	DECLARE @value FLOAT
	DECLARE @Tenor INT
	SET @Tenor =1

	IF NOT EXISTS(SELECT 1 FROM CW_NewCarROI WITH( NOLOCK) WHERE CW_CarSegmentId=@CarSegmentId AND CW_CityCategoryId=@CityCategoryId)
	BEGIN
	WHILE @ROIValues <> ''
	BEGIN
		--Get index of ',' in string
		--If found den return index else 0
		SET @Index = CHARINDEX(',', @ROIValues)

		--If found i.e. not 0
		IF @Index > 0
			BEGIN
				--Get required value before ','
				IF(LEFT(@ROIValues, @Index - 1)<>'')
				SET @value = LEFT(@ROIValues, @Index - 1)
				ELSE
				SET @value=NULL
				--Set remaining value after ',' to same variable again
				SET @ROIValues = RIGHT(@ROIValues, LEN(@ROIValues) - @Index)
			END
		ELSE
			BEGIN
				--To get remianing last value of the string
				SET @value = @ROIValues
				--Set oriinal string as '' to end loop
				SET @ROIValues = ''
			END
		INSERT INTO CW_NewCarROI(CW_CarSegmentId,CW_CityCategoryId,Tenor,ROI,IsActive,UpdatedBy) VALUES(@CarSegmentId,@CityCategoryId,@Tenor,@value,@IsActive,@UpdatedBy);
		SET @Tenor = @Tenor+1;
		--PRINT @Id
	END
	END

	ELSE
	BEGIN
	WHILE @ROIValues <> ''
	BEGIN
		--Get index of ',' in string
		--If found den return index else 0
		SET @Index = CHARINDEX(',', @ROIValues)

		--If found i.e. not 0
		IF @Index > 0
			BEGIN
				--Get required value before ','
				IF(LEFT(@ROIValues, @Index - 1)<>'')
				SET @value = LEFT(@ROIValues, @Index - 1)
				ELSE
				SET @value=NULL
				--Set remaining value after ',' to same variable again
				SET @ROIValues = RIGHT(@ROIValues, LEN(@ROIValues) - @Index)
			END
		ELSE
			BEGIN
				--To get remianing last value of the string
				SET @value = @ROIValues
				--Set oriinal string as '' to end loop
				SET @ROIValues = ''
			END
			UPDATE CW_NewCarROI SET ROI=@value,IsActive=@IsActive,UpdatedOn=@UpdatedOn,UpdatedBy=@UpdatedBy
			WHERE CW_CarSegmentId=@CarSegmentId AND CW_CityCategoryId=@CityCategoryId AND Tenor=@Tenor
		
		SET @Tenor = @Tenor+1;
		--PRINT @Id
	END


	END


END