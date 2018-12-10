IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SkodaCarSpecs]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SkodaCarSpecs]
GO

	

-- =============================================
-- Author:		Vaibhav K
-- Create date: 03-Apr-2012
-- Description:	Inserts skoda car specifications in table SkodaCarsPriceSpecs for versonId
-- =============================================
CREATE PROCEDURE [dbo].[OLM_SkodaCarSpecs]
	-- Add the parameters for the stored procedure here
	@VersionId			NUMERIC,
	@FuelType			VARCHAR(50),
	@Transmission		VARCHAR(50),
	@TransmissionLarge	VARCHAR(100),
	@Price				VARCHAR(50),
	@Engine				VARCHAR(100),
	@Torque				VARCHAR(50),
	@Displacement		NUMERIC,
	@Power				VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO SkodaCarsPriceSpecs(VersionId,FuelType,Transmission,Transmission_large
				,Price,Engine,Torque,Displacement,Power,IsActive)
	VALUES(@VersionId,@FuelType,@Transmission,@TransmissionLarge,@Price,
				@Engine,@Torque,@Displacement,@Power,1)
END


