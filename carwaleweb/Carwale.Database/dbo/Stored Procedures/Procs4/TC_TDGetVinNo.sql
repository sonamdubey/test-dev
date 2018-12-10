IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDGetVinNo]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDGetVinNo]
GO

	
-- =============================================
-- Author:		Binumon George
-- Create date: 29-06-2012
-- Description:	Get vin No of TD Car
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDGetVinNo]
	@TC_TDCarId INT,
	@BranchId BIGINT,
	@VinNo VARCHAR(50)OUTPUT
AS
BEGIN
	SELECT @VinNo=VinNo FROM TC_TDCars WHERE TC_TDCarsId=@TC_TDCarId AND BranchId=@BranchId
END


