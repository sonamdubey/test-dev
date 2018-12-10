IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_AddUsedCarStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_AddUsedCarStock]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: <Create Date,,>
-- Description:	Add Stock in table --BA_Stock || BA_StockDetails
-- =============================================

CREATE PROCEDURE [dbo].[BA_AddUsedCarStock] 
	-- Add the parameters for the stored procedure here
       @BrokerId INT,
      @Kms FLOAT = NULL,
      @Color VARCHAR(50)  = NULL,
      @EntryDate DATETIME = NULL,
      @Comments VARCHAR(500) = NULL,
      @IsActive BIT = 1, --ISActive
      @VersionId INT = NULL,
      @OwnerTypeId  INT = NULL,
	  @Price VARCHAR(20) = NULL,
	  @MakeYear VARCHAR(20)= NULL
	 
AS
BEGIN
	DECLARE @StockId INT = NULL
	DECLARE @MakeId INT = NULL
    DECLARE  @ModelId INT = NULL
	DECLARE @ModifyDate DATETIME  = NULL
     DECLARE @DeletdDate DATETIME = NULL
	--DECLARE  @MakeYear VARCHAR(20) = NULL
	 DECLARE @TransmissionId INT = NULL
	 DECLARE @FuelTypeId INT = NULL

---Get the ModelId and MakeId || Transmission \\FuelType
SELECT @ModelId=CV.CarModelId, @MakeId = CM.CarMakeId,@TransmissionId = CV.CarTransmission,@FuelTypeId = CV.CarFuelType 
FROM CarVersions AS CV  WITH(NOLOCK)
INNER JOIN CarModels AS CM  WITH(NOLOCK) ON CV.ID = @VersionId AND CV.CarModelId = CM.ID
	SET NOCOUNT OFF;
	---Insert into Stock Table .
	INSERT INTO [dbo].[BA_Stock]
           ([BrokerId]
           ,[Kms]
           ,[Color]
           ,[OwnerTypeId]
		   ,[TransmissionId]
           ,[FuelTypeId]
           ,[Comments]
		   ,[Price]
           ,[IsActive])
     VALUES
			(@BrokerId,
			@Kms,
			@Color,
			@OwnerTypeId,
			@TransmissionId,
			@FuelTypeId,
			@Comments,
			@Price,
			@IsActive )

--Get the Stock Id from Stock table.
SET @StockId = SCOPE_IDENTITY();
---SELECT  @StockId  = @@IDENTITY


--SELECT @ModelId, @MakeId

--IF @ModelId <> NULL AND @MakeId <> NULL AND @StockId <> NULL
BEGIN
--Insert into Stock Details table also
INSERT INTO [dbo].[BA_StockDetails]
           ([BrokerId]
           ,[StockId]
           ,[Kms]
           ,[Color]
           ,[OwnerTypeId]
		    ,[TransmissionId]
           ,[FuelTypeId]
           ,[Comments]
           ,[IsActive]
           ,[EntryDate]
           ,[ModifyDate]
           ,[DeletedDate]
           ,[CarMakeId]
           ,[CarModelId]
           ,[CarVersionId]
           ,[MakeYear]
		   ,[Price])
     VALUES
	 (@BrokerId,
	 @StockId,
	 @Kms,
	 @Color,
	 @OwnerTypeId,
	 @TransmissionId,
	 @FuelTypeId,
	 @Comments,
	 @IsActive,
	 @EntryDate,
	 NUll, --Modify Date
	 NULL, --Deleted Date
	 @MakeId,
	 @ModelId,
	 @VersionId,
	 @MakeYear,
	 @Price
	 )
END
---
SELECT @StockId AS ID --return 
    
END
