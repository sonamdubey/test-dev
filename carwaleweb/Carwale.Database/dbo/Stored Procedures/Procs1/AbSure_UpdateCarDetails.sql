IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_UpdateCarDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_UpdateCarDetails]
GO

	-- ============================================================
-- Author:		Yuga Hatolkar
-- ALTER date: 
-- Description:	
-- Modified By : Suresh Prajapati on 16th June, 2015
-- Description : Updated version fuel type on versionId update
-- ============================================================
CREATE PROCEDURE [dbo].[AbSure_UpdateCarDetails] @Make INT
	,@Model INT
	,@Version INT
	,@MakeName VARCHAR(100)
	,@ModelName VARCHAR(100)
	,@VersionName VARCHAR(100)
	,@RegNo VARCHAR(50)
	,@RegNoOld VARCHAR(50)
	,@CarId INT
	,@UpdatedOn DATETIME
	,@UpdatedBy INT
	,@MakeIdOld INT
	,@ModelIdOld INT
	,@VersionIdOld INT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @OldMake VARCHAR(100)
		,@OldModel VARCHAR(100)
		,@OldVersion VARCHAR(100)
		,@OldVersionId INT
		,@NewFuelType TINYINT

	INSERT INTO AbSure_CarDetailsLog (
		AbSureCarId
		,VersionId
		,UpdatedOn
		,UpdatedBy
		,RegNumber
		)
	VALUES (
		@CarId
		,@VersionIdOld
		,@UpdatedOn
		,@UpdatedBy
		,@RegNoOld
		)

	SET @NewFuelType = (
			SELECT CarFuelType
			FROM CarVersions
			WHERE ID = @Version
			)

	--INSERT INTO AbSureCarDetailsLog (AbSureCarId, VersionId, UpdatedOn, UpdatedBy, RegNumber)
	--SELECT @CarId, @VersionIdOld, @UpdatedOn, @UpdatedBy, @RegNoOld FROM AbSure_CarDetails ACD
	--INNER JOIN vwMMV V ON V.VersionId = ACD.VersionId
	--WHERE Id=@CarId
	UPDATE AbSure_CarDetails
	SET Make = @MakeName
		,Model = @ModelName
		,Version = @VersionName
		,VersionId = @Version
		,FuelType = @NewFuelType
		,RegNumber = @RegNo
	WHERE Id = @CarId
END

