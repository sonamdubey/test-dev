IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDCarsSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDCarsSave]
GO

	
-- =============================================
-- Author:		Binumon George
-- Create date: 15-06-2012
-- Description:	Entering TD cars details
-- Modified by Nilesh on 17-09-2013 for maintaining logs of Dealers masters for Mobile APP
-- Modified By : Suresh Prajapati on 22nd April, 2016
-- Description : Removed concatination of @CarName with @RegNo in new td car add condition
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDCarsSave]
	-- Add the parameters for the stored procedure here
	@TDCarsId INT = NULL
	,@BranchId INT
	,@VersionId INT
	,@RegNo VARCHAR(50)
	,@KmsDriven NUMERIC(18, 0)
	,@CarName VARCHAR(100)
	,@VinNo VARCHAR(50)
	,@Status INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status = 0 -- Default staus will be 0.Insertion or updation not happened status will be 0

	-- Insert statements for procedure here
	--IF (@RegNo IS NOT NULL)
	--BEGIN
	--	SET @CarName = @CarName + '(' + ISNULL(@RegNo, '') + ')'
	--END
	IF (@TDCarsId IS NULL) --IF id parameter is null, we inserting new Tdcars to TC_TDCars table
	BEGIN
		--IF (@RegNo IS NOT NULL)
		--BEGIN
		--	SET @CarName = @CarName + '(' + ISNULL(@RegNo, '') + ')'
		--END
		-- checking unique vin no.
		IF NOT EXISTS (
				SELECT TC_TDCarsId
				FROM TC_TDCars WITH (NOLOCK)
				WHERE VinNo = @VinNo
					AND BranchId = @BranchId
					AND IsActive = 1
				)
		BEGIN
			INSERT INTO TC_TDCars (
				BranchId
				,VersionId
				,RegNo
				,KmsDriven
				,CarName
				,VinNo
				)
			VALUES (
				@BranchId
				,@VersionId
				,@RegNo
				,@KmsDriven
				,@CarName
				,@VinNo
				)

			SET @Status = 1

			------------------------below code is added by Nilesh on 17-09-2013 for maintaining logs of Dealers masters for Mobile APP
			INSERT INTO TC_DealerMastersLog (
				DealerId
				,TableName
				,CreatedOn
				)
			VALUES (
				@BranchId
				,'TC_TDCars'
				,GETDATE()
				)
				----------------------------------------------------------------------------------------------------------------------
		END
		ELSE
		BEGIN
			SET @Status = 3 --vin no already exist
		END
	END
	ELSE --IF @TDCarsId contaiing data, we updatig TC_TDCars information
	BEGIN
		-- checking unique vin no.
		IF NOT EXISTS (
				SELECT TC_TDCarsId
				FROM TC_TDCars WITH (NOLOCK)
				WHERE VinNo = @VinNo
					AND BranchId = @BranchId
					AND TC_TDCarsId <> @TDCarsId
					AND IsActive = 1
				)
		BEGIN
			UPDATE TC_TDCars
			SET BranchId = @BranchId
				,VersionId = @VersionId
				,RegNo = @RegNo
				,VinNo = @VinNo
				,KmsDriven = @KmsDriven
				,CarName = @CarName
			WHERE TC_TDCarsId = @TDCarsId

			SET @Status = 2

			------------------------below code is added by Nilesh on 17-09-2013 for maintaining logs of Dealers masters for Mobile APP
			INSERT INTO TC_DealerMastersLog (
				DealerId
				,TableName
				,CreatedOn
				)
			VALUES (
				@BranchId
				,'TC_TDCars'
				,GETDATE()
				)
				----------------------------------------------------------------------------------------------------------------------
		END
		ELSE
		BEGIN
			SET @Status = 3 --vin no already exist
		END
	END
END
