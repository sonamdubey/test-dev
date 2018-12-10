IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_EditUsedCarStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_EditUsedCarStock]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	Add Stock in table --BA_Stock || BA_StockDetails
-- =============================================
--Add Tranction here
CREATE PROCEDURE [dbo].[BA_EditUsedCarStock] 
	-- Add the parameters for the stored procedure here
      @StockId INT,
      @Kms FLOAT = NULL,
      @Color VARCHAR(50)  = NULL,
      @ModifyDate DATETIME = NULL,
      @Comments VARCHAR(500) = NULL,
      @IsActive BIT = 1, --ISActive
      @VersionId INT = NULL,
	  @Price VARCHAR(20) = NULL,
	  @MakeYear DATE,
      @OwnerTypeId  INT = NULL
	 
AS
BEGIN
	DECLARE @MakeId INT = NULL
    DECLARE  @ModelId INT = NULL
    DECLARE @DeletdDate DATETIME = NULL
	--DECLARE  @MakeYear VARCHAR(20) = NULL
	DECLARE @Status BIT = 0
	DECLARE @TransmissionId TINYINT  = NULL
	DECLARE  @FuelTypeId INT = NULL

	SET NOCOUNT ON;

---Get the ModelId and MakeId
SELECT @ModelId=CV.CarModelId, @MakeId = CM.CarMakeId, @TransmissionId = CV.CarTransmission, @FuelTypeId = Cv.CarFuelType 
FROM CarVersions AS CV WITH(NOLOCK)
INNER JOIN CarModels AS CM WITH(NOLOCK) ON CV.ID = @VersionId AND CV.CarModelId = CM.ID

---Update Stock Table .
UPDATE [dbo].[BA_Stock]
   SET 
      [Kms] = @Kms
      ,[Color] = @Color
      ,[OwnerTypeId] = @OwnerTypeId
      ,[TransmissionId] = @TransmissionId
      ,[FuelTypeId] = @FuelTypeId
      ,[Comments] = @Comments
	  ,[Price] = @Price
 WHERE  ID = @StockId

--IF @ModelId <> NULL AND @MakeId <> NULL AND @StockId <> NULL
BEGIN
--Insert into Stock Details table also
UPDATE [dbo].[BA_StockDetails]
   SET 
      [Kms] = @Kms
      ,[Color] = @Color
      ,[OwnerTypeId] = @OwnerTypeId
      ,[FuelTypeId] = @FuelTypeId
      ,[Comments] = @Comments
	  ,[TransmissionId] = @TransmissionId
      ,[ModifyDate] = GETDATE()
      ,[CarMakeId] = @MakeId
      ,[CarModelId] = @ModelId
      ,[CarVersionId] = @VersionId
      ,[MakeYear] = @MakeYear
	  ,[Price] = @Price
 WHERE StockId = @StockId

 ---SET @StatusId = 2
END

SET @Status = 1  ---------

SELECT @Status AS Status --return    

END

-------------------------
/****** Object:  StoredProcedure [dbo].[BA_GetCarWaleDealerContact]    Script Date: 6/9/2014 7:13:28 PM ******/
SET ANSI_NULLS ON
