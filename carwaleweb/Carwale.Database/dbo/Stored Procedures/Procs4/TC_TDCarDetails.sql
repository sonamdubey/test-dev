IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDCarDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDCarDetails]
GO

	-- =============================================
-- Author:		Binumon George
-- Create date: 18 Jun, 2012
-- Description:	Getting TDCardetails for editing.
-- Modified By : Tejashree Patil on 30-10-2014, Added @ApplicationId to identify application and joined with vwAllMMV view.
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDCarDetails]
	-- Add the parameters for the stored procedure here
	@TC_TDCarId INT,
	@BranchId INT,
	@ApplicationId TINYINT = 1
AS
BEGIN
	--EXEC Classified_GetMakes
	IF(@TC_TDCarId IS NOT NULL)
	BEGIN
		SELECT	TDC.TC_TDCarsId, TDC.KmsDriven, TDC.RegNo,TDC.VinNo, TDC.VersionId, V.MakeId MakeId, V.ModelId ModelId
		FROM	TC_TDCars TDC WITH(NOLOCK)
		--LEFT JOIN CarVersions Ve WITH(NOLOCK) On Ve.Id= TDC.VersionId 
		--LEFT JOIN CarModels Mo WITH(NOLOCK) On Mo.Id=Ve.CarModelId 
		--LEFT JOIN CarMakes Ma WITH(NOLOCK) On Ma.Id=Mo.CarMakeId
				LEFT JOIN vwAllMMV V ON V.VersionId = TDC.VersionId
		WHERE	TDC.TC_TDCarsId = @TC_TDCarId 
				AND TDC.BranchId=@BranchId 
				AND V.ApplicationId = @ApplicationId
	END
END

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



