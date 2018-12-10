IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[ms].[GetCarFuelType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [ms].[GetCarFuelType]
GO

	
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		chetan Kane
-- Create date: 8th August 2012
-- Description:	This Sp Returns body type of car
-- =============================================
CREATE PROCEDURE [ms].[GetCarFuelType]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		SELECT FuelTypeId,FuelTypeName FROM TC_CarFuelType
END



