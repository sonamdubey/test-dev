IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SMBE_SaveFuel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SMBE_SaveFuel]
GO

	-- =============================================
-- Author:	RAHUL KUMAR
-- Create date: 17-DEC-2013
-- Description:	Save Engine fuel,transmission type,EngineType
-- =============================================
CREATE PROCEDURE [dbo].[OLM_SMBE_SaveFuel]
	-- Add the parameters for the stored procedure here
	@Id NUMERIC
	,@EngineFuelTrans VARCHAR(20)
	,@FuelType SMALLINT
	,@TransmissionType SMALLINT
	,@EngineType VARCHAR(15)
AS
BEGIN
	UPDATE OLM_BookingData
	SET EngineFuelTrans = @EngineFuelTrans
		,FuelType = @FuelType
		,TransmissionType = @TransmissionType
		,EngineType = @EngineType
	WHERE Id = @Id
END