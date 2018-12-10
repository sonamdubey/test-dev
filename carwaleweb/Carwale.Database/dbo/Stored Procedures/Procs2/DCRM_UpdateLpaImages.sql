IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UpdateLpaImages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UpdateLpaImages]
GO

	
-- =============================================
-- Author:		Amit Yadav
-- Create date: 12-04-2016
-- Description:	To insert attached LPA image filename.
-- EXEC [DCRM_UpdateLpaImages] 1,'Stark',1139,2,'172.16.1.68',3,null
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_UpdateLpaImages] 
	
	@SalesDealerId INT,	--SalesDealerId for which the image is been uploaded or deleted. 
	@AttachedLpaName VARCHAR(150)=NULL,	--Name of the LPA images.
	@AttachedLpaDetailsIds VARCHAR(50)=NULL,	--Id of M_AttachedLpaDetails table.
	@Type SMALLINT,	--Used for insert or update existing image
	@FileHostURL	VARCHAR(250) = NULL,
	@UpdatedBy	INT=NULL,
	@IsUpdate BIT OUTPUT,
	@LPAImageId INT OUTPUT
AS
BEGIN

 DECLARE @RowCount INT ,@TotalCount INT,@LpaId INT --@RowCount to get current row, @FileName for filename, @TotalCount for total number of rows in table,@LpaId Image Id which are to be deleted.
 
 IF @Type=1 --For Insert
	 BEGIN
	 
	 IF @AttachedLpaName IS NOT NULL AND @AttachedLpaName !='' 
		BEGIN
			SET @AttachedLpaName = CAST(@SalesDealerId AS VARCHAR)+'_'+@AttachedLpaName
			INSERT INTO M_AttachedLpaDetails
			(
				SalesDealerId,
				AttachedFileName,
				FileHostUrl,
				UploadedOn,
				UploadedBy
			)
			VALUES
			(
				@SalesDealerId,
				@AttachedLpaName,
				@FileHostURL,
				GETDATE(),
				@UpdatedBy
			)
			SET @LPAImageId=SCOPE_IDENTITY();
			SET @IsUpdate=1
		END
	END

 IF @Type=2 AND @AttachedLpaDetailsIds IS NOT NULL AND @AttachedLpaDetailsIds != '' --@Type =2 For Update 
	BEGIN
		DECLARE @TempLpaIds TABLE (RowId INT IDENTITY(1,1), LpaId INT)
		INSERT INTO @TempLpaIds SELECT * FROM fnSplitCSV(@AttachedLpaDetailsIds)
		SET @TotalCount = @@ROWCOUNT --Total rows in table
		SET @RowCount = 1
		SELECT * FROM @TempLpaIds
		
		WHILE @RowCount<=@TotalCount
		BEGIN
			SELECT @LpaId=LpaId FROM @TempLpaIds WHERE RowId = @RowCount
			UPDATE M_AttachedLpaDetails --For deleting a image, make isActive = 0
			SET IsActive=0,UploadedBy=@UpdatedBy
			WHERE Id=@LpaId
			SET @RowCount += 1
		END
		SET @IsUpdate=1
	END
END
--------------------------------------------------------------------------------------------------------------


