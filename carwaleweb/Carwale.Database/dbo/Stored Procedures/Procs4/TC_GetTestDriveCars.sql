IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetTestDriveCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetTestDriveCars]
GO

	-- Author:		Binu
-- Create date: 19 Jun 2012
-- Description:	This Procedure will return testdrive cars tabele base on Make
-- Modified By: Nilesh Utture on 9th August, 2013 Added ModelId in SELECT statement as required for Android app API
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetTestDriveCars] 
@BranchId BIGINT,
@Source TINYINT = NULL
AS
BEGIN
	IF @Source = 1 -- Request from API (Mobile App)
	BEGIN
		SELECT TDC.TC_TDCarsId AS Id, TDC.CarName AS Name, V.ModelId, TDC.VersionId FROM TC_TDCars  TDC
		JOIN vwMMV V ON TDC.VersionId = V.VersionId
		WHERE IsActive=1 AND TDC.BranchId=@BranchId
		ORDER BY TDC.CarName ASC
	END 
	ELSE
	BEGIN
		SELECT TDC.TC_TDCarsId, TDC.CarName FROM TC_TDCars  TDC
		WHERE IsActive=1 AND TDC.BranchId=@BranchId
		ORDER BY TDC.CarName ASC
	END
	
END








/****** Object:  StoredProcedure [dbo].[TC_SellerInquiryDetailsForAPI]    Script Date: 09/17/2013 18:41:47 ******/
SET ANSI_NULLS ON
