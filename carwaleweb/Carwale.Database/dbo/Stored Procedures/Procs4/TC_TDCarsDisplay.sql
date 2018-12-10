IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDCarsDisplay]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDCarsDisplay]
GO

	
-- ================================================================================
-- Author:		Tejashree Patil
-- Create date: 22 June 2012
-- Description:	This will return all records of test drive cars for given branch
-- Modified By: Vishal Srivastava AE1830 on 03-03-2014 1742 HRS IST
-- Modified By : Suresh Prajapati on 19th April, 2016
-- Description : 1. Changed @BranchId Data-type to INT from BIGINT
--				 2. Removed alias for TDCarId
--				 3. Fetched VersionId and VIN Number
-- EXECUTE [TC_TDCarsDisplay] 5
-- ================================================================================
CREATE PROCEDURE [dbo].[TC_TDCarsDisplay] @BranchId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT C.TC_TDCarsId --AS TDCarId
		,C.CarName
		,C.RegNo
		,C.KmsDriven
		--,C.HavingIssue
		,C.VersionId
		,C.VinNo
	FROM TC_TDCars AS C WITH (NOLOCK)
	WHERE C.BranchId = @BranchId
		AND IsActive = 1
END
