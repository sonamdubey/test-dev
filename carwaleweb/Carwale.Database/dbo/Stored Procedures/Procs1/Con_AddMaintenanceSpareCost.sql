IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_AddMaintenanceSpareCost]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_AddMaintenanceSpareCost]
GO

	-- =============================================
-- Author:		<Deepak Tripathi>
-- Create date: <21-July-2008>
-- Description:	<Add SpareParts>
-- =============================================
CREATE PROCEDURE [dbo].[Con_AddMaintenanceSpareCost]
	-- Add the parameters for the stored procedure here
	@Id				NUMERIC,
	@VersionId		NUMERIC,
	@SPId			NUMERIC,
	@Quantity		DECIMAL(12,2),
	@UnitId			INT,
	@IsFixed		BIT,
	@TotalCost		DECIMAL(12,2),
	@LabourCharges	DECIMAL(12,2),
	@LastUpdate		DATETIME,
	@Status			NUMERIC OUTPUT
AS
BEGIN
	IF @Id = -1
		BEGIN
			INSERT INTO Con_SpareCost
				(VersionId, SPId, Quantity, UnitId, IsFixed, TotalCost, LabourCharges, LastUpdate, IsActive)
			VALUES
				(@VersionId, @SPId, @Quantity, @UnitId, @IsFixed, @TotalCost, @LabourCharges, @LastUpdate, 1)
			
			SET @Status = 1
		END
		
	ELSE
		BEGIN
			UPDATE Con_SpareCost 
			SET SPId = @SPId, Quantity = @Quantity, UnitId = @UnitId, IsFixed = @IsFixed,
				TotalCost = @TotalCost, LabourCharges = @LabourCharges, LastUpdate = @LastUpdate
			WHERE Id = @Id

			SET @Status = 1
		END
END
